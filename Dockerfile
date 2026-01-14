# 1 Сборка проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["ToDoWebApplication.csproj", "./"]
COPY ["../ToDoWebApplication.Application/ToDoWebApplication.Application.csproj", "../ToDoWebApplication.Application/"]
COPY ["../ToDoWebApplication.Contracts/ToDoWebApplication.Contracts.csproj", "../ToDoWebApplication.Contracts/"]
COPY ["../ToDoWebApplication.Domain/ToDoWebApplication.Domain.csproj", "../ToDoWebApplication.Domain/"]
COPY ["../ToDoWebApplication.Infrastructure/ToDoWebApplication.Infrastructure.csproj", "../ToDoWebApplication.Infrastructure/"]

RUN dotnet restore "ToDoWebApplication.csproj"

# Копируем всё остальное
COPY . .

WORKDIR "/src/ToDoWebApplication"
RUN dotnet publish "ToDoWebApplication.csproj" -c Release -o /app/publish

# 2 Runtime контейнер
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ToDoWebApplication.dll"]
