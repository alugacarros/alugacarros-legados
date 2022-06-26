FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AlugaCarros.Legados/AlugaCarros.Legados.Api.csproj", "./AlugaCarros.Legados/"]

RUN dotnet restore "AlugaCarros.Legados/AlugaCarros.Legados.Api.csproj"
COPY . .
WORKDIR "src/AlugaCarros.Legados"
RUN dotnet build "AlugaCarros.Legados.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlugaCarros.Legados.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlugaCarros.Legados.Api.dll"]