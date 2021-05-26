using LocantaApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocantaApp.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
    }

    public class InMemmoryRestaturanData : IRestaurantData
    {

        List<Restaurant> restaurants;
        public InMemmoryRestaturanData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant{Id = 1, Name ="Gunaydin", Location= "Ugur Mumcu", Cuisine= CuisineType.Turkish },
                new Restaurant{Id = 2, Name ="NusrEt", Location= "Cebeci", Cuisine= CuisineType.Mexican },
                new Restaurant{Id = 3, Name ="Develi", Location= "Filistin Caddesi", Cuisine= CuisineType.Turkish }
                


            };


        }
        public IEnumerable<Restaurant> GetAll()
        {
            return this.restaurants;
        }
    }
}
