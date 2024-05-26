using BlogSystem.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserManager
    {
        Task<List<Followers>> GetMostFolloerUser();
        Task RegisterAsync(string email, string password);
        Task RegisterAsync(string email, string password, string IconPath, string nickName);
        bool Login(string email, string password, out Guid userId, out string nickName, out string iconPath);
        Task ChangePassWord(string email, string oldPassWord, string newPassWord);
        Task ChangeUserInformation(string email, string siteName, string imgPath);
        UserInformationDto GetUserByEmail(string email);

    }
}
