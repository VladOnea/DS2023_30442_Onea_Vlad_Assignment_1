FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EnergyManagamentUser/EnergyManagamentUser.csproj", "EnergyManagamentUser/"]
RUN dotnet restore "EnergyManagamentUser/EnergyManagamentUser.csproj"
COPY . .
WORKDIR "/src/EnergyManagamentUser"
RUN dotnet build "EnergyManagamentUser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnergyManagamentUser.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnergyManagamentUser.dll"]
