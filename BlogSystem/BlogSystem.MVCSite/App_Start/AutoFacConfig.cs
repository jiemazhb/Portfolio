using Autofac;
using Autofac.Integration.Mvc;
using BlogSystem.BLL;
using BlogSystem.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.App_Start
{
    public class AutoFacConfig
    {
        public static void Register()
        {
            //配置AutoFac的基本信息
            var builder = new ContainerBuilder();
            //进行依赖注入的的注册
            //builder.RegisterType<ArticleManager>().As<IArticleManager>();
            //builder.RegisterType<UserManager>().As<IUserManager>();
            Assembly dal = Assembly.Load("BlogSystem.DAL");
            builder.RegisterAssemblyTypes(dal).AsImplementedInterfaces().PropertiesAutowired();

            var bll = Assembly.Load("BlogSystem.BLL");
            builder.RegisterAssemblyTypes(bll).AsImplementedInterfaces().PropertiesAutowired();

            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            //构建
            var container = builder.Build();
            //实现
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}