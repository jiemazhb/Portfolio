using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserManager : IUserManager
    {
        private readonly IUserService _userService;
        public UserManager(IUserService userService)
        {
            _userService = userService;
        }
        public async Task ChangePassWord(string email, string oldPassWord, string newPassWord)
        {
            var result = _userService.GetAll();
            if (result.Any(u => u.Email == email && u.PassWord == oldPassWord))
            {
                var user = result.First(u => u.Email == email && u.PassWord == oldPassWord);
                user.PassWord = newPassWord;
                await _userService.EditAsync(user);
            }
        }

        public async Task ChangeUserInformation(string email, string siteName, string imgPath)
        {
            var user = _userService.GetAll().First(u => u.Email == email);
            user.SiteName = siteName;
            user.ImagePath = imgPath;
            await _userService.EditAsync(user);
        }

        public UserInformationDto GetUserByEmail(string email)
        {
            if (_userService.GetAll().Any(u => u.Email == email))
            {
                var result = _userService.GetAll().Where(u => u.Email == email).Select(user =>
                new UserInformationDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    ImgPath = user.ImagePath,
                    SiteName = user.SiteName,
                    FansCount = user.FansCount,
                    FocusCount = user.FocusCount
                }).First();
                return result;
            }
            else
            {
                throw new ArgumentException("邮箱地址不纯在");
            }
        }

        public bool Login(string email, string password, out Guid userId ,out string nickName)
        {
            var result = _userService.GetAll().FirstOrDefault(u => u.Email == email && u.PassWord == password);
            if (result == null)
            {
                userId = new Guid();
                nickName = null;
                return false;
            }
            else
            {
                userId = result.Id;
                nickName = result.NickName;
                return true;
            }
        }
        public async Task RegisterAsync(string email, string password)
        {
            await _userService.CreateAsync(new User()
            {
                Email = email,
                PassWord = password,
                SiteName = "默认小站",
                ImagePath = "default.png"
            });
        }
        public async Task RegisterAsync(string email, string password , string IconPath, string nickName)
        {
            await _userService.CreateAsync(new User()
            {
                Email = email,
                PassWord = password,
                SiteName = "默认小站",
                ImagePath = IconPath,
                NickName = nickName
            });
        }
    }
}
