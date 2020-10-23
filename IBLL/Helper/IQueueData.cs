using Application;
using Application.Logger;
using IBLL.ServiceModels;
using IDAL.DataContext;
using IDAL.DbModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Helper
{
    public interface IQueueData
    {
        void ToDb();
        void Enqueue(object obj);
    }

    public enum QueueDataType
    {
        Register,
        Upload
    }

    public class RegisterQueueData : IQueueData
    {
        private ConcurrentQueue<StudentDataImport> _queue = new ConcurrentQueue<StudentDataImport>();
        private bool _isEnd = true;
        public void Enqueue(object obj)
        {
            if (obj is StudentDataImport)
            {
                StudentDataImport t = obj as StudentDataImport;
                _queue.Enqueue(t);
            }
        }
        public void ToDb()
        {

            if (!_isEnd)
                return;
            _isEnd = false;
            if (_queue.Count > 0)
                LogWriter.Instance.AddLog("队列中当前剩余数量：" + _queue.Count);

            bool hasData = _queue.TryDequeue(out StudentDataImport data);



            if (hasData)
            {
                using (DbUseContext db = new DbUseContext())
                {
                    while (hasData)
                    {

                        if (!data.IsHadUpload)
                        {
                            try
                            {

                                db.LexueidRelationIDCard.Add(new LexueidRelationIDCard
                                {
                                    idcard = data.IDCard,
                                    lexueid = data.Lexueid,
                                    phoneNumber = data.PhoneNumber,
                                    qqNumber = data.QQ
                                });
                                db.UserTable.Add(new UserTable
                                {
                                    FK_School = data.SchoolName,
                                    FK_SchoolID = data.SchoolNo,
                                    FK_Specialty = Convert.ToInt32(data.SpecailtyId),
                                    FK_SpecialtyName = data.SpecialtyName,
                                    lexueid = data.Lexueid,
                                    UserDesc = "注册学生",
                                    UserPassword = data.Pwd,
                                    IsDelete = 0,
                                    IsLocked = 0,
                                    UserName = data.UserName,
                                    UserType = 1,
                                    UseState = 1,
                                    KaoHao = data.Kaohao,
                                    Roid = 6,
                                    RegisterDate = data.RegisterDate
                                });
                                db.SaveChanges();
                                data.IsHadUpload = true;
                                RedisHelper.Instance.SetModel(data.IDCard, data, RedisIndex.STUDENT_MANAGER_SYSTEM);
                            }
                            catch (Exception ex)
                            {
                                LogWriter.Instance.AddLog(Newtonsoft.Json.JsonConvert.SerializeObject(data) + "\r\n" + ex.Message);
                            }
                        }

                        hasData = _queue.TryDequeue(out data);
                    }

                }
            }
            _isEnd = true;
        }
    }

    public class PhoneUserQueueData : IQueueData
    {
        private ConcurrentQueue<UploadUserServiceModel> _queue { get; set; } = new ConcurrentQueue<UploadUserServiceModel>();
        private bool _isEnd { get; set; } = true;
        public void ToDb()
        {
            if (!_isEnd)
                return;
            _isEnd = false;
            if (_queue.Count > 0)
                LogWriter.Instance.AddLog("学生上传队列中当前剩余数量：" + _queue.Count);

            bool hasData = _queue.TryDequeue(out UploadUserServiceModel data);

            if (hasData)
            {
                using (DbUseContext db = new DbUseContext())
                {
                    Dictionary<string, string> dicSpecialty = db.Base_specialtyType.ToDictionary(k => k.No, v => v.Name);
                    Dictionary<string, string> dicSchool = db.Base_School.ToDictionary(k => k.SchoolNo, v => v.SchoolName);

                    while (hasData)
                    {
                        try
                        {
                            string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string maxKaohao = "";
                            string kaohaoKey = "MaxKaohao";
                            int currKaohao = 0;

                            int totalStudentCount = 0;
                            data.Specialties.ForEach(s => totalStudentCount += s.Students.Count);

                            if (RedisHelper.Instance.IsSet(kaohaoKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
                            {
                                maxKaohao = RedisHelper.Instance.StringGet(kaohaoKey, RedisIndex.STUDENT_MANAGER_SYSTEM);
                                currKaohao = Convert.ToInt32(maxKaohao) + 1;
                                maxKaohao = (Convert.ToInt32(maxKaohao) + totalStudentCount).ToString();
                            }
                            else
                            {
                                int iMaxKaohao = db.Database.SqlQuery<int?>("select Max(Cast(Right(KaoHao,LEN(KaoHao)-2) as int)) from UserTable  where UserType=1 and KaoHao like '[kK][sS]%'").FirstOrDefault() ?? 0;
                                maxKaohao = (iMaxKaohao + totalStudentCount).ToString();
                                currKaohao = iMaxKaohao + 1;
                            }
                            RedisHelper.Instance.StringSet(kaohaoKey, maxKaohao, RedisIndex.STUDENT_MANAGER_SYSTEM);


                            foreach (var item in data.Specialties)
                            {

                                foreach (var student in item.Students)
                                {
                                    db.UserTable.Add(new UserTable
                                    {
                                        FK_SchoolID = data.SchoolNo,
                                        FK_School = dicSchool[data.SchoolNo],
                                        FK_Specialty = Convert.ToInt32(item.SpecialtyId),
                                        FK_SpecialtyName = dicSpecialty[item.SpecialtyId],
                                        RegisterDate = dateNow,
                                        UserClass = student.ClassName,
                                        UserClassCode = student.ClassCode,
                                        UserName = student.UserName,
                                        UserPassword = student.Password,
                                        Roid = 5,
                                        IsDelete = 0,
                                        KaoHao = "ks" + currKaohao,
                                        UserDesc = "学校导入学生",
                                        lexueid = "17" + data.SchoolNo + string.Format("{0:000}", currKaohao++),
                                        UserType = 1,
                                        UseState = 1,
                                        IsLocked = 0,
                                    });
                                }
                            }
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            LogContent.Instance.WriteLog(new AppOpLog()
                            {
                                LogMessage = "[手机用户上传出错]：" + ex.Message + $"。[学校：{data.SchoolNo}]",
                                MemberID = "学生数据上传",
                                MethodName = "[GlabolData:StudentToDb]"
                            }, Log4NetLevel.Error);
                            _queue.Enqueue(data);
                        }

                        hasData = _queue.TryDequeue(out data);
                    }
                }
            }

            _isEnd = true;
        }
        public void Enqueue(object obj)
        {
            if (obj is UploadUserServiceModel)
            {
                UploadUserServiceModel t = obj as UploadUserServiceModel;
                _queue.Enqueue(t);
            }
        }
    }
}
