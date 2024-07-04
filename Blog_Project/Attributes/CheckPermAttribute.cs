using Blog_Project.Models.Domain;

namespace Blog_Project.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
    public class CheckPermAttribute : Attribute
    {
        public Permission permission;

        public CheckPermAttribute(Permission permission)
        {
            this.permission = permission;
        }
    }
}
