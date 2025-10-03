# ====== Build stage (.NET 8) ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia sólo el csproj para cachear restore
COPY DoorController/DoorController.csproj DoorController/
RUN dotnet restore DoorController/DoorController.csproj

# Copia el resto del código
COPY . .

# Publica en Release
RUN dotnet publish DoorController/DoorController.csproj -c Release -o /app/publish

# ====== Runtime stage (.NET 8) ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
# Usa ENV para la cadena de conexión si quieres
# ENV ConnectionStrings__Default="Data Source=app_data/doors.db"

ENTRYPOINT ["dotnet", "DoorController.dll"]
