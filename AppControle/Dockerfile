# Dockerfile para ASP.NET Core 8 API

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AppControle.csproj", "AppControle/"]
RUN dotnet restore "AppControle.csproj"
COPY . .
WORKDIR "/src/AppControle"
RUN dotnet build "AppControle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppControle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppControle.dll"]
