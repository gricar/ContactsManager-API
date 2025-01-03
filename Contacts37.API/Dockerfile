FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Contacts37.API/Contacts37.API.csproj", "Contacts37.API/"]
COPY ["Contact37.Persistence/Contacts37.Persistence.csproj", "Contact37.Persistence/"]
COPY ["Contacts37.Application/Contacts37.Application.csproj", "Contacts37.Application/"]
COPY ["Contacts37.Domain/Contacts37.Domain.csproj", "Contacts37.Domain/"]
RUN dotnet restore "./Contacts37.API/./Contacts37.API.csproj"
COPY . .
WORKDIR "/src/Contacts37.API"
RUN dotnet build "./Contacts37.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Contacts37.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contacts37.API.dll"]