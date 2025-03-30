using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class NotasRepository : INotaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string NotaCacheKey = "AllNotaCache";
        private readonly int CacheExpirationTime = 3600;
        public NotasRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(NotaCacheKey);
        }

        public async Task<ICollection<NotaEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(NotaCacheKey, out ICollection<NotaEntity> NotaCached))
                return NotaCached;

            var NotasFromDb = await _context.Notas.OrderBy(c => c.NotaValor).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(NotaCacheKey, NotasFromDb, cacheEntryOptions);
            return NotasFromDb;
        }

        public async Task<NotaEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(NotaCacheKey, out ICollection<NotaEntity> NotaCached))
            {
                var Nota = NotaCached.FirstOrDefault(c => c.Id == id);
                if (Nota != null)
                    return Nota;
            }

            return await _context.Notas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Notas.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(NotaEntity Nota)
        {
            _context.Notas.Add(Nota);
            return await Save();
        }

        public async Task<bool> UpdateAsync(NotaEntity Nota)
        {

            _context.Update(Nota);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Nota = await GetAsync(id);
            if (Nota == null)
                return false;

            _context.Notas.Remove(Nota);
            return await Save();
        }
    }
}
