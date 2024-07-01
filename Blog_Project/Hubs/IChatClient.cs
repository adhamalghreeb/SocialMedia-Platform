namespace Blog_Project.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user,string admin = null, string message = null);
    }
}
