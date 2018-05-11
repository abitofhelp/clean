// Project Root Folder (i.e. C:\GIT\Clean)
dotnet new sln -n "Clean"

mkdir src
cd src
dotnet new lib -o Clean.Adapter
dotnet new console -o Clean.Configuration
dotnet new lib -o Clean.Domain
dotnet new lib -o Clean.Shared
dotnet new lib -o Clean.UseCase
cd ..
dotnet sln add src/Clean.Adapter/Clean.Adapter.csproj
dotnet sln add src/Clean.Configuration/Clean.Configuration.csproj
dotnet sln add src/Clean.Domain/Clean.Domain.csproj
dotnet sln add src/Clean.Shared/Clean.Shared.csproj
dotnet sln add src/Clean.UseCase/Clean.UseCase.csproj

mkdir tests
cd tests
dotnet new lib -o Clean.Adapter.UnitTests
dotnet new lib -o Clean.Configuration.UnitTests
dotnet new lib -o Clean.Domain.UnitTests
dotnet new lib -o Clean.Shared.UnitTests
dotnet new lib -o Clean.UseCase.UnitTests
cd ..
dotnet sln add tests/Clean.Adapter.UnitTests/Clean.Adapter.UnitTests.csproj
dotnet sln add tests/Clean.Configuration.UnitTests/Clean.Configuration.UnitTests.csproj
dotnet sln add tests/Clean.Domain.UnitTests/Clean.Domain.UnitTests.csproj
dotnet sln add tests/Clean.Shared.UnitTests/Clean.Shared.UnitTests.csproj
dotnet sln add tests/Clean.UseCase.UnitTests/Clean.UseCase.UnitTests.csproj