#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CatalogGraphQL/CatalogGraphQL.csproj", "CatalogGraphQL/"]
RUN dotnet restore "CatalogGraphQL/CatalogGraphQL.csproj"
COPY . .
WORKDIR "/src/CatalogGraphQL"
RUN dotnet build "CatalogGraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatalogGraphQL.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogGraphQL.dll"]