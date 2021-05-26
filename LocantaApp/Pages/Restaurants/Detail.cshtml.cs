using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocantaApp.Core;
using LocantaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocantaApp.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        public readonly IRestaurantData RestaurantData;


        public DetailModel(IRestaurantData restaurantData) {
            RestaurantData = restaurantData;
        } 

        public Restaurant Restaurant { get; set; }
        public IActionResult OnGet(int restaurantId)
        {
            Restaurant = new Restaurant();
            Restaurant.Location = "Not found";
            Restaurant = RestaurantData.GetById(restaurantId);
            //RestaurantData.GetRestaurantsById(restaurantId);
            if(Restaurant == null){
                return RedirectToPage("./NotFound");
            }
            return Page();

        }
    }
}
