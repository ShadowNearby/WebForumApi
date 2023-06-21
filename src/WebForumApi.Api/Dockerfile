##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["./Directory.Packages.props", "./"]
COPY ["src/WebForumApi.Api/WebForumApi.Api.csproj", "src/WebForumApi.Api/"]
COPY ["src/WebForumApi.Application/WebForumApi.Application.csproj", "src/WebForumApi.Application/"]
COPY ["src/WebForumApi.Domain/WebForumApi.Domain.csproj", "src/WebForumApi.Domain/"]
COPY ["src/WebForumApi.Infrastructure/WebForumApi.Infrastructure.csproj", "src/WebForumApi.Infrastructure/"]
COPY . .
WORKDIR "/src/src/WebForumApi.Api"

FROM build AS publish
RUN dotnet publish "WebForumApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebForumApi.Api.dll"]