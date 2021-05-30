using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocantaApp.Core;
using LocantaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
