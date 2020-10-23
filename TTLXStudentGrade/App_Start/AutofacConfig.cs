using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IBLL;
using IBLL.Impl;
using IDAL;
using IDAL.Impl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TTLXStudentGrade
{
    public class AutofacConfig
    {
        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register(Type type)
        {
            var builder = new ContainerBuilder();
            //注册MVC控制器（注册所有到控制器，控制器注入，就是需要在控制器的构造函数中接收对象）
            builder.RegisterControllers(type.Assembly);
            builder.RegisterApiControllers(type.Assembly);
            builder.RegisterType<DbServerEntity>().As<IDbServerEntity>();
            builder.RegisterType<DbUserEntity>().As<IDbUserEntity>();

            Type basetype = typeof(IDependency);
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(basetype))
                .Where(t => basetype.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();
            //设置依赖解析器
            var container = builder.Build();

            builder.RegisterApiControllers(type.Assembly);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}