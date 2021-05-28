using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LocantaApp.Core;
using LocantaApp.Data;

namespace LocantaApp.Pages.R2
{
    public class IndexModel : PageModel
    {
        private readonly LocantaApp.Data.LocantaAppDbContext _context;

        public IndexModel(LocantaApp.Data.LocantaAppDbContext context)
        {
            _context = context;
        }

        public IList<Restaurant> Restaurant { get;set; }

        public async Task OnGetAsync()
        {
            Restaurant = await _context.Restaurants.ToListAsync();
        }
    }
}
