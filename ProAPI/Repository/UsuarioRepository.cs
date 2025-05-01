using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string UserCacheKey = "UsersCacheKey";
        private readonly int CacheExpirationTime = 3600;
        public UsuarioRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(UserCacheKey);
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Users.OrderBy(c => c.Name).ToListAsync(); // sin caché
        }

        public async Task<User> GetAsync(int id)
        {
            if (_cache.TryGetValue(UserCacheKey, out ICollection<User> UserCached))
            {
                var User = UserCached.FirstOrDefault(c => c.Id.Equals(id));
                if (User != null)
                    return User;
            }

            return await _context.Users.FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(c => c.Id.Equals(id));
        }

        public async Task<bool> CreateAsync(User User)
        {
            _context.Users.Add(User);
            return await Save();
        }

        public async Task<bool> UpdateAsync(User User)
        {

            _context.Update(User);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var User = await GetAsync(id);
            if (User == null)
                return false;

            _context.Users.Remove(User);
            return await Save();
        }
    }
}
