using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly DatabaseContext _context;
        private IRepository<RefreshToken> _refreshToken;
        private IRepository<ApiUser> _apiUser;
        private IRepository<UserImage> _userImage;
        private IRepository<UserMatch> _usermatch;
        private IRepository<Message> _message;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;

        }

        public IRepository<ApiUser> ApiUsers => _apiUser ??= new Repository<ApiUser>(_context);

        public IRepository<RefreshToken> RefreshTokens => _refreshToken ??= new Repository<RefreshToken>(_context);

        public IRepository<UserImage> UserImages => _userImage ??= new Repository<UserImage>(_context);

        public IRepository<UserMatch> UserMatches => _usermatch ??= new Repository<UserMatch>(_context);

        public IRepository<Message> Messages => _message ??= new Repository<Message>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
