using LocantaApp.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LocantaApp.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly LocantaAppDbContext dbCtx;
        public SqlRestaurantData(LocantaAppDbContext dbCtx)
        {
            this.dbCtx = dbCtx;

        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            dbCtx.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            return dbCtx.SaveChanges();
        }

        public Restaurant Delete(int restaurantId)
        {
            var restaurant = GetById(restaurantId);
            if (restaurant != null)
            {
                dbCtx.Restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public Restaurant GetById(int restaurantId)
        {

            return dbCtx.Restaurants.Find(restaurantId);
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {

            var query = from r in dbCtx.Restaurants
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name select r;
            return query;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = dbCtx.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }
}
