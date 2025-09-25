using MovieStoreApi.Models.Dto;

namespace MovieStoreApi.Data
{
    public class MovieStore
    {
        public static List<MovieDTO> MovieList = new List<MovieDTO> {
                new MovieDTO { Id = 1, Title = "Devara" },
                new MovieDTO { Id = 2, Title = "Dragon" }
            };
    }
}
