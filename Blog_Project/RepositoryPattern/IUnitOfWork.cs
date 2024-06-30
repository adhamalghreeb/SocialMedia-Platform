using Blog_Project.Models.Domain;
using Blog_Project.Repositories.implementation;
using Blog_Project.Repositories.Interface;

namespace Blog_Project.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public IBlogPost BlogPosts { get; }
        public ICategory Categories { get; }
        public IBlogImage BlogImages { get; }
        public IBaseRepository<Comment> comments { get; }


        public int Complete();
    }
}
