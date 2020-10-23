using Application;
using Application.Common;
using Application.Enum;
using Application.Logger;
using IBLL.Helper;
using IBLL.ServiceModels;
using IDAL;
using IDAL.DbModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace IBLL.Impl
{
    public class UserService : IUserService
    {
        public IDbUserEntity _db { get; set; }

        public UserService() { }

        public UserService(IDbUserEntity db)
        {
            this._db = db;
        }


        private static string _KaohaoKey = "MaxKaohao";

        private static object objlock = new object();

        public LoginResult Login(string userId, string pwd)
        {

            var student = RedisHelper.Instance.GetModel<StudentDataImport>(userId.ToUpper(), RedisIndex.STUDENT_MANAGER_SYSTEM);
            UserTable user;
            if (student != null)
            {
                if (student.Pwd == pwd)
                {
                    user = new UserTable
                    {
                        IsAdmin = null,
                        UserType = 1,
                        KaoHao = student.Kaohao,
                        UserName = student.UserName,
                        lexueid = student.Lexueid,
                        FK_School = student.SchoolName,
                        FK_SchoolID = student.SchoolNo,
                        FK_Specialty = Convert.ToInt32(student.SpecailtyId),
                        FK_SpecialtyName = student.SpecialtyName,
                        IDcard = userId
                    };
                }
                else
                {
                    return new LoginResult { code = 0, message = "用户名或密码错误" };
                }
            }
            else
            {
                var idcard = _db.Set<LexueidRelationIDCard>().FirstOrDefault(s => s.idcard == userId);

                if (idcard == null)
                {
                    user = _db.Set<UserTable>().FirstOrDefault(s => s.KaoHao == userId && s.UserPassword == pwd);
                }
                else
                {
                    user = _db.Set<UserTable>().FirstOrDefault(s => s.lexueid == idcard.lexueid && s.UserPassword == pwd);
                }

                if (user == null)
                {
                    return new LoginResult { code = 0, message = "用户名或密码错误" };
                }

                if (user.UserType == 1 && user.KaoHao == userId)
                {
                    return new LoginResult { code = 0, message = "请使用身份证号登录" };
                }

                if (idcard != null)
                {
                    user.IDcard = idcard.idcard;
                }
            }



            if (user.IsAdmin == 1 || user.UserType == 3 || user.UserType == 1)
            {
                UserData data = new UserData
                {
                    Kaohao = user.KaoHao,
                    Lexueid = user.lexueid,
                    UserName = user.UserName,
                };
                if (user.IsAdmin == 1)
                {
                    data.IsAdmin = true;
                    data.UserRole = UserRole.Admin;
                }
                else
                {
                    if (user.UserType == 1)
                    {
                        data.IDCard = user.IDcard;
                    }
                    data.UserRole = (UserRole)user.UserType;
                    data.SchoolCode = user.FK_SchoolID;
                    data.SchoolName = user.FK_School;
                    if (user.UserType == 1)
                    {
                        data.SpecialtyId = user.FK_Specialty == null ? "" : user.FK_Specialty.ToString();
                        data.SpecialtyName = user.FK_SpecialtyName;
                    }
                }
                return new LoginResult { code = 1, data = data };
            }
            return new LoginResult { code = 0, message = "当前用户没有权限" };
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultModel Login(Expression<Func<UserTable, bool>> where)
        {
            UserTable userQuery = _db.Set<UserTable>().Where(u => u.IsDelete == 0).FirstOrDefault(where);
            if (userQuery == null)
            {
                return new ResultModel { code = 0, message = "用户名或密码错误" };
            }
            if (userQuery.IsAdmin == 1 || userQuery.UserType == 3 || userQuery.UserType == 1)
            {
                UserData data = new UserData
                {
                    Kaohao = userQuery.KaoHao,
                    Lexueid = userQuery.lexueid,
                    UserName = userQuery.UserName,
                };
                if (userQuery.IsAdmin == 1)
                {
                    data.IsAdmin = true;
                    data.UserRole = UserRole.Admin;
                }
                else
                {
                    data.UserRole = (UserRole)userQuery.UserType;
                    data.SchoolCode = userQuery.FK_SchoolID;
                    data.SchoolName = userQuery.FK_School;
                    if (userQuery.UserType == 1)
                    {
                        data.SpecialtyId = userQuery.FK_Specialty == null ? "" : userQuery.FK_Specialty.ToString();
                        data.SpecialtyName = userQuery.FK_SpecialtyName;
                    }
                }


                return new ResultModel { code = 1, message = "登录成功", data = data };
            }
            return new ResultModel { code = 0, message = "当前用户没有权限" };
        }

        public int GetCount(Expression<Func<UserTable, bool>> where)
        {
            return _db.GetCount(where);
        }

        public StudentRegisterResult StudentRegister(string schoolNo, string userName, string idcard, string phone, string pwd, string specialtyId, string qq)
        {
            if (!DataVerify.Instance.CheckIDCard(idcard))
            {
                return new StudentRegisterResult { code = 0, message = "请输入正确的身份证号码" };
            }
            //if (!DataVerify.Instance.CheckPhoneNumber(phone))
            //{
            //    return new StudentRegisterResult { code = 0, message = "请输入正确的手机号码" };
            //}

            idcard = idcard.ToUpper();
            StudentRegisterResult model = new StudentRegisterResult();
            var stduent = RedisHelper.Instance.GetModel<StudentDataImport>(idcard, RedisIndex.STUDENT_MANAGER_SYSTEM);
            if (stduent == null)
            {
                var result = _db.QuerySingle<LexueidRelationIDCard>(c => c.idcard == idcard);
                if (result == null)
                {
                    string maxKaohao = "";
                    lock (objlock)
                    {
                        if (RedisHelper.Instance.IsSet(_KaohaoKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
                        {
                            maxKaohao = RedisHelper.Instance.StringGet(_KaohaoKey, RedisIndex.STUDENT_MANAGER_SYSTEM);
                            maxKaohao = (Convert.ToInt32(maxKaohao) + 1).ToString();
                        }
                        else
                        {
                            int iMaxKaohao = GetMaxKaoHao();
                            maxKaohao = (iMaxKaohao + 1).ToString();
                        }
                        RedisHelper.Instance.StringSet(_KaohaoKey, maxKaohao, RedisIndex.STUDENT_MANAGER_SYSTEM);
                    }

                    stduent = new StudentDataImport
                    {
                        SchoolNo = schoolNo,
                        SchoolName = GlabolData.Instance._dicSchool[schoolNo],
                        SpecailtyId = specialtyId,
                        SpecialtyName = GlabolData.Instance._dicSpecialty[specialtyId],
                        IDCard = idcard,
                        Kaohao = "ks" + maxKaohao,
                        UserName = userName,
                        Pwd = pwd,
                        Lexueid = "17" + schoolNo + string.Format("{0:000}", maxKaohao),
                        PhoneNumber = phone,
                        QQ = qq,
                        IsHadUpload = false,
                        RegisterDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    if (!GlabolData.Instance.Datas.ContainsKey(QueueDataType.Register))
                    {
                        GlabolData.Instance.Datas.Add(QueueDataType.Register, new RegisterQueueData());
                    }
                    GlabolData.Instance.Datas[QueueDataType.Register].Enqueue(stduent);
                    model.code = 1;
                    model.message = "报名成功！";
                    model.student = stduent;
                    RedisHelper.Instance.SetModel(idcard, stduent, RedisIndex.STUDENT_MANAGER_SYSTEM);
                }
                else
                {
                    var usertable = _db.QuerySingle<UserTable>(u => u.lexueid == result.lexueid);

                    if (usertable == null)
                    {
                        model.code = 0;
                        model.message = "信息错误，请联系管理员！";
                    }
                    else
                    {
                        if (usertable.UserName == userName)
                        {
                            model.code = 0;
                            model.message = "您已经注册过了，请勿重复注册！";
                        }
                        else
                        {
                            model.code = 0;
                            model.message = "此身份证已有账号，请使用您本人的身份证报名！若您忘记密码，请在首页使用身份证和姓名找回密码";
                        }
                    }

                }
            }
            else
            {
                if (stduent.UserName == userName)
                {
                    model.code = 0;
                    model.message = "您已经注册过了，请勿重复注册！";
                }
                else
                {
                    model.code = 0;
                    model.message = "此身份证已有账号，请使用您本人的身份证报名！若您忘记密码，请在首页使用身份证和姓名找回密码";
                }
            }

            return model;
        }

        public int GetMaxKaoHao()
        {
            return _db.QueryBySql<int?>("select Max(Cast(Right(KaoHao,LEN(KaoHao)-2) as int)) from UserTable  where UserType=1 and KaoHao like '[kK][sS]%'").FirstOrDefault() ?? 0;
        }

        public StudentRegisterResult FindPwd(string idcard, string name)
        {
            idcard = idcard.ToUpper();
            StudentRegisterResult model = new StudentRegisterResult();
            var stduent = RedisHelper.Instance.GetModel<StudentDataImport>(idcard, RedisIndex.STUDENT_MANAGER_SYSTEM);
            if (stduent == null)
            {
                var result = _db.QuerySingle<LexueidRelationIDCard>(c => c.idcard == idcard);
                if (result == null)
                {
                    model.code = 0;
                    model.message = "此身份证未注册，请先注册。";
                }
                else
                {
                    var usertable = _db.QuerySingle<UserTable>(u => u.lexueid == result.lexueid && u.UserName == name);
                    if (usertable == null)
                    {
                        model.code = 0;
                        model.message = "身份证和姓名不匹配，请重新输入。";
                    }
                    else
                    {
                        stduent = new StudentDataImport
                        {
                            SchoolNo = usertable.FK_SchoolID,
                            SchoolName = GlabolData.Instance._dicSchool[usertable.FK_SchoolID],
                            SpecailtyId = usertable.FK_Specialty.ToString(),
                            SpecialtyName = GlabolData.Instance._dicSpecialty[usertable.FK_Specialty.ToString()],
                            IDCard = result.idcard,
                            Kaohao = usertable.KaoHao,
                            UserName = usertable.UserName,
                            Pwd = usertable.UserPassword,
                            Lexueid = usertable.lexueid,
                            PhoneNumber = result.phoneNumber,
                        };
                        model.code = 1;
                        model.student = stduent;
                    }

                }
            }
            else
            {
                if (stduent.UserName == name)
                {
                    model.code = 1;
                    model.student = stduent;
                }
                else
                {
                    model.code = 0;
                    model.message = "身份证和姓名不匹配，请重新输入。";
                }
            }
            return model;
        }


        public ResultModel UserBind(string kaohao, string userName, string pwd, string idcard, string phone, string qq)
        {
            if (!DataVerify.Instance.CheckIDCard(idcard))
            {
                return new ResultModel { code = 0, message = "请输入正确的身份证号码" };
            }
            //if (!DataVerify.Instance.CheckPhoneNumber(phone))
            //{
            //    return new ResultModel { code = 0, message = "请输入正确的手机号码" };
            //}

            idcard = idcard.ToUpper();
            var student = RedisHelper.Instance.GetModel<StudentDataImport>(idcard, RedisIndex.STUDENT_MANAGER_SYSTEM);
            if (student != null)
            {
                if (student.Kaohao != kaohao)
                    return new ResultModel { code = 0, message = "此身份证已绑定其他账号，请填写本人身份证" };
                if (student.UserName != userName || student.Pwd != pwd)
                    return new ResultModel { code = 0, message = "请检查考号、姓名和密码是否正确" };
                return new ResultModel { code = 0, message = "您已经绑定过了，请勿重复绑定" };
            }

            string sql = "SELECT lexueid FROM UserTable where KaoHao=@kaohao and UserName=@userName and UserPassword=@pwd";
            List<string> list = _db.QueryBySql<string>(sql, new SqlParameter("@kaohao", kaohao), new SqlParameter("@userName", userName), new SqlParameter("@pwd", pwd));

            if (list == null || list.Count == 0)
                return new ResultModel { code = 0, message = "请检查考号、姓名和密码是否正确" };

            var lexueid = list[0];
            bool success = _db.Add(new LexueidRelationIDCard { idcard = idcard, lexueid = lexueid, phoneNumber = phone, qqNumber = qq });
            if (success)
            {
                return new ResultModel { code = 1, message = "绑定成功" };
            }
            else
            {
                return new ResultModel { code = 0, message = "服务器错误，请稍后再试" };
            }
        }

        public ResultModel GetStudentList(int? page, int? pageSize, string schoolNo, string specialtyId, string studentName, bool pagination = true)
        {
            if (string.IsNullOrEmpty(schoolNo))
            {
                return new ResultModel { code = 0, message = "学校参数不合法" };
            }

            var user = _db.Set<UserTable>().Where(u => u.FK_SchoolID == schoolNo && u.UserType == 1 && u.IsDelete == 0);
            if (!string.IsNullOrEmpty(specialtyId))
            {
                int s_id = Convert.ToInt32(specialtyId);
                user = user.Where(u => u.FK_Specialty == s_id);
            }

            if (!string.IsNullOrEmpty(studentName))
            {
                user = user.Where(u => u.UserName.Contains(studentName));
            }

            int total = user.Count();

            var cardTable = _db.Set<LexueidRelationIDCard>().GroupBy(c => new { c.lexueid, c.idcard }).Select(c => new { c.Key.lexueid, c.Key.idcard });

            try
            {
                var data = (from a in user
                            join b in cardTable
                            on a.lexueid equals b.lexueid
                            into temp
                            from t in temp.DefaultIfEmpty()
                            select new StudentMsgModel
                            {
                                IDCard = t.idcard,
                                SpecialtyName = a.FK_SpecialtyName,
                                Kaohao = a.KaoHao,
                                Lexueid = a.lexueid,
                                UserName = a.UserName,
                                Pwd = a.UserPassword
                            }).OrderByDescending(c => c.IDCard);
                List<StudentMsgModel> list;
                if (pagination)
                {
                    int page1 = page ?? 1;
                    int pageSize1 = pageSize ?? 20;
                    list = data.Skip(pageSize1 * (page1 - 1)).Take(pageSize1).ToList();
                }
                else
                {
                    list = data.ToList();
                }
                return new ResultModel
                {
                    code = 1,
                    data = new { total, rows = list }
                };
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return new ResultModel
                {
                    code = 0,
                    message = "服务器错误"
                };
            }
        }

        public ResultModel DownloadStudent(string schoolNo, string specialtyId, string filepath)
        {
            var result = GetStudentList(null, null, schoolNo, specialtyId, null, false);
            if (result.code == 0)
                return new ResultModel { code = 0, message = result.message };

            List<StudentMsgModel> studentlist = result.data.rows;

            Dictionary<string, List<StudentMsgModel>> dic = new Dictionary<string, List<StudentMsgModel>>();

            studentlist.GroupBy(s => s.SpecialtyName).Select(s => s.Key).ToList().ForEach(s =>
            {
                dic.Add(s, studentlist.FindAll(l => l.SpecialtyName == s));
            });

            if (dic.Count == 0)
                return new ResultModel { code = 0, message = "没有需要导出的学生" };

            if (ExcelHelper.Instance.StudentConvertToFile(dic, filepath))
                return new ResultModel { code = 1 };
            else
                return new ResultModel { code = 0, message = "导出失败" };
        }
    }
}