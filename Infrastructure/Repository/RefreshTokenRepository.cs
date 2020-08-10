using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>
    {
        public RefreshTokenRepository(DbContext context) : base(context)
        {
  
        }

        public async Task<RefreshToken> GetTheTokenByTokenProp(string token)
        {
           return await _context.Set<RefreshToken>().Where(r => r.Token == token).FirstOrDefaultAsync();
           
        }

    }
}
