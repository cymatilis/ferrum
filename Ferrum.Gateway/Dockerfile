#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Ferrum.Gateway/Ferrum.Gateway.csproj", "Ferrum.Gateway/"]
RUN dotnet restore "Ferrum.Gateway/Ferrum.Gateway.csproj"
COPY . .
WORKDIR "/src/Ferrum.Gateway"
RUN dotnet build "Ferrum.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ferrum.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ferrum.Gateway.dll"]