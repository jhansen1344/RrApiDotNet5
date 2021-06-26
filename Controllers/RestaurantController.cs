using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterAPI.Models;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : Controller
    {
        private RestaurantDbContext _context;
        public RestaurantController(RestaurantDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromForm] RestaurantEdit model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Restaurants.Add(new Restaurant(){
                Name = model.Name,
                Location = model.Location
            });

            await _context.SaveChangesAsync();

            return Ok();
            
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.Ratings)
                .ToListAsync();

            List<RestaurantListItem> restaurantList = restaurants.Select(r =>
                new RestaurantListItem(){
                    Id = r.Id,
                    Name = r.Name,
                    Location = r.Location,
                    AverageScore = r.AverageRating
                }).ToList();

            return Ok(restaurantList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(restaurant is null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromForm] RestaurantEdit model, [FromRoute] int id)
        {
            var oldRestaurant = await _context.Restaurants.FindAsync(id);

            if(oldRestaurant == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!string.IsNullOrWhiteSpace(model.Name)){
                oldRestaurant.Name = model.Name;
            }
            if(!string.IsNullOrWhiteSpace(model.Location))
            {
                oldRestaurant.Location = model.Location;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if(restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    
}