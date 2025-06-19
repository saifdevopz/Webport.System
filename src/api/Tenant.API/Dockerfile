# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/api/Tenant.API/Tenant.API.csproj", "src/api/Tenant.API/"]
COPY ["src/modules/tenant/Tenant.Infrastructure/Tenant.Infrastructure.csproj", "src/modules/tenant/Tenant.Infrastructure/"]
COPY ["src/common/Common.Infrastructure/Common.Infrastructure.csproj", "src/common/Common.Infrastructure/"]
COPY ["src/common/Common.Application/Common.Application.csproj", "src/common/Common.Application/"]
COPY ["src/common/Common.Domain/Common.Domain.csproj", "src/common/Common.Domain/"]
COPY ["src/modules/tenant/Tenant.Application/Tenant.Application.csproj", "src/modules/tenant/Tenant.Application/"]
COPY ["src/modules/tenant/Tenant.Domain/Tenant.Domain.csproj", "src/modules/tenant/Tenant.Domain/"]
COPY ["src/modules/tenant/Tenant.Presentation/Tenant.Presentation.csproj", "src/modules/tenant/Tenant.Presentation/"]
COPY ["src/common/Common.Presentation/Common.Presentation.csproj", "src/common/Common.Presentation/"]
RUN dotnet restore "./src/api/Tenant.API/Tenant.API.csproj"
COPY . .
WORKDIR "/src/src/api/Tenant.API"
RUN dotnet build "./Tenant.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tenant.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tenant.API.dll"]