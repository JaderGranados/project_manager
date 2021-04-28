#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["project_manager/project_manager.csproj", "project_manager/"]
COPY ["ProjectManager.BL/project_manager.bl.csproj", "ProjectManager.BL/"]
COPY ["project_manager.data/project_manager.data.csproj", "project_manager.data/"]
COPY ["project_manager.models/project_manager.common.csproj", "project_manager.models/"]
COPY ["project_manager.facade/project_manager.contract.csproj", "project_manager.facade/"]
COPY ["project_manager.emailing/project_manager.emailing.csproj", "project_manager.emailing/"]
RUN dotnet restore "project_manager/project_manager.csproj"
COPY . .
WORKDIR "/src/project_manager"
RUN dotnet build "project_manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "project_manager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "project_manager.dll"]
