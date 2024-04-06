using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStoreRepositories;
using System.Net;

namespace MusicStore.Api.Controllers
{

    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository repository;
        private readonly ILogger<GenresController> logger;

        public GenresController(IGenreRepository repository, ILogger<GenresController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        [HttpGet]
        public async  Task<IActionResult> Get() { 

            var response = new BaseResponseGeneric<ICollection<GenreResponseDto>>();
            var genresDb = await repository.GetAsync();
            try
            {
                //Mapping
                var genres = genresDb.Select(x => new GenreResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status,

                }).ToList();
                response.Data = genres;
                response.Success= true;
                logger.LogInformation("Obteniendo todos los generos musicales");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al obtener la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }
        [HttpGet("{id:int}")]
        public async  Task<IActionResult> Get (int id)
        {
            var response = new BaseResponseGeneric<GenreResponseDto>();
            var genreDb = await repository.GetAsync(id);
            try {
                if(genreDb is null)
                {
                    logger.LogWarning($"el genero musical con id {id} no se encontro");
                    return BadRequest(response);
                }
                else
                {
                    //mapeo
                    var genre = new GenreResponseDto
                    {
                       Id=genreDb.Id,
                       Name=genreDb.Name,
                       Status=genreDb.Status,
                    };
                    response.Data = genre;
                    response.Success = true;
                    logger.LogInformation("Obteniendo el genero musical");
                    return Ok(response.Data);
                }

                
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al obtener la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(GenreRequestDto genreRequestDto)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                //mapping
                var genreDb = new  Genre(){
                    Name = genreRequestDto.Name,
                    Status= genreRequestDto.Status,
                };

                var genreId= await repository.AddAsync(genreDb);
                response.Data = genreId;
                response.Success = true;
                logger.LogInformation("Registrando un nuevo genero musical");
                return StatusCode((int)HttpStatusCode.Created, response);  
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al registrar la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put (int id,GenreRequestDto genreRequestDto) {

            var response = new BaseResponse();

            try
            {
                var genreDb = await repository.GetAsync(id);
                if(genreDb is null)
                {
                    response.ErrorMessage = "No se encontro Registro";
                    return NotFound(response);
                }
                genreDb.Name=genreRequestDto.Name;
                genreDb.Status=genreRequestDto.Status;

                await repository.UpdateAsync();
                response.Success = true;
                logger.LogInformation("Genero musical actualizado");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al actualizar la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse();

            try
            {
                var genreDb = await repository.GetAsync(id);

                if(genreDb is null)
                {
                    response.ErrorMessage = "No se encontro Registro";
                    logger.LogWarning(response.ErrorMessage);
                    return NotFound(response);

                }
                await repository.DeleteAsync(id);
                response.Success = true;
                logger.LogInformation($"Genero musical eliminado");
                return Ok(response);
            }   
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al eliminar la informacion";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }

        }
    }
}
