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
        Task<AddressEntity> GetUserAddress(Guid userId);
        void AddAddress(AddressEntity address);
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

        public async Task<AddressEntity> GetUserAddress(Guid userId)
        {
            var user = await m_Context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return await GetAddress(user.AddressId);
        }
        public void AddAddress(AddressEntity address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            m_Context.Entry(address).State = EntityState.Added;
        }
    }
}
