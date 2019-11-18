using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //GERAL
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         void DeleteRange<T>(T[] entityArray) where T : class;
         Task<bool> SaveChangeAsync();

         //EVENTOS
         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false);
         Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false);


         //PALESTRANTES
         Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false);
         Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos = false);
    }
}