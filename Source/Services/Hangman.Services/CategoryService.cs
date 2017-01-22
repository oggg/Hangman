namespace Hangman.Services
{
    using System.Linq;
    using Contracts;
    using Data.Repositories;
    using Models;

    public class CategoryService : ICategoryService
    {
        IRepository<Category> categories;

        public CategoryService(IRepository<Category> categories)
        {
            this.categories = categories;
        }

        public IQueryable<Category> GetAll()
        {
            return this.categories.All();
        }

        public Category GetById(int categoryId)
        {
            Category category = this.categories.All().FirstOrDefault(w => w.Id == categoryId);
            return category;
        }
    }
}
