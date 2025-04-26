# Use the official .NET 9 SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything early (solution + full src folder)
COPY . .

# Restore and build
WORKDIR /app/src/PatientRecoverySystem.API
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "PatientRecoverySystem.API.dll"]
