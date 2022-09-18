#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
COPY --from=mcr.microsoft.com/dotnet/aspnet:2.1 /usr/share/dotnet /usr/share/dotnet
COPY --from=mcr.microsoft.com/dotnet/aspnet:3.1 /usr/share/dotnet /usr/share/dotnet
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY --from=mcr.microsoft.com/dotnet/sdk:2.1 /usr/share/dotnet /usr/share/dotnet
COPY --from=mcr.microsoft.com/dotnet/sdk:3.1 /usr/share/dotnet /usr/share/dotnet
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/UserRolesAPI/UserRolesAPI.csproj", "src/UserRolesAPI/"]
COPY ["src/UserRolesAPI.Infrastructure/UserRolesAPI.Infrastructure.csproj", "src/UserRolesAPI.Infrastructure/"]
COPY ["src/UserRolesAPI.Core/UserRolesAPI.Core.csproj", "src/UserRolesAPI.Core/"]
COPY ["src/UserRolesAPI.SharedKernel/UserRolesAPI.SharedKernel.csproj", "src/UserRolesAPI.SharedKernel/"]
RUN dotnet restore "src/UserRolesAPI/UserRolesAPI.csproj"
COPY . .
WORKDIR "/src/src/UserRolesAPI"
RUN dotnet build "UserRolesAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserRolesAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/UserRolesAPI/aspnetapp.pfx", "/https/"]
ENTRYPOINT ["dotnet", "UserRolesAPI.dll"]