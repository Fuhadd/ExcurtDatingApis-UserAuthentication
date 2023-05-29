using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ApiUser> ApiUsers { get; }

        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<UserImage> UserImages { get; }
        IRepository<UserMatch> UserMatches { get; }
        IRepository<Message> Messages { get; }
        Task Save();
    }
}
