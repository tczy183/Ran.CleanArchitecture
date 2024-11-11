FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/src/Web/Web.csproj", "services/src/Web/"]
COPY ["framework/Ran.Core/Ran.Core.csproj", "framework/Ran.Core/"]
COPY ["services/src/Infrastructure/Infrastructure.csproj", "services/src/Infrastructure/"]
COPY ["services/src/Application/Application.csproj", "services/src/Application/"]
COPY ["services/src/Domain/Domain.csproj", "services/src/Domain/"]
RUN dotnet restore "services/src/Web/Web.csproj"
COPY . .
WORKDIR "/src/services/src/Web"
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
