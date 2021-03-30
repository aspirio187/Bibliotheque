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
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<IEnumerable<CategoryEntity>> GetCategoriesAsync();
        Task<CategoryEntity> GetCategoryAsync(int categoryId);
        void AddCategory(CategoryEntity category);
        void DeleteCategory(CategoryEntity category);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await m_Context.Categories.AnyAsync(x => x.Id == categoryId);
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync()
        {
            return await m_Context.Categories.ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryAsync(int categoryId)
        {
            return await m_Context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
        }

        public void AddCategory(CategoryEntity category)
        {
            if (category is null) throw new ArgumentNullException(nameof(category));
            m_Context.Categories.Add(category);
        }

        public void DeleteCategory(CategoryEntity category)
        {
            if (category is null) throw new ArgumentNullException(nameof(category));
            m_Context.Categories.Remove(category);
        }
    }
}
