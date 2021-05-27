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
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetById(int restaurantId);
        Restaurant Update(Restaurant updatedRestaurant);
        int Commit();
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

        
        public Restaurant GetById(int restaurantId)
        {
            return restaurants.SingleOrDefault(r => r.Id == restaurantId);
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name=null)
        {

            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name select r;
            
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);

            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;

            }
            return restaurant;
        }

        public int Commit()
        {
            return 0;
        }

    }
}
