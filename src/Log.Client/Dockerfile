FROM nginx:alpine AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Log.Client/Log.Client.csproj", "Log.Client/"]
RUN dotnet restore "Log.Client/Log.Client.csproj"
COPY . .
WORKDIR /src/Log.Client
RUN dotnet build "Log.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Log.Client.csproj -c Release -o /app/publish \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=true

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["Log.Client/nginx.conf", "/etc/ngix/nginx.conf"]
