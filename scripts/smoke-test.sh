#!/usr/bin/env bash

set -euo pipefail

smoke_engine="${HM_CONTAINER_ENGINE:-podman}"
smoke_sql_port="${HM_SQL_PORT:-1433}"
smoke_api_port="${HM_API_PORT:-5230}"
smoke_container="home-management-smoke-$$"
smoke_sql_password="$(openssl rand -hex 16)Aa1!"
smoke_admin_email="admin@example.local"
smoke_admin_password="AdminUser-Aa1!"
smoke_api_url="http://127.0.0.1:${smoke_api_port}"
smoke_api_log="$(mktemp)"
smoke_api_pid=""

cleanup() {
  if [ -n "$smoke_api_pid" ]; then
    kill "$smoke_api_pid" 2>/dev/null || true
    wait "$smoke_api_pid" 2>/dev/null || true
  fi

  "$smoke_engine" stop "$smoke_container" >/dev/null 2>&1 || true
  unlink "$smoke_api_log" 2>/dev/null || true
}
trap cleanup EXIT

for smoke_command in "$smoke_engine" curl jq openssl dotnet rg; do
  if ! command -v "$smoke_command" >/dev/null; then
    printf 'Brak wymaganego polecenia: %s\n' "$smoke_command" >&2
    exit 1
  fi
done

dotnet build HomeManagement.sln --no-restore >/dev/null

"$smoke_engine" run --rm --detach \
  --name "$smoke_container" \
  --publish "${smoke_sql_port}:1433" \
  --env ACCEPT_EULA=Y \
  --env MSSQL_PID=Developer \
  --env MSSQL_SA_PASSWORD="$smoke_sql_password" \
  mcr.microsoft.com/mssql/server:2022-latest >/dev/null

smoke_sql_ready=false
for smoke_attempt in $(seq 1 90); do
  if "$smoke_engine" logs "$smoke_container" 2>&1 \
    | rg "SQL Server is now ready for client connections" >/dev/null; then
    smoke_sql_ready=true
    break
  fi
  sleep 1
done

if [ "$smoke_sql_ready" != true ]; then
  "$smoke_engine" logs "$smoke_container"
  exit 1
fi

ASPNETCORE_ENVIRONMENT=Development \
ASPNETCORE_URLS="$smoke_api_url" \
ConnectionStrings__HomeManagementConnection="Server=localhost,${smoke_sql_port};Database=HomeManagementSmoke;User Id=sa;Password=${smoke_sql_password};TrustServerCertificate=True" \
InitDBSettings__InitDatabaseData=true \
InitDBSettings__Email="$smoke_admin_email" \
InitDBSettings__Password="$smoke_admin_password" \
dotnet run --project HomeManagement/HomeManagement.csproj --no-build --no-launch-profile \
  >"$smoke_api_log" 2>&1 &
smoke_api_pid="$!"

smoke_api_ready=false
for smoke_attempt in $(seq 1 90); do
  if curl --fail --silent "$smoke_api_url/openapi/v1.json" >/dev/null; then
    smoke_api_ready=true
    break
  fi

  if ! kill -0 "$smoke_api_pid" 2>/dev/null; then
    sed -n '1,260p' "$smoke_api_log"
    exit 1
  fi
  sleep 1
done

if [ "$smoke_api_ready" != true ]; then
  sed -n '1,260p' "$smoke_api_log"
  exit 1
fi

smoke_anonymous_registration_status="$(curl --silent --output /dev/null --write-out '%{http_code}' \
  -H 'Content-Type: application/json' \
  -d '{"email":"anonymous@example.local","password":"Anonymous-Aa1!"}' \
  "$smoke_api_url/api/auth/register")"
test "$smoke_anonymous_registration_status" = 401

smoke_login_response="$(curl --fail --silent \
  -H 'Content-Type: application/json' \
  -d "{\"email\":\"$smoke_admin_email\",\"password\":\"$smoke_admin_password\"}" \
  "$smoke_api_url/api/auth/login")"
smoke_token="$(jq -er '.accessToken' <<<"$smoke_login_response")"
smoke_auth_header="Authorization: Bearer $smoke_token"

smoke_admin_registration_status="$(curl --silent --output /dev/null --write-out '%{http_code}' \
  -H "$smoke_auth_header" \
  -H 'Content-Type: application/json' \
  -d '{"email":"member@example.local","password":"MemberUser-Aa1!"}' \
  "$smoke_api_url/api/auth/register")"
test "$smoke_admin_registration_status" = 200

smoke_profile="$(curl --fail --silent \
  -H "$smoke_auth_header" \
  "$smoke_api_url/api/Profile/me")"
smoke_user_id="$(jq -er '.id' <<<"$smoke_profile")"

smoke_updated_profile="$(curl --fail --silent \
  -X PUT \
  -H "$smoke_auth_header" \
  -H 'Content-Type: application/json' \
  -d '{"calendarEventBackgroundColor":"#123456"}' \
  "$smoke_api_url/api/Profile/me")"
jq -e --arg email "$smoke_admin_email" \
  '.email == $email and .calendarEventBackgroundColor == "#123456"' \
  <<<"$smoke_updated_profile" >/dev/null

for smoke_index in $(seq 1 20); do
  curl --fail --silent \
    -H "$smoke_auth_header" \
    -H 'Content-Type: application/json' \
    -d "{\"title\":\"Smoke event $smoke_index\",\"startDate\":\"2030-01-01T10:00:00+00:00\",\"endDate\":\"2030-01-01T11:00:00+00:00\"}" \
    "$smoke_api_url/api/CalendarEvent" >/dev/null
done

smoke_calendar="$(curl --fail --silent \
  -H "$smoke_auth_header" \
  "$smoke_api_url/api/CalendarEvent")"
test "$(jq 'length' <<<"$smoke_calendar")" = 20

smoke_invalid_event_status="$(curl --silent --output /dev/null --write-out '%{http_code}' \
  -H "$smoke_auth_header" \
  -H 'Content-Type: application/json' \
  -d '{"title":"Invalid event","startDate":"2030-01-01T12:00:00+00:00","endDate":"2030-01-01T11:00:00+00:00"}' \
  "$smoke_api_url/api/CalendarEvent")"
test "$smoke_invalid_event_status" = 400

smoke_work_item="$(curl --fail --silent \
  -H "$smoke_auth_header" \
  -H 'Content-Type: application/json' \
  -d "{\"title\":\"Smoke task\",\"priority\":true,\"isDone\":false,\"assignedToUserId\":\"$smoke_user_id\"}" \
  "$smoke_api_url/api/WorkItem")"
smoke_work_item_id="$(jq -er '.id' <<<"$smoke_work_item")"

smoke_updated_work_item="$(curl --fail --silent \
  -X PUT \
  -H "$smoke_auth_header" \
  -H 'Content-Type: application/json' \
  -d "{\"title\":\"Smoke task updated\",\"priority\":false,\"isDone\":true,\"assignedToUserId\":\"$smoke_user_id\"}" \
  "$smoke_api_url/api/WorkItem/$smoke_work_item_id")"
jq -e '.title == "Smoke task updated" and .isDone == true' \
  <<<"$smoke_updated_work_item" >/dev/null

smoke_delete_status="$(curl --silent --output /dev/null --write-out '%{http_code}' \
  -X DELETE \
  -H "$smoke_auth_header" \
  "$smoke_api_url/api/WorkItem/$smoke_work_item_id")"
test "$smoke_delete_status" = 204

printf '%s\n' \
  'Smoke test zakończony powodzeniem.' \
  '  - migracje i seed: OK' \
  '  - logowanie i profil: OK' \
  '  - rejestracja tylko dla administratora: OK' \
  '  - kalendarz z 20 wydarzeniami: OK' \
  '  - walidacja dat: OK' \
  '  - CRUD zadań: OK'
