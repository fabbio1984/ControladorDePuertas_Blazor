# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY . .

# Restaurar dependencias y compilar
RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto 80
EXPOSE 80

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "ControladorDePuertas.dll"]
