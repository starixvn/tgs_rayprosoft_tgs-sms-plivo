#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["Tgs.Sms.Plivo/Tgs.Sms.Plivo.csproj", "Tgs.Sms.Plivo/"]
COPY ["Tgs.Sms.Plivo.Repositories/Tgs.Sms.Plivo.Repositories.csproj", "Tgs.Sms.Plivo.Repositories/"]
COPY ["Tgs.Sms.Plivo.Core/Tgs.Sms.Plivo.Core.csproj", "Tgs.Sms.Plivo.Core/"]
RUN dotnet restore "Tgs.Sms.Plivo/Tgs.Sms.Plivo.csproj"
COPY . .
WORKDIR "/src/Tgs.Sms.Plivo"
RUN dotnet build "Tgs.Sms.Plivo.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Sms.Plivo.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Sms.Plivo.dll"]