FROM mcr.microsoft.com/dotnet/aspnet:7.0.11-alpine3.18-amd64 AS base
WORKDIR /app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Log.Api/Log.Api.csproj", "Log.Api/"]
RUN dotnet restore "Log.Api/Log.Api.csproj"
COPY . .
WORKDIR "/src/Log.Api"
RUN dotnet build "Log.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Log.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Log.Api.dll"]