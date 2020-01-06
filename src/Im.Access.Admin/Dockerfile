FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Im.Access.Admin/Im.Access.Admin.csproj", "src/Im.Access.Admin/"]
COPY ["Im.Access.Admin.BusinessLogic/Im.Access.Admin.BusinessLogic.csproj", "src/Im.Access.Admin.BusinessLogic/"]
COPY ["Im.Access.EntityFramework/Im.Access.EntityFramework.csproj", "src/Im.Access.EntityFramework/"]
COPY ["Im.Access.EntityFramework.Extensions/Im.Access.EntityFramework.Extensions.csproj", "src/Im.Access.EntityFramework.Extensions/"]
COPY ["Im.Access.Admin.BusinessLogic.Shared/Im.Access.Admin.BusinessLogic.Shared.csproj", "src/Im.Access.Admin.BusinessLogic.Shared/"]
COPY ["Im.Access.Admin.BusinessLogic.Identity/Im.Access.Admin.BusinessLogic.Identity.csproj", "src/Im.Access.Admin.BusinessLogic.Identity/"]
COPY ["Im.Access.EntityFramework.Identity/Im.Access.EntityFramework.Identity.csproj", "src/Im.Access.EntityFramework.Identity/"]
COPY ["Im.Access.EntityFramework.Shared/Im.Access.EntityFramework.Shared.csproj", "src/Im.Access.EntityFramework.Shared/"]
RUN dotnet restore "src/Im.Access.Admin/Im.Access.Admin.csproj"
COPY . .
WORKDIR "/src/Im.Access.Admin"
RUN dotnet build "Im.Access.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Im.Access.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "Im.Access.Admin.dll"]
