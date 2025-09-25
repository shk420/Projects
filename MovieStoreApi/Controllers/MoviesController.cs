using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MovieStoreApi.Data;
using MovieStoreApi.Models;
using MovieStoreApi.Models.Dto;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace MovieStoreApi.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MovieDTO>> GetMovies()
        {
            return Ok(MovieStore.MovieList);
        }

        [HttpGet("{Id:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MovieDTO> GetMovie(int Id)


        {
            var movie = MovieStore.MovieList.SingleOrDefault(i => i.Id == Id);
            if (Id == 0)
            {
                return BadRequest();
            }
            ;
            if (movie == null)
            {
                return NotFound();
            }
            ;
            return Ok(movie);

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<MovieDTO> PostMovie([FromBody] MovieDTO movieDTO)
        {
            if (MovieStore.MovieList.FirstOrDefault(u => u.Title.ToLower() == movieDTO.Title.ToLower()) != null)
            {
                ModelState.AddModelError(" Duplicate error:- ", " Movie already exits ");
                return BadRequest(ModelState);

            }
            if (movieDTO == null)
            {
                return BadRequest(movieDTO);
            }
            if (movieDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            movieDTO.Id = MovieStore.MovieList.OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            MovieStore.MovieList.Add(movieDTO);

            return CreatedAtRoute("GetMovie", new { Id = movieDTO.Id }, movieDTO);

        }


        [HttpDelete("{Id:int}", Name = " DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public IActionResult DeleteMovie(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            var movie = MovieStore.MovieList.FirstOrDefault(i => i.Id == Id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieStore.MovieList.Remove(movie);
            return NoContent();
        }

        [HttpPut("Id:int" , Name = " UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]


        public IActionResult UpdateMovie(int Id, [FromBody] MovieDTO movieDTO)
        {
            if (movieDTO == null || Id != movieDTO.Id)
            {
                return BadRequest();
            }
            var movie = MovieStore.MovieList.FirstOrDefault(i => i.Id == Id);
            movie.Title = movieDTO.Title;
            return NoContent();
        }

        [HttpPatch("Id:int", Name = " UpdatePartialMovie")]

        public IActionResult UpdatePartialMovie(int Id, JsonPatchDocument<MovieDTO> patchDTO)
        {
            if (patchDTO == null || Id == 0)
            {
                return BadRequest();
            }
            var movie = MovieStore.MovieList.FirstOrDefault(i => i.Id == Id);
            if (movie == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(movie, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }


    }
}
