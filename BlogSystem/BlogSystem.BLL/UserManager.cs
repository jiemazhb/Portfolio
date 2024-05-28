using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogSystem.BLL
{
    public class UserManager : IUserManager
    {
        private readonly IUserService _userService;
        public UserManager(IUserService userService)
        {
            _userService = userService;
        }
        public async Task StatisticsUserDataAsync(WebSiteStatistic data)
        {
            data.registers = await _userService.GetAll().CountAsync();
        }
        public async Task<UserInformationDto> GetUserById(Guid userId)
        {
            return await _userService.GetOneByIdAsync(userId).Select(u => new UserInformationDto { 
                Id = u.Id,
                Email = u.Email,
                ImgPath = u.ImagePath,
                FansCount = u.FansCount,
                FocusCount = u.FocusCount,
                UserName = u.NickName
            }).FirstAsync();
        }
        public async Task<List<Followers>> GetMostFolloerUser()
        {
            return await _userService.GetAll().OrderByDescending(u => u.FansCount).Take(6).Select(u => new Followers()
            {
                UserName = u.NickName,
                AvatarPath = u.ImagePath,
                AmountOfFuns = u.FansCount
            }).ToListAsync();
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
                return null;
            }
        }

        public bool Login(string email, string password, out Guid userId ,out string nickName, out string iconPath)
        {
            var result = _userService.GetAll().FirstOrDefault(u => u.Email == email && u.PassWord == password);
            if (result == null)
            {
                userId = new Guid();
                nickName = null;
                iconPath = null;
                return false;
            }
            else
            {
                userId = result.Id;
                nickName = result.NickName;
                iconPath = result.ImagePath;
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
                SiteName = "default",
                ImagePath = IconPath,
                NickName = nickName
            });
        }
    }
}
