FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Im.Access.Portal/Im.Access.Portal.csproj", "src/Im.Access.Portal/"]
COPY ["src/Im.Access.EntityFramework.Identity/Im.Access.EntityFramework.Identity.csproj", "src/Im.Access.EntityFramework.Identity/"]
COPY ["src/Im.Access.EntityFramework/Im.Access.EntityFramework.csproj", "src/Im.Access.EntityFramework/"]
COPY ["src/Im.Access.EntityFramework.Extensions/Im.Access.EntityFramework.Extensions.csproj", "src/Im.Access.EntityFramework.Extensions/"]
COPY ["src/Im.Access.EntityFramework.Shared/Im.Access.EntityFramework.Shared.csproj", "src/Im.Access.EntityFramework.Shared/"]
RUN dotnet restore "src/Im.Access.Portal/Im.Access.Portal.csproj"
COPY . .
WORKDIR "/src/src/Im.Access.Portal"
RUN dotnet build "Im.Access.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Im.Access.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Im.Access.Portal.dll"]