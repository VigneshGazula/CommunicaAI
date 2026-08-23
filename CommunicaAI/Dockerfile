# ==============================
# BUILD
# ==============================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY CommunicaAI.csproj ./
RUN dotnet restore "CommunicaAI.csproj"

COPY . .
RUN dotnet publish "CommunicaAI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ==============================
# RUNTIME
# ==============================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "CommunicaAI.dll"]
