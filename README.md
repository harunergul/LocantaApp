

# LocantaApp
 A simple reference `ASP.NET Core` App for restaurants. This app is created for the educational purposes.

 Creating application by 
```bash
$ dotnet new webapp 
```

<a href="#behind-the-scenes">Internal of ASP.NET Application</a>

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
![Alt text](screens/installedpackages.PNG?raw=true "Installing Entity Framework Core.")

Install `dotnet-ef`  command line tool

```bash 
dotnet tool install --global dotnet-ef
```

Execute following command inside `LocantaApp.Data` project
```bash 
dotnet-ef dbcontext list
```
![DB Context](screens/dbcontext.PNG?raw=true "Installing Entity Framework Core.")

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

![Alt text](screens/dotnet-ef-info.PNG?raw=true "dotnet -ef info command")

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
 
![Migration folder screenshot](screens/migration-folder.PNG?raw=true "dotnet -ef info command")


* For listing migrations
```bash
dotnet-ef migrations list -s ..\LocantaApp\LocantaApp.csproj
```

![executing dotnet -ef info command ](screens/ef-migration.PNG?raw=true "dotnet -ef info command")


* Applying migrations to Database
```bash
dotnet-ef database update  -s ..\LocantaApp\LocantaApp.csproj
```


![After applying migrations ](screens/applying-migrations.PNG?raw=true "dotnet -ef info command")


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


## Scafolding 

Create a folder inside Pages than do the rest

![Adding scaffolded item ](screens/scaffolded-item.png?raw=true "Adding scaffolded item")

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
![Alt text](screens/installedpackages.PNG?raw=true "Installing Entity Framework Core.")

Install `dotnet-ef`  command line tool

```bash 
dotnet tool install --global dotnet-ef
```

Execute following command inside `LocantaApp.Data` project
```bash 
dotnet-ef dbcontext list
```
![Alt text](screens/dbcontext.PNG?raw=true "Installing Entity Framework Core.")

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

![dotnet -ef info command](screens/dotnet-ef-info.PNG?raw=true "dotnet -ef info command")

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
 
![Migration folder screenshot](screens/migration-folder.PNG?raw=true "dotnet -ef info command")


* For listing migrations
```bash
dotnet-ef migrations list -s ..\LocantaApp\LocantaApp.csproj
```

![executing dotnet -ef info command ](screens/ef-migration.PNG?raw=true "dotnet -ef info command")


* Applying migrations to Database
```bash
dotnet-ef database update  -s ..\LocantaApp\LocantaApp.csproj
```


![After applying migrations ](screens/applying-migrations.PNG?raw=true "dotnet -ef info command")


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


## Scafolding 

Create a folder inside Pages than do the rest

![Adding scaffolded item ](screens/scaffolded-item.png?raw=true "Adding scaffolded item")
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
![Installing Entity Framework Core.](screens/entityframeworkcoreinstall.png?raw=true "Installing Entity Framework Core.")

Install all the listed item here
![Installed packages](screens/installedpackages.PNG?raw=true "Installing Entity Framework Core.")

Install `dotnet-ef`  command line tool

```bash 
dotnet tool install --global dotnet-ef
```

Execute following command inside `LocantaApp.Data` project
```bash 
dotnet-ef dbcontext list
```
![DB Context](screens/dbcontext.PNG?raw=true "DB Context.")

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

![dotnet -ef info](screens/dotnet-ef-info.PNG?raw=true "dotnet -ef info command")

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
 
![Migration folder screenshot](screens/migration-folder.PNG?raw=true "dotnet -ef info command")


* For listing migrations
```bash
dotnet-ef migrations list -s ..\LocantaApp\LocantaApp.csproj
```

![executing dotnet -ef info command ](screens/ef-migration.PNG?raw=true "dotnet -ef info command")


* Applying migrations to Database
```bash
dotnet-ef database update  -s ..\LocantaApp\LocantaApp.csproj
```


![After applying migrations ](screens/applying-migrations.PNG?raw=true "dotnet -ef info command")


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


## Scafolding 

Create a folder inside Pages than do the rest

![Adding scaffolded item ](screens/scaffolded-item.png?raw=true "Adding scaffolded item")
 

![Razor Pages Using Entity Framework (CRUD) ](screens/razorpageusingcrud.jpg?raw=true "Razor Pages Using Entity Framework (CRUD)")

![Razor Page (CRUD) ](screens/razorpage-crud-model.PNG?raw=true "Razor Pages(CRUD)")

![Scaffolded Page ](screens/scaffolded-page.JPG?raw=true "Scaffolded Page")


## Enforcing validation on the client using `partial` 

We can embed our scripts using `partial` tag to embed our page. In the below example we embed our validation script which will apply validation rule according to our validation info which we have defined on the entity object. 

![Validation Script Partial](screens/validation-script-partial.JPG?raw=true "Validation Script Partial")


## Creating an API Controller that responde HTTP Requests

Right click on Api folder and add new Item.

![Adding API Controller](screens/api-controller.JPG?raw=true "Adding API Controller")

![Adding API Controller](screens/api-controller-2.JPG?raw=true "Adding API Controller")


## Adding client side libraries

* Using visual studio
![Using visual studio to adding client side libraries](screens/adding-client-side-library.png?raw=true "Using visual studio to adding client side libraries")



* Managing Client Libraries Using npm and NodeJS
  
  * initialize npm project inside startup project
  
  ```csharp
  npm init
  ```
  

![Development and prod scripts](screens/development-vs-prod-script.JPG?raw=true "Development and prod scripts")

By default static files only served from `wwwroot` folder, but this behaviour is configured for this application. If we do not serve any static file we can remove this folder from our application. We can rename this folder and also we can add extra folders for static file handling.


![Development and prod scripts](screens/wwwroot-alternative.JPG?raw=true "Development and prod scripts")


# Behind The Scenes

[HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext "HttpContext Class")

## Middleware

We register our middleware Startup.cs Configure method.




Adds the AuthorizationMiddleware to the specified IApplicationBuilder, which enables authorization capabilities.
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection(); //Adds middleware for redirecting HTTP Requests to HTTPS.
            app.UseStaticFiles(); // Enables static file serving
            /*
            by default wwwroot files content is only available 
            but we can configure and add extra functionality 
            */
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/node_modules") 
            });


            app.UseRouting();
            /*
              Defines a point in the middleware pipeline where routing 
              decisions are made, and an Endpoint is associated with the HttpContext. 
            */

            app.UseAuthorization();

            /*
            Adds the AuthorizationMiddleware to the specified IApplicationBuilder,
             which enables authorization capabilities.
             When authorizing a resource that is routed using endpoint routing, 
             this call must appear between the calls to app.UseRouting() and 
             app.UseEndpoints(...) for the middleware to function correctly.

            */

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.Use(SayHelloMiddleware);
        }

        private RequestDelegate SayHelloMiddleware(RequestDelegate nextMiddleware)
        {
            return async ctx =>
            {
                if (ctx.Request.Path.StartsWithSegments("/hello"))
                {
                    await ctx.Response.WriteAsync("Hello, World!");
                }
                else
                {
                    await nextMiddleware(ctx);
                    // ctx.Response.StatusCode = 200; we can manipulate after other middlewares completed their processing
                }
            };
        }
```

## Logging

Logging configuration is done inside `appsettings.json`. 
```csharp
namespace LocantaApp.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRestaurantData restaurant;
        
        private readonly ILogger<ListModel> Logger;
        
        [BindProperty(SupportsGet =true)] // basic model binding with Page and PageModel
        public string SearchTerm { get; set; }
        
        [TempData]
        public string Message { get; set; }
        public IEnumerable<Restaurant> restaurants;
        public ListModel(IConfiguration config, IRestaurantData restaurant , ILogger<ListModel> logger)
        {
            this.config = config;
            this.restaurant = restaurant;
            this.Logger = logger;
        }
        

        public void OnGet()
        {
            Logger.LogInformation("Do something");
          //  Message = config["AuthorName"];
            this.restaurants = this.restaurant.GetRestaurantsByName(SearchTerm);
        }
    }
}
```


#### Overriding settings
As we see in the configuration file there are several way to configure the ASP.NET core application. We can override existing appsettings using environment variables and command line arguments.


![Configuration](screens/configuration.JPG?raw=true "Configuration")


## Connecting to MySQL

* Install below dependency  inside LocantaApp
```bash
dotnet add package MySql.EntityFrameworkCore --version 5.0.3.1
```

* Create schema with mysql-workbench
![Creating schema](screens/mysql-creating-schema.png?raw=true "Creating schema")


* Inside schama file passconfiguration

```csharp
public void ConfigureServices(IServiceCollection services)
{ 
    services.AddDbContextPool<LocantaAppDbContext>(options => {

        if (_env.IsDevelopment())
        {
            options.UseMySQL("server=localhost;database=locantaappdb;user=appuser;password=Apppasswd123.");
             //or
            options.UseMySQL(Configuration.GetConnectionString("LocantaAppDb"));
        }
        else
        {
            options.UseSqlite(Configuration.GetConnectionString("LocantaAppDb"));
        }
    
    });



    // services.AddSingleton<IRestaurantData, InMemmoryRestaturantData>();
    services.AddScoped<IRestaurantData, SqlRestaurantData>();
    services.AddControllers();
    services.AddRazorPages();
}
```

* Adding migrations 
```bash
dotnet-ef migrations add <migrationName> -s ..\LocantaApp\LocantaApp.csproj
```

* Applying migrations to Database
```bash
dotnet-ef database update  -s ..\LocantaApp\LocantaApp.csproj
```

## Pubslishing Application

Inside the web project directory  execute following command.
```bash
dotnet publish -o c:\temp\LocantaApp
```

### Lunching application from command line (Using MSBuild to execute npm install)
Visit the publish directory and execute command with assembly. 

```bash
$ c:\temp\LocantaApp
$ dotnet LocantaApp.dll
```

Our application will throw an error because there is no `node_module` folder inside this directory. One way to solve the problem is to execute `npm install` inside this directory. But we want to automate our deployment process. So there should be a better way.

Add these item to `.csproj` file.

```xml
<Target Name="PostBuild" AfterTargets="ComputeFilesToPublish">
    <Exec Command="npm install"></Exec>
</Target>
<ItemGroup> 
    <Content Include="node_modules/**" CopyToPublishDirectory="PreserveNewest"/>
</ItemGroup>
```

### Building self-contained application

Previous deployment require dotnet framework installed on the computer. 
We have to specify which runtime version should include.

```bash 
dotnet publish -o c:/temp/LocantaApp-self  --self-contained -r win-x64
```


Check [Runtime identifiers catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog "Runtime identifiers catalog")

we can run applicatin in two ways. One of them is to use computer dotnet run environment. such as 

```bash 
dotnet LocantaApp.dll
```

For the computer has no dotnet environment. (In this case on windows machine)
```bash 
LocantaApp.exe
```

![Runing exe file](screens/exerun.JPG?raw=true "Runnig exe file")