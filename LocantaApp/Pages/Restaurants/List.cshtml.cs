using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocantaApp.Core;
using LocantaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LocantaApp.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRestaurantData restaurant;

        [BindProperty(SupportsGet =true)] // basic model binding with Page and PageModel
        public string SearchTerm { get; set; }
        public string Message { get; set; }
        public IEnumerable<Restaurant> restaurants;
        public ListModel(IConfiguration config, IRestaurantData restaurant)
        {
            this.config = config;
            this.restaurant = restaurant;
        }
        

        public void OnGet()
        {
            Message = config["AuthorName"];
            this.restaurants = this.restaurant.GetRestaurantsByName(SearchTerm);
        }
    }
}
