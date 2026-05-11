FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Reparo.Host/Reparo.Host.csproj", "Reparo.Host/"]
COPY ["Reparo.Client/Reparo.Client.csproj", "Reparo.Client/"]
COPY ["Reparo.Client.Components/Reparo.Client.Components.csproj", "Reparo.Client.Components/"]
COPY ["Reparo.Models/Reparo.Models.csproj", "Reparo.Models/"]
COPY ["Misc/Utilities/Reparo.Utilities.csproj", "Misc/Utilities/"]
COPY ["Reparo.Services/Client/Reparo.Client.Services.csproj", "Reparo.Services/Client/"]
COPY ["Reparo.Services/Server/Reparo.Server.Services.csproj", "Reparo.Services/Server/"]
RUN dotnet restore "./Reparo.Host/Reparo.Host.csproj"
COPY . .
WORKDIR "/src/Reparo.Host"
RUN dotnet build "./Reparo.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Reparo.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reparo.Host.dll"]