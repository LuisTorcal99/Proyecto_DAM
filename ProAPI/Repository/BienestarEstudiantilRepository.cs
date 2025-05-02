using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class BienestarEstudiantilRepository : IBienestarEstudiantilRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string BienestarEstudiantilCacheKey = "AllBienestarEstudiantilCache";
        private readonly int CacheExpirationTime = 3600;
        public BienestarEstudiantilRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(BienestarEstudiantilCacheKey);
        }

        public async Task<ICollection<BienestarEstudiantilEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(BienestarEstudiantilCacheKey, out ICollection<BienestarEstudiantilEntity> BienestarEstudiantilCached))
                return BienestarEstudiantilCached;

            var BienestarEstudiantilsFromDb = await _context.BienestarEstudiantil.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(BienestarEstudiantilCacheKey, BienestarEstudiantilsFromDb, cacheEntryOptions);
            return BienestarEstudiantilsFromDb;
        }

        public async Task<BienestarEstudiantilEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(BienestarEstudiantilCacheKey, out ICollection<BienestarEstudiantilEntity> BienestarEstudiantilCached))
            {
                var BienestarEstudiantil = BienestarEstudiantilCached.FirstOrDefault(c => c.Id == id);
                if (BienestarEstudiantil != null)
                    return BienestarEstudiantil;
            }

            return await _context.BienestarEstudiantil.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BienestarEstudiantil.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(BienestarEstudiantilEntity BienestarEstudiantil)
        {
            _context.BienestarEstudiantil.Add(BienestarEstudiantil);
            return await Save();
        }

        public async Task<bool> UpdateAsync(BienestarEstudiantilEntity BienestarEstudiantil)
        {

            _context.Update(BienestarEstudiantil);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var BienestarEstudiantil = await GetAsync(id);
            if (BienestarEstudiantil == null)
                return false;

            _context.BienestarEstudiantil.Remove(BienestarEstudiantil);
            return await Save();
        }
    }
}
