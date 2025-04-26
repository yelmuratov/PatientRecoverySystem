# Use the official .NET 9 SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

WORKDIR /app/src/PatientRecoverySystem.NotificationsService
RUN dotnet restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "PatientRecoverySystem.NotificationsService.dll"]
