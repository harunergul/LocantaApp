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


        public IActionResult OnGet(int restaurantId)
        {

            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();

            Restaurant = restaturantData.GetById(restaurantId);
            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();

        }

        public IActionResult OnPost()
        {

            if(ModelState.IsValid)
            {
                Restaurant = restaturantData.Update(Restaurant);
                restaturantData.Commit();
            }
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            return Page();

        }
    }
}
