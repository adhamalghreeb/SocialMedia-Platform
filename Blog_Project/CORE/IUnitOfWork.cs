using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.Repositories.implementation;

namespace Blog_Project.CORE
{
    public interface IUnitOfWork : IDisposable
    {
        public IBlogPost BlogPosts { get; }
        public ICategory Categories { get; }
        public IBlogImage BlogImages { get; }
        public IBaseRepository<Comment> comments { get; }
        public IBaseRepository<follow> follows { get; }


        public int Complete();
    }
}
