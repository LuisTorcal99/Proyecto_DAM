using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using System.Security.Cryptography.Xml;

namespace RestAPI.Repository
{
    public class AsignaturaRepository : IAsignaturaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string AsignaturaCacheKey = "AllAsignaturaCache";
        private readonly int CacheExpirationTime = 3600;
        public AsignaturaRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(AsignaturaCacheKey);
        }

        public async Task<ICollection<AsignaturaEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(AsignaturaCacheKey, out ICollection<AsignaturaEntity> asignaturaCached))
                return asignaturaCached;

            var asignaturasFromDb = await _context.Asignaturas.OrderBy(c => c.Nombre).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(AsignaturaCacheKey, asignaturasFromDb, cacheEntryOptions);
            return asignaturasFromDb;
        }

        public async Task<AsignaturaEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(AsignaturaCacheKey, out ICollection<AsignaturaEntity> asignaturaCached))
            {
                var asignatura = asignaturaCached.FirstOrDefault(c => c.Id == id);
                if (asignatura != null)
                    return asignatura;
            }

            return await _context.Asignaturas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Asignaturas.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(AsignaturaEntity asignatura)
        {
            _context.Asignaturas.Add(asignatura);
            return await Save();
        }

        public async Task<bool> UpdateAsync(AsignaturaEntity asignatura)
        {

            _context.Update(asignatura);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var asignatura = await GetAsync(id);
            if (asignatura == null)
                return false;

            _context.Asignaturas.Remove(asignatura);
            return await Save();
        }
    }
}
