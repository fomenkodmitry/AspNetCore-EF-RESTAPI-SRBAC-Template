using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SqlRepository
    {
        private readonly Context _context;

        public SqlRepository(Context context)
        {
            _context = context;
        }

        public void Execute(string str) => _context.Database.ExecuteSqlRaw(str);
    }
}