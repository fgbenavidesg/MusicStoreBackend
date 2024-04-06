using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Entities.Info;
using MusicStore.Persistence;


namespace MusicStoreRepositories
{
#pragma warning disable CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
    public class ConcertRepository : RepositoryBase<Concert>, IConcertRepository
#pragma warning restore CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
    {
        public ConcertRepository(ApplicationDbContext context) : base(context)
        {

        }
        public  override async Task<ICollection<Concert>> GetAsync()
        {
            //eager loading
            return await context.Set<Concert>()
                            .Include(x=>x.Genre)
                            .AsNoTracking()
                            .ToListAsync();

        }
        public async Task<ICollection<ConcertInfo>> GetAsync(string? title) 
        {
            //lazy loading
            //return await context.Set<Concert>()
            //.Where(x => x.Title.Contains(title ?? string.Empty))
            //.AsNoTracking()
            //.Select(x => new ConcertInfo
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description,
            //    Place = x.Place,
            //    UnitPrice = x.UnitPrice,
            //    Genre = x.Genre.Name,
            //    GenreId = x.GenreId,
            //    DateEvent = x.DateEvent.ToShortDateString(),
            //    TimeEvent = x.DateEvent.ToShortTimeString(),
            //    ImageUrl = x.ImageUrl,
            //    TicketsQuantity = x.TicketsQuantity,
            //    Finalized = x.Finalized,
            //    Status = x.Status ? "Activo" : "Inactivo"
            //})
            //.ToListAsync();
            var query = context.Set<ConcertInfo>()
                .FromSqlRaw("usp_Report {0}",title ?? string.Empty);
            return await query.ToListAsync();

        }
    }
}
