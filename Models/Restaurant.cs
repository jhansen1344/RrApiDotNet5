using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        public virtual List<Rating> Ratings { get; set; }

        public double AverageRating { 
            get
            {
                if (Ratings.Count == 0)
                {
                    return 0;
                }
                double total = 0.0;
                foreach (Rating rating in Ratings)
                {
                    total += rating.Score;
                }
                return total / Ratings.Count;
            }
         }
    }
}