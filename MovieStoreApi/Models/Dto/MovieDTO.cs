using System.ComponentModel.DataAnnotations;

namespace MovieStoreApi.Models.Dto
{
    public class MovieDTO
    {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

       
    }
}
