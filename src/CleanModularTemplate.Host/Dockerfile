FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY --parents src/**/*.csproj ./
COPY ["Directory.Build.props", "Directory.Packages.props", "./"]
RUN dotnet restore "src/CleanModularTemplate.Host/CleanModularTemplate.Host.csproj"
COPY . .
WORKDIR "/src/src/CleanModularTemplate.Host"
RUN dotnet build "./CleanModularTemplate.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanModularTemplate.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanModularTemplate.Host.dll"]
