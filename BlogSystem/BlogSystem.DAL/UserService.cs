using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    /// <summary>
    /// 继承IUserService是为了以后实现依赖注入
    /// </summary>
    public class UserService:BaseService<User>, IUserService
    {
        public UserService():base(new BlogContext())
        {

        }
    }
}
