using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class EventoRepository : IEventoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string EventoCacheKey = "AllEventosCache";
        private readonly int CacheExpirationTime = 3600;
        public EventoRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        public void ClearCache()
        {
            _cache.Remove(EventoCacheKey);
        }

        public async Task<ICollection<EventoEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(EventoCacheKey, out ICollection<EventoEntity> EventoCached))
                return EventoCached;

            var EventosFromDb = await _context.Eventos.OrderBy(c => c.Nombre).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(EventoCacheKey, EventosFromDb, cacheEntryOptions);
            return EventosFromDb;
        }

        public async Task<EventoEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(EventoCacheKey, out ICollection<EventoEntity> EventoCached))
            {
                var Evento = EventoCached.FirstOrDefault(c => c.Id == id);
                if (Evento != null)
                    return Evento;
            }

            return await _context.Eventos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Eventos.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(EventoEntity Evento)
        {
            _context.Eventos.Add(Evento);
            return await Save();
        }

        public async Task<bool> UpdateAsync(EventoEntity Evento)
        {

            _context.Update(Evento);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Evento = await GetAsync(id);
            if (Evento == null)
                return false;

            _context.Eventos.Remove(Evento);
            return await Save();
        }
    }
}
