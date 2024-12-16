# Sauvegarder l'emplacement initial
$initialLocation = Get-Location
$ErrorActionPreference = "Stop"

try {
    # Créer le projet
    Write-Host "Creating new WebAPI project..."
    dotnet new webapi --name YesLocation.Api --output ./src/YesLocation.Api
    Set-Location ./src/YesLocation.Api

    # Installer les packages nécessaires avec versions spécifiques
    $packages = @(
        @{Name="Microsoft.EntityFrameworkCore"; Version="8.0.2"},
        @{Name="Microsoft.EntityFrameworkCore.Design"; Version="8.0.2"},
        @{Name="Pomelo.EntityFrameworkCore.MySql"; Version="8.0.1"},
        @{Name="Microsoft.AspNetCore.Authentication.JwtBearer"; Version="8.0.2"},
        @{Name="itext7"; Version="8.0.2"},
        @{Name="itext7.pdfhtml"; Version="5.0.2"}
    )

    Write-Host "Installing packages..."
    foreach ($package in $packages) {
        Write-Host "Installing package: $($package.Name) version $($package.Version)"
        dotnet add package $package.Name --version $package.Version
        Start-Sleep -Seconds 1
    }
    Set-Location ../..

    Write-Host "Creating new class libraries..."
    dotnet new classlib --name YesLocation.Domain --output ./src/YesLocation.Domain
    dotnet new classlib --name YesLocation.Infrastructure --output ./src/YesLocation.Infrastructure
    dotnet new classlib --name YesLocation.Application --output ./src/YesLocation.Application

    Write-Host "Adding references..."
    Set-Location ./src/YesLocation.Api
    dotnet add reference ../YesLocation.Application/YesLocation.Application.csproj
    dotnet add reference ../YesLocation.Infrastructure/YesLocation.Infrastructure.csproj
    
    Set-Location ../YesLocation.Application
    dotnet add reference ../YesLocation.Domain/YesLocation.Domain.csproj
    
    Set-Location ../YesLocation.Infrastructure
    dotnet add reference ../YesLocation.Domain/YesLocation.Domain.csproj

    Write-Host "Project setup completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "Une erreur s'est produite : $_" -ForegroundColor Red
}
finally {
    # Retourner à l'emplacement initial
    Set-Location $initialLocation
}