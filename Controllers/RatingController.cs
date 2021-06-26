using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantRaterAPI.Models;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : Controller
    {
        private RestaurantDbContext _context;
        public RatingController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostRating([FromForm] RatingEdit model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Ratings.Add(new Rating(){
                Score = model.Score,
                RestaurantId = model.RestaurantId
            });

            await _context.SaveChangesAsync();

            return Ok();
        }
        
    }
}