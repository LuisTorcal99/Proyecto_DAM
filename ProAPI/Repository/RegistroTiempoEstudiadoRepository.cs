using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class RegistroTiempoEstudiadoRepository : IRegistroTiempoEstudiadoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string RegistroTiempoEstudiadoCacheKey = "AllRegistroTiempoEstudiadoCache";
        private readonly int CacheExpirationTime = 3600;
        public RegistroTiempoEstudiadoRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(RegistroTiempoEstudiadoCacheKey);
        }

        public async Task<ICollection<RegistroTiempoEstudiadoEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(RegistroTiempoEstudiadoCacheKey, out ICollection<RegistroTiempoEstudiadoEntity> RegistroTiempoEstudiadoCached))
                return RegistroTiempoEstudiadoCached;

            var RegistroTiempoEstudiadosFromDb = await _context.RegistroTiempoEstudiado.OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(RegistroTiempoEstudiadoCacheKey, RegistroTiempoEstudiadosFromDb, cacheEntryOptions);
            return RegistroTiempoEstudiadosFromDb;
        }

        public async Task<RegistroTiempoEstudiadoEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(RegistroTiempoEstudiadoCacheKey, out ICollection<RegistroTiempoEstudiadoEntity> RegistroTiempoEstudiadoCached))
            {
                var RegistroTiempoEstudiado = RegistroTiempoEstudiadoCached.FirstOrDefault(c => c.Id == id);
                if (RegistroTiempoEstudiado != null)
                    return RegistroTiempoEstudiado;
            }

            return await _context.RegistroTiempoEstudiado.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RegistroTiempoEstudiado.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(RegistroTiempoEstudiadoEntity RegistroTiempoEstudiado)
        {
            _context.RegistroTiempoEstudiado.Add(RegistroTiempoEstudiado);
            return await Save();
        }

        public async Task<bool> UpdateAsync(RegistroTiempoEstudiadoEntity RegistroTiempoEstudiado)
        {

            _context.Update(RegistroTiempoEstudiado);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var RegistroTiempoEstudiado = await GetAsync(id);
            if (RegistroTiempoEstudiado == null)
                return false;

            _context.RegistroTiempoEstudiado.Remove(RegistroTiempoEstudiado);
            return await Save();
        }
    }
}
