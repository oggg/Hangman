using Hangman.Models;
using Hangman.Web.Infrastructure.Mapping;

namespace Hangman.Web.Models
{
    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}