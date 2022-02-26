FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Remato.csproj", "./"]
RUN dotnet restore "Remato.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Remato.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Remato.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Remato.dll"]
