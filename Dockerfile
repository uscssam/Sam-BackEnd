FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SAM.Api/SAM.Api.csproj", "SAM.Api/"]
COPY ["SAM.Repositories/SAM.Repositories.Database.csproj", "SAM.Repositories/"]
COPY ["SAM.Entities/SAM.Entities.csproj", "SAM.Entities/"]
RUN dotnet restore "./SAM.Api/SAM.Api.csproj"
COPY . .
WORKDIR "/src/SAM.Api"
RUN dotnet build "./SAM.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SAM.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SAM.Api.dll"]