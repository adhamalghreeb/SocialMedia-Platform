using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.implementation;
using Blog_Project.Repositories.Interface;

namespace Blog_Project.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly appDBcontext appDBcontext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        public IBlogPost BlogPosts { get; private set; }
        public ICategory Categories { get; private set; }
        public IBlogImage BlogImages { get; private set; }

        public IBaseRepository<Comment> comments { get; private set; }
        public IBaseRepository<follow> follows { get; private set; }

        public UnitOfWork(appDBcontext appDBcontext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.appDBcontext = appDBcontext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            BlogPosts = new BlogPostRepo(appDBcontext);
            Categories = new CategoryRepo(appDBcontext);
            BlogImages = new BlogImageRepo(appDBcontext, webHostEnvironment, httpContextAccessor);
            comments = new BaseRepository<Comment>(appDBcontext);
            follows = new BaseRepository<follow>(appDBcontext);
        }
        public int Complete()
        {
            return appDBcontext.SaveChanges();
        }

        public void Dispose()
        {
            appDBcontext.Dispose();
        }
    }
}
