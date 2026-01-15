# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты
COPY ToDoWebApplication/ ToDoWebApplication/
COPY ToDoWebApplication.Application/ ToDoWebApplication.Application/
COPY ToDoWebApplication.Domain/ ToDoWebApplication.Domain/
COPY ToDoWebApplication.Contracts/ ToDoWebApplication.Contracts/
COPY ToDoWebApplication.Infrastructure/ ToDoWebApplication.Infrastructure/

# Restore
RUN dotnet restore ToDoWebApplication/ToDoWebApplication.csproj

# Publish
RUN dotnet publish ToDoWebApplication/ToDoWebApplication.csproj \
    -c Release \
    -o /app/publish

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ToDoWebApplication.dll"]
