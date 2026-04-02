# WorkoutTrackerAPI — Short Local Setup

Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or remote)
- Visual Studio 2026 or any editor

Quick steps

1) Clone repository  
`git clone https://github.com/limpanJJ/WorkoutTrackerAPI.git cd WorkoutTrackerAPI`

2) Create `.env` from example (choose one)  
`cp .env.example .env`  
`copy .env.example .env`

3) Generate a secure secret (recommended base64)  
`rand -base64 32`
`[PowerShell] [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))`

4) Put secret in `.env` (example entries)  
`Jwt__SecretKey=your-generated-base64-secret-here`  
`Jwt__Issuer=WorkoutTrackerAPI`  
`Jwt__Audience=WorkoutTrackerClient`  
(Optional) `Jwt__ExpirationInMinutes=60`

5) Ensure `.env` is ignored by git  
`echo ".env" >> .gitignore`

6) (Optional) Install DotNetEnv if you need the loader  
`dotnet add package DotNetEnv`

7) Configure database connection in `appsettings.json` (example)  
`"ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkoutTrackerDB;Trusted_Connection=true;" }`

8) Apply EF migrations  
`dotnet tool install --global dotnet-ef`  
`dotnet ef database update`
9) Run the app  
`dotnet run`

Notes
- This project reads secrets from `.env` (loaded in `Program.cs`). You may also use environment variables; keep `.env` out of the repo.
