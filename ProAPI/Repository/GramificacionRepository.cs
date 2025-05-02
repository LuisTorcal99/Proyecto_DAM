using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class GamificacionRepository : IGamificacionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string GamificacionCacheKey = "AllGamificacionCache";
        private readonly int CacheExpirationTime = 3600;
        public GamificacionRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(GamificacionCacheKey);
        }

        public async Task<ICollection<GamificacionEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(GamificacionCacheKey, out ICollection<GamificacionEntity> GamificacionCached))
                return GamificacionCached;

            var GamificacionsFromDb = await _context.Gamificacion.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(GamificacionCacheKey, GamificacionsFromDb, cacheEntryOptions);
            return GamificacionsFromDb;
        }

        public async Task<GamificacionEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(GamificacionCacheKey, out ICollection<GamificacionEntity> GamificacionCached))
            {
                var Gamificacion = GamificacionCached.FirstOrDefault(c => c.Id == id);
                if (Gamificacion != null)
                    return Gamificacion;
            }

            return await _context.Gamificacion.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Gamificacion.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(GamificacionEntity Gamificacion)
        {
            _context.Gamificacion.Add(Gamificacion);
            return await Save();
        }

        public async Task<bool> UpdateAsync(GamificacionEntity Gamificacion)
        {

            _context.Update(Gamificacion);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Gamificacion = await GetAsync(id);
            if (Gamificacion == null)
                return false;

            _context.Gamificacion.Remove(Gamificacion);
            return await Save();
        }
    }
}
