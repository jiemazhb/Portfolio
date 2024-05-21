using BlogSystem.Dto;
using System;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserManager
    {
        Task RegisterAsync(string email, string password);
        Task RegisterAsync(string email, string password, string IconPath, string nickName);
        bool Login(string email, string password, out Guid userId, out string nickName);
        Task ChangePassWord(string email, string oldPassWord, string newPassWord);
        Task ChangeUserInformation(string email, string siteName, string imgPath);
        UserInformationDto GetUserByEmail(string email);

    }
}
