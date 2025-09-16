using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CounterStrikeItemsApi.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task AddAsync(RefreshToken refreshToken)
            => await _context.RefreshTokens.AddAsync(refreshToken);

        public void Update(RefreshToken refreshToken) 
            => _context.RefreshTokens.Update(refreshToken);

        public async Task RevokeTokensForUserAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked).ToListAsync();
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() 
            => await _context.SaveChangesAsync();
    }
}
