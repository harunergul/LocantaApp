# LocantaApp
 A simple reference `ASP.NET Core` App for restaurants. This app is created for the educational purposes.

 Creating application by 
```bash
$ dotnet new webapp 
```


### `.csproj` files

These files represents project. It tells .NET how to build the project.All .NET projects list their dependencies in the `.csproj` file. When you run `dotnet restore`, it uses this file to figure out which *NuGet* packages to download and copy to the project folder.

The `.csproj` file also contains all the information that .NET tooling needs to build the project. It includes the type of the project being built (console, web, desktop, etc.), the platform this project targets and any dependencies on other projects or 3rd party libraries.