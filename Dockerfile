# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только файлы проектов (для лучшего кэширования)
COPY ["ToDoWebApplication/ToDoWebApplication.csproj", "ToDoWebApplication/"]
COPY ["ToDoWebApplication.Application/ToDoWebApplication.Application.csproj", "ToDoWebApplication.Application/"]
COPY ["ToDoWebApplication.Domain/ToDoWebApplication.Domain.csproj", "ToDoWebApplication.Domain/"]
COPY ["ToDoWebApplication.Contracts/ToDoWebApplication.Contracts.csproj", "ToDoWebApplication.Contracts/"]
COPY ["ToDoWebApplication.Infrastructure/ToDoWebApplication.Infrastructure.csproj", "ToDoWebApplication.Infrastructure/"]
COPY ["ToDoWebApplication/ToDoWebApplication.sln", "."]

# Восстанавливаем зависимости основного проекта
RUN dotnet restore "ToDoWebApplication/ToDoWebApplication.csproj"

# Копируем весь исходный код
COPY . .

# Публикуем приложение
RUN dotnet publish "ToDoWebApplication/ToDoWebApplication.csproj" \
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