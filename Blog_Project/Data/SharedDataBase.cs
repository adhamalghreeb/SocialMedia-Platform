using Blog_Project.Models.Domain;
using System.Collections.Concurrent;

namespace Blog_Project.Data
{
    public class SharedDataBase
    {
        public readonly ConcurrentDictionary<string, UserConnection> connections = new ConcurrentDictionary<string, UserConnection>();
        public SharedDataBase()
        {
            
        }
    }
}
