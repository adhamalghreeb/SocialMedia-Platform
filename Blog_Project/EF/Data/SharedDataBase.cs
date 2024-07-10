using Blog_Project.CORE.Models.Domain;
using System.Collections.Concurrent;

namespace Blog_Project.EF.Data
{
    public class SharedDataBase
    {
        public readonly ConcurrentDictionary<string, UserConnection> connections = new ConcurrentDictionary<string, UserConnection>();
        public SharedDataBase()
        {

        }
    }
}
