using Blog_Project.CORE;
using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.EF.Data;
using Blog_Project.Repositories.implementation;


namespace Blog_Project.EF.RepositoryPattern
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
