using IBLL.ServiceModels;
using IDAL.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IUserService : IDependency
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        LoginResult Login(string userId, string pwd);

        /// <summary>
        /// 登录  old
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        ResultModel Login(Expression<Func<UserTable, bool>> where);

        int GetCount(Expression<Func<UserTable, bool>> where);

        /// <summary>
        /// 获取数据库中最大的考号
        /// </summary>
        /// <returns></returns>
        int GetMaxKaoHao();

        /// <summary>
        /// 学生注册
        /// </summary>
        /// <param name="schoolNo"></param>
        /// <param name="userName"></param>
        /// <param name="idcard"></param>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="specialtyId"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        StudentRegisterResult StudentRegister(string schoolNo, string userName, string idcard, string phone, string pwd, string specialtyId, string qq);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        StudentRegisterResult FindPwd(string idcard, string name);

        /// <summary>
        /// 用户身份绑定
        /// </summary>
        /// <param name="kaohao"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="idcard"></param>
        /// <param name="phone"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        ResultModel UserBind(string kaohao, string userName, string pwd, string idcard, string phone, string qq);

        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="schoolNo"></param>
        /// <param name="specialtyId"></param>
        /// <param name="studentName"></param>
        /// <returns></returns>
        ResultModel GetStudentList(int? page, int? pageSize, string schoolNo, string specialtyId, string studentName, bool pagination = true);

        /// <summary>
        /// 学生列表下载
        /// </summary>
        /// <param name="schoolNo"></param>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        ResultModel DownloadStudent(string schoolNo, string specialtyId, string filepath);

    }
}
