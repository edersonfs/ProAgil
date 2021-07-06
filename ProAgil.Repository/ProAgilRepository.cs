using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
  public class ProAgilRepository : IProAgilRepository
  {
    private readonly ProAgilContext _context;

    public ProAgilRepository(ProAgilContext context)
    {
      this._context = context;
      this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    // GERAL

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Update<T>(T entity) where T : class
    {
      _context.Update(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
       _context.Remove(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
       return (await _context.SaveChangesAsync()) > 0;
    }    

    // EVENTO    

    public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
    {
      IQueryable<Evento> query = _context.Eventos
      .Include(x => x.Lotes)
      .Include(x => x.RedesSociais);

      if (includePalestrantes == true) {
          query = query.Include(x => x.PalestrantesEventos).ThenInclude(x => x.Palestrante);
      }

      query = query.OrderByDescending(x => x.DataEvento);

      return await query.ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
      .Include(x => x.Lotes)
      .Include(x => x.RedesSociais);

      if (includePalestrantes == true) {
          query = query.Include(x => x.PalestrantesEventos).ThenInclude(x => x.Palestrante);
      }

      query = query.OrderByDescending(x => x.DataEvento).Where(x => x.Tema.ToLower().Contains(tema.ToLower()));

      return await query.ToArrayAsync();
    }

    public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
      .Include(x => x.Lotes)
      .Include(x => x.RedesSociais);

      if (includePalestrantes == true) {
          query = query.Include(x => x.PalestrantesEventos).ThenInclude(x => x.Palestrante);
      }

      query = query.OrderByDescending(x => x.DataEvento).Where(x => x.Id == EventoId);

      return await query.FirstOrDefaultAsync();
    }

    // PALESTRANTE

    public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false)
    {
      IQueryable<Palestrante> query = _context.Palestrantes
      .Include(x => x.RedesSociais);

      if (includeEventos == true) {
          query = query.Include(x => x.PalestrantesEventos).ThenInclude(x => x.Evento);
      }

      query = query.OrderBy(x => x.Nome).Where(x => x.Id == PalestranteId);

      return await query.FirstOrDefaultAsync();
    }   

    public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
    {
      IQueryable<Palestrante> query = _context.Palestrantes
      .Include(x => x.RedesSociais);

      if (includeEventos == true) {
          query = query.Include(x => x.PalestrantesEventos).ThenInclude(x => x.Evento);
      }

      query = query.OrderBy(x => x.Nome).Where(x => x.Nome.ToLower().Contains(name.ToLower()));

      return await query.ToArrayAsync();
    }
  }
}