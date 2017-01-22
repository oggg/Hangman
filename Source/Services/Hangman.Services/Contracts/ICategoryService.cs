using System.Linq;
using Hangman.Models;

namespace Hangman.Services.Contracts
{
    public interface ICategoryService
    {
        Category GetById(int categoryId);

        IQueryable<Category> GetAll();
    }
}
