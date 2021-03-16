using Bibliotheque.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Repositories
{
    public partial interface ILibraryRepository
    {
        Task<bool> AddressExists(Guid addressID);
        Task<IEnumerable<AddressEntity>> GetAddresses();
        Task<AddressEntity> GetAddress(Guid addressId);
    }

    // TODO : Commenter le code
    public partial class LibraryRepository : ILibraryRepository
    {
        public async Task<bool> AddressExists(Guid addressId)
        {
            return await m_Context.Addresses.AnyAsync(x => x.Id == addressId);
        }

        public async Task<IEnumerable<AddressEntity>> GetAddresses()
        {
            return await m_Context.Addresses.ToListAsync();
        }

        public async Task<AddressEntity> GetAddress(Guid addressId)
        {
            return await m_Context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);
        }
    }
}
