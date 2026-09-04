# HomeManagement

Domowa aplikacja klient–serwer składająca się z API w .NET 10, klienta Angular 20 i bazy SQL Server. Jedna instancja aplikacji obsługuje jedno gospodarstwo domowe.

## Dostępne moduły

- logowanie przez ASP.NET Core Identity,
- wspólny kalendarz wydarzeń,
- profil i kolor wydarzeń użytkownika,
- wspólna lista zadań z przypisaniem do domownika.

## Wymagania

- .NET SDK 10,
- Node.js obsługiwany przez Angular 20,
- Docker, Podman albo istniejąca instancja SQL Server,
- repozytorium klienta `HomeManagementWebClient`.

## Uruchomienie bazy

Ustaw silne hasło konta `sa`, a następnie uruchom SQL Server za pomocą Dockera:

```bash
export MSSQL_SA_PASSWORD='your-strong-local-password'
docker compose up -d sqlserver
```

W przypadku Podmana uruchom gniazdo używane przez provider Compose, a następnie użyj tego samego pliku `compose.yaml`:

```bash
systemctl --user start podman.socket
export MSSQL_SA_PASSWORD='your-strong-local-password'
podman compose up -d sqlserver
```

Jeśli nie korzystasz z providera Compose, usługę można uruchomić również bezpośrednio poleceniem `podman run`, zachowując port `1433` i zmienne `ACCEPT_EULA`, `MSSQL_PID` oraz `MSSQL_SA_PASSWORD` z pliku `compose.yaml`.

## Konfiguracja API

Sekrety developerskie nie są przechowywane w repozytorium. Ustaw połączenie z bazą i konto startowe:

```bash
dotnet user-secrets --project HomeManagement/HomeManagement.csproj set \
  'ConnectionStrings:HomeManagementConnection' \
  'Server=localhost,1433;Database=HomeManagement;User Id=sa;Password=your-strong-local-password;TrustServerCertificate=True'
dotnet user-secrets --project HomeManagement/HomeManagement.csproj set 'InitDBSettings:InitDatabaseData' 'true'
dotnet user-secrets --project HomeManagement/HomeManagement.csproj set 'InitDBSettings:Email' 'admin@example.local'
dotnet user-secrets --project HomeManagement/HomeManagement.csproj set 'InitDBSettings:Password' 'your-strong-user-password'
```

Migracje wykonują się przy starcie aplikacji. Uruchom API:

```bash
dotnet run --project HomeManagement/HomeManagement.csproj --launch-profile https
```

API jest dostępne pod `https://localhost:7065/api`, a dokumentacja developerska pod `https://localhost:7065/docs`.

Konto z seeda otrzymuje role administratora i użytkownika. Publiczna rejestracja jest wyłączona: utworzenie kolejnego konta przez `POST /api/auth/register` wymaga tokenu administratora. Endpoint można wywołać z poziomu ReDoc albo klienta HTTP, przekazując nagłówek `Authorization: Bearer {token}`.

## Uruchomienie klienta

W repozytorium Angulara:

```bash
npm ci
npm start
```

Klient jest dostępny pod `http://localhost:4200`.

## Weryfikacja

Backend:

```bash
dotnet restore HomeManagement.sln
dotnet build HomeManagement.sln --no-restore
dotnet test HomeManagement.sln --no-build --no-restore
```

Integracyjny smoke test API uruchamiający tymczasowy SQL Server przez Podmana:

```bash
./scripts/smoke-test.sh
```

Skrypt wymaga poleceń `podman`, `curl`, `jq`, `openssl`, `rg` i `dotnet`. Kontener oraz testowa baza są automatycznie usuwane po zakończeniu. Alternatywny silnik można wskazać zmienną `HM_CONTAINER_ENGINE`.

Frontend:

```bash
npm ci
npm run build
npm test -- --watch=false
```
