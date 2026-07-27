# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish application
RUN dotnet publish -c Release -o /app/publish


# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final

WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Render expects the application to listen
# on all network interfaces
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# Render's default web service port
EXPOSE 10000

# Start ASP.NET Core application
ENTRYPOINT ["dotnet", "CommunicaAI.dll"]
