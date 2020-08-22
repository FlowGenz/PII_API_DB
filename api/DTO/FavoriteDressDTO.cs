using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class FavoriteDressDTO
    {
        public FavoriteDressDTO()
        {
        }
        [Required]
        public string FavoriteId { get; set;}
        [Required]
        public bool IsFavorite { get; set;}
    }
}