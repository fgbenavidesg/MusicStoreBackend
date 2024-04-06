using Microsoft.EntityFrameworkCore;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Persistence;

namespace MusicStoreRepositories
{
#pragma warning disable CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
#pragma warning restore CS8613 // La nulabilidad de los tipos de referencia en el tipo de valor devuelto no coincide con el miembro implementado de forma implícita
    {

        //readonly paara que no se pueda usar en otro lado, solo en el constructor
        public GenreRepository(ApplicationDbContext context) :base(context) { 
        
        }

    }
}
