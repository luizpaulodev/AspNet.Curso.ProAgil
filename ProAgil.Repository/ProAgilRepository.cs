using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public ProAgilContext context { get; }
        public ProAgilRepository(ProAgilContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // METODOS GERAIS
        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<bool> SaveChangeAsync()
        {
           return await context.SaveChangesAsync() > 0;
        }
        
        // METODOS DO EVENTOS
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                .OrderByDescending(x => x.DataEvento);

            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                .OrderByDescending(x => x.DataEvento)
                .Where(w => w.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                .OrderByDescending(x => x.DataEvento)
                .Where(w => w.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        // METODOS DO PALESTRANTE
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = context.Palestrantes
                .Include(x => x.RedesSociais);

            if(includeEventos){
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking()
                .OrderBy(x => x.Nome)
                .Where(w => w.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = context.Palestrantes
                .Include(x => x.RedesSociais);

            if(includeEventos){
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking()
                .OrderBy(x => x.Nome)
                .Where(w => w.Nome.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}