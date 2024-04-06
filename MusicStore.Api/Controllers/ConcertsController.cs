using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStoreRepositories;

namespace MusicStore.Api.Controllers
{
    [ApiController]
    [Route("api/concerts")]
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertRepository repository;
        private readonly ILogger<ConcertsController> logger;
        private readonly IGenreRepository genreRepository;
        public ConcertsController(IConcertRepository repository,IGenreRepository genreRepository, ILogger<ConcertsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.genreRepository = genreRepository; 

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var concertsDb = await repository.GetAsync();
            return Ok(concertsDb);
        }
        [HttpGet("title")]
        public async Task<IActionResult> Get(string? title)
        {
            //var concerts = await repository.GetAsync(x => x.Title.Contains(title ?? string.Empty), X => X.DateEvent);
            var concerts = await repository.GetAsync(title);
            return Ok(concerts);
        }
        [HttpPost]
        public async Task<IActionResult> Post(ConcertRequestDto concertRequestDto)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                //validate genre id
                var genre = await genreRepository.GetAsync(concertRequestDto.GenreId);
                if(genre is null)
                {
                    response.ErrorMessage = $"El id de genero musical {concertRequestDto.GenreId} es incorrecto.";
                    logger.LogWarning(response.ErrorMessage);
                    return BadRequest(response);
                }
                //maping
                var concertDb = new Concert
                {
                    Title= concertRequestDto.Title,
                    Description= concertRequestDto.Description,
                    Place = concertRequestDto.Place,
                    UnitPrice = concertRequestDto.UnitPrice,
                    GenreId =concertRequestDto.GenreId,
                    DateEvent=concertRequestDto.DateEvent,
                    ImageUrl=concertRequestDto.ImageUrl,
                    TicketsQuantity=concertRequestDto.TicketsQuantity,

                };
                response.Data = await repository.AddAsync(concertDb);
                response.Success= true;

            }catch (Exception ex)
            {
                response.ErrorMessage= "Ocurrio un error al guardar la informacion";
                logger.LogError(ex, ex.Message);
            }
            return Ok(response);
        }
        
    }
}
