# syntax=docker/dockerfile:1

# The .NET version must match the TargetFramework in Playr.csproj.
ARG DOTNET_VERSION=8.0

# Stage 1: Generate the Tailwind CSS.
FROM node:22-alpine AS frontend

WORKDIR /src

COPY package.json package-lock.json ./
RUN --mount=type=cache,target=/root/.npm npm ci --no-audit --no-fund --loglevel=info

COPY Styles ./Styles
COPY Components ./Components

RUN npm run build:css

# Stage 2: Compile and publish the Blazor application.
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

WORKDIR /src

COPY . .

# Use the CSS generated in the frontend stage.
COPY --from=frontend /src/wwwroot/tailwind.css ./wwwroot/tailwind.css

RUN dotnet restore Playr.csproj
RUN dotnet publish Playr.csproj -c Release -o /app/publish --no-restore

# Stage 3: Run the published application.
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .

# Run without root privileges.
USER $APP_UID

ENTRYPOINT ["dotnet", "Playr.dll"]
