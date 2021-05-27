# LocantaApp
 A simple reference `ASP.NET Core` App for restaurants. This app is created for the educational purposes.

 Creating application by 
```bash
$ dotnet new webapp 
```


### `.csproj` files

These files represents project. It tells .NET how to build the project.All .NET projects list their dependencies in the `.csproj` file. When you run `dotnet restore`, it uses this file to figure out which *NuGet* packages to download and copy to the project folder.

The `.csproj` file also contains all the information that .NET tooling needs to build the project. It includes the type of the project being built (console, web, desktop, etc.), the platform this project targets and any dependencies on other projects or 3rd party libraries.

### Creating solution file manually

`.sln` files used by visual studio for managing projects. A `.sln` file can manage multiple project. To manage the project we have to add `.csproj` to `.sln` file

![Alt text](screens/solution.png?raw=true "Adding project to solution.")

## Practical VisualStudio shortcuts
Markdown | Less 
--- | --- 
ctrl + F5 | Run the application without debugger
F7| Change editor from razor page to page model or vice versa.


## Razor
Razor is a markup syntax that lets you embed server-based code  into web pages.

Scaffolding Razor Pages with the ASP.NET Core Code Generator tool

```bash 
dotnet tool install --global dotnet-aspnet-codegenerator
```

installing code generation tool for ASP.NET Core. Contains the dotnet-aspnet-codegenerator command used for generating controllers and views.

```bash 
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
``` 

For more detail 

```bash 
dotnet aspnet-codegenerator razorpage --help
```

#### Generating a page using the Empty template link

`dotnet aspnet-codegenerator razorpage Product Empty -udl -outDir Pages\Product`
udl -> use default layout

## Razor Page vs Page Model

Page model doing all the hardwork such as data access, performing business logic and put the data together for Razor page to display. Razor Page performs view part and also provide a way to embed C# code in web page.

```csharp 
@page
@model ListModel
@{
}

<h1>Restaurants</h1>
```

```csharp 
namespace LocantaApp.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
```


## Adding Models 

One of the way for adding entities to our project is actually creating a seperate project that hold models then we add model project to our existing web project.

In visual studio right click on solution  then select `Add->New Project ...` and select `Class Library`.

Creating project `LocantaApp.Core`.

We also create another project in a same way for data access layer. 

Creating project `LocantaApp.Data`.


## Installing Entity Framework for data access

Install framework by right clicking `LocantaApp.Data` project select `Manage NuGet package` and search entityframeworkcore.
![Alt text](screens/entityframeworkcoreinstall.png?raw=true "Installing Entity Framework Core.")

Install all the listed item here
![Alt text](screens/installedpackages.png?raw=true "Installing Entity Framework Core.")

Install `dotnet-ef`  command line tool

```bash 
dotnet tool install --global dotnet-ef
```

Execute following command inside `LocantaApp.Data` project
```bash 
dotnet-ef dbcontext list
```
![Alt text](screens/dbcontext.png?raw=true "Installing Entity Framework Core.")

Install `sqlite` using cli in `LocantaApp.Data` folder.
```bash 
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 5.0.6
```

We will store connection info inisde `appsettings.json` file which is located inside `LocantaApp`


```json 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "AuthorName": "Harun ERGUL",
  "ConnectionStrings": {
    "LocantaAppDb": "Data Source=locantaApp.db;Password=simplePassword;"
  }
}
```
 
So how `ConnectionStrings` reach to our `LocantaAppDbContext` and we connect the database?
The Answer is Startup.cs's `ConfigureServices` method. We will register our DbContext inside this method.


```csharp 
public void ConfigureServices(IServiceCollection services)
{ 
    services.AddDbContext<LocantaAppDbContext>(options => {
        options.UseSqlite(Configuration.GetConnectionString("LocantaAppDb"));
    });

    ....
}
```

Now execute following command inside _*LocantaApp.Data*_ folder. 
*-s* means startup project.

```bash
dotnet-ef dbcontext info -s ..\LocantaApp\LocantaApp.csproj
```

![Alt text](screens/dotnet-ef-info.png?raw=true "dotnet -ef info command")