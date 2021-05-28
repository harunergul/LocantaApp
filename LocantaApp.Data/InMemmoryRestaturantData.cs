using LocantaApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocantaApp.Data
{
    public class InMemmoryRestaturantData : IRestaurantData
    {

        List<Restaurant> restaurants;
        public InMemmoryRestaturantData()
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

        public Restaurant Add(Restaurant newRestaurant)
        {
            newRestaurant.Id = restaurants.Max(r => r.Id)+1;
            restaurants.Add(newRestaurant);
            return newRestaurant; 
        }

        public Restaurant Delete(int restaurantId)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id== restaurantId);
            if(restaurant!=null)
            {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }
    }
}
