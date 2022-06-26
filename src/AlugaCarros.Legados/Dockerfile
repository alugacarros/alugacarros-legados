#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./services/identidade/AlugaCarros.Legados.Api/AlugaCarros.Legados.Api.csproj", "./services/identidade/AlugaCarros.Legados.Api/"]
COPY ["./building blocks/core/AgViagens.Core/AgViagens.Core.csproj", "./building blocks/core/AgViagens.Core/"]
COPY ["./building blocks/core/AgViagens.Core.WebApi/AgViagens.Core.WebApi.csproj", "./building blocks/core/AgViagens.Core.WebApi/"]

RUN dotnet restore "services/identidade/AlugaCarros.Legados.Api/AlugaCarros.Legados.Api.csproj"
COPY . .
WORKDIR "services/identidade/AlugaCarros.Legados.Api"
RUN dotnet build "AlugaCarros.Legados.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlugaCarros.Legados.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlugaCarros.Legados.Api.dll"]