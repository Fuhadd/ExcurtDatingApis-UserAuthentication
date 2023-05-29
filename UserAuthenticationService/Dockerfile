#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserAuthenticationService/UserAuthenticationService.csproj", "UserAuthenticationService/"]
RUN dotnet restore "UserAuthenticationService/UserAuthenticationService.csproj"
COPY . .
WORKDIR "/src/UserAuthenticationService"
RUN dotnet build "UserAuthenticationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserAuthenticationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV KEY="2782fabc-4848-45a0-8ce3-e2caf9406df0"
ENTRYPOINT ["dotnet", "UserAuthenticationService.dll"]