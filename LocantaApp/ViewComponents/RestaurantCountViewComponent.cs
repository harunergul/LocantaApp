using LocantaApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

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
