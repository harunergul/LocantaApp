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
    "LocantaAppDb": "Data Source=locantaApp.db;"
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

If we only call this command inside *LocantaApp.Data* it will fail. 
```bash
dotnet-ef dbcontext info 
```
When we run above command, the only information the Entity Framework has available to it is the information that is in this project *LocantaApp.Data*. But *LocantaApp.Data* does not contain startup configuration, *ConfigureServices*, which is inside *LocantaApp* Web Project. And *LocantaApp.Data* also does not have *appsettings.json* file which contains connection strings.

This is one of the situations that we are going to face if we separate our data access components and our *DbContext* from the rest of the application.


#### Entity Framework migrations
Migrations are all about keeping a database schema in sync with the models in application. Any time We make a change in our entities, We can these Entity Framework migrations to create the schema changes that we can appy to our database.

```bash
dotnet-ef migrations 
```
* Adding migrations 
```bash
dotnet-ef migrations add initalcreate -s ..\LocantaApp\LocantaApp.csproj
```

After calling this command we can see our migration inside `Migration` folder. Entity Framework doing some bookkeeping, the most interesting file in here is `...initalcreate.cs` file, that was the name that we gave our migration. And in here, we can see that the Entity Framework created some C# code that invokes an API on this `MigrationBuilder` object that will do things like create new tables.

Entity Framework sees our class in DbSet and decide to create a table.
 
![Migration folder screenshot](screens/migration-folder.png?raw=true "dotnet -ef info command")


* For listing migrations
```bash
dotnet-ef migrations list -s ..\LocantaApp\LocantaApp.csproj
```

![executing dotnet -ef info command ](screens/ef-migration.png?raw=true "dotnet -ef info command")


* Applying migrations to Database
```bash
dotnet-ef database update  -s ..\LocantaApp\LocantaApp.csproj
```


![After applying migrations ](screens/applying-migrations.png?raw=true "dotnet -ef info command")


## Razor Pages


In ASP.NET Core, if an `.cshtml` file begins with `_` underscore, which means they are partial view. So they should not include `@page` directive.


## View Components

View components are similar to partial views, but they're much more powerful. View components don't use model binding, and only depend on the data provided when calling into it. 

View components are intended anywhere you have reusable rendering logic that's too complex for a partial view, such as:

* Dynamic navigation menus
* Tag cloud (where it queries the database)
* Login panel
* Shopping cart
* Recently published articles
* Sidebar content on a typical blog
* A login panel that would be rendered on every page and show either  the links to log out or log in, depending on the log in state of - the user

One Significant difference between razor pages and view components is that a view component doesn't respond to an HTTP requests. A view componet is like a partial view. 


As you in below *ViewComponent* we have *Invoke* method which acts like a controller, it
gets data create a model and pass that model to *View* component. 
RestaurantCountViewComponent uses injected service and make data access then creates it's model.

```csharp
namespace LocantaApp.ViewComponents
{
    public class RestaurantCountViewComponent : ViewComponent
    {
        private readonly IRestaurantData restaurantData;
        public RestaurantCountViewComponent(IRestaurantData restaurantData)
        {
            this.restaurantData  =  restaurantData;
        }

        public IViewComponentResult Invoke()
        {
            var count = restaurantData.GetCount();
            return View(count);
        }
    }
}
```