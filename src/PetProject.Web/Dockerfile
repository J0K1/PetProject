# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetProject.Web/PetProject.Web.csproj", "src/PetProject.Web/"]
COPY ["src/PetProject.Shared/PetProject.Shared.csproj", "src/PetProject.Shared/"]
COPY ["src/PetProject.User/PetProject.User.csproj", "src/PetProject.User/"]
COPY ["src/PetProject.Game/PetProject.Game.csproj", "src/PetProject.Game/"]
COPY ["src/PetProject.Steam/PetProject.Steam.csproj", "src/PetProject.Steam/"]
RUN dotnet restore "./src/PetProject.Web/PetProject.Web.csproj"
COPY . .
WORKDIR "/src/src/PetProject.Web"
RUN dotnet build "./PetProject.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetProject.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Копируем сертификат внутрь контейнера
COPY ./certs/devcert.pfx /app/certs/devcert.pfx
ENTRYPOINT ["dotnet", "PetProject.Web.dll"]