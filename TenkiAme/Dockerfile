
##Base Stage
#Specify the base image as ASP.NET runtime image based on .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

#Set the user to non-root
USER app

#Set the working directory inside the container where the application will be located
WORKDIR /app

#Expose ports 8080 and 8081 for incoming connections
EXPOSE 8080

##Publish Stage
#Specify a .NET sdk image for building/publishing apps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish

#Defines an argument that can be passed during the build process to specify the build config (the default used is Release)
ARG BUILD_CONFIGURATION=Release

#Ses the working directory inside the container where the source code will be copied into
WORKDIR /src/TenkiAme

#Copy all the project files
COPY . .

#Restores the NuGet packages specified in the project file
RUN dotnet restore "./TenkiAme.csproj"

#Publish the application to the `app/publish` directory. /p:UseAppHost=false prevents the creation of an executable.
RUN dotnet publish "./TenkiAme.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

##Final Stage
#Inherit from the base stage to use the runtime environment
FROM base AS final

#Set the working directory in the container to app
WORKDIR /app

#Copy the published application from the publish stage into the final image
COPY --from=publish /app/publish .

#Set the command to run when the container starts, which is to execute the app (`TenkiAme.dll`)
ENTRYPOINT ["dotnet", "TenkiAme.dll"]