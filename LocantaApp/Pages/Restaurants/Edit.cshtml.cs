using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocantaApp.Data;
using LocantaApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocantaApp.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaturantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty] // Any changes in the form on Restaurant will update Restaurant data.
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaturantData, IHtmlHelper htmlHelper)
        {
            this.restaturantData = restaturantData;
            this.htmlHelper = htmlHelper;
        }


        public IActionResult OnGet(int? restaurantId) //? marks mean restaurantId is nullable
        {

            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();

            if(restaurantId.HasValue)
            {
                Restaurant = restaturantData.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();

            }
            
            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();

        }

        public IActionResult OnPost()
        {

            if(!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
            }

            if (Restaurant.Id > 0)
            {
                Restaurant = restaturantData.Update(Restaurant);
            }
            else
            {
                Restaurant = restaturantData.Add(Restaurant);
            }

            restaturantData.Commit();
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });




        }
    }
}
