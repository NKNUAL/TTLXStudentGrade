﻿using IBLL.Helper;
using IBLL.ServiceModels;
using IDAL;
using IDAL.DataContext;
using IDAL.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Impl
{
    public class PhoneService : IPhoneService
    {

        public IDbUserEntity _db { get; set; }
        public IBaseService _baseService { get; set; }
        public PhoneService() { }

        public PhoneService(IDbUserEntity db, IBaseService baseService)
        {
            this._db = db; _baseService = baseService;
        }

        public List<PhoneLimitModel> GetSchoolLimitCount(string schoolNo)
        {
            var limits = _db.Set<SchoolPhoneUserLimit>()
                .Where(s => s.SchoolNo == schoolNo)
                .ToList();

            var specialties = _baseService.GetSpecialty();

            List<PhoneLimitModel> models = new List<PhoneLimitModel>();

            foreach (var specialty in specialties)
            {
                var limit = limits.Find(s => s.SpecialtyId == specialty.SpecialtyId);
                if (limit == null)
                {
                    models.Add(new PhoneLimitModel
                    {
                        SpecialtyId = specialty.SpecialtyId,
                        SpecialtyName = specialty.SpecialtyName,
                        LimitCount = 0
                    });
                }
                else
                {
                    models.Add(new PhoneLimitModel
                    {
                        SpecialtyId = specialty.SpecialtyId,
                        SpecialtyName = specialty.SpecialtyName,
                        LimitCount = limit.LimitCount ?? 0
                    });
                }
            }
            return models;

        }

        public bool UpdateSchoolLimit(string schoolNo, string specialtyId, int limitCount)
        {
            var limit = _db.Set<SchoolPhoneUserLimit>()
                .FirstOrDefault(s => s.SchoolNo == schoolNo && s.SpecialtyId == specialtyId);
            if (limit == null)
            {
                _db.Add(new SchoolPhoneUserLimit
                {
                    SchoolNo = schoolNo,
                    SpecialtyId = specialtyId,
                    LimitCount = limitCount
                });
            }
            else
            {
                limit.LimitCount = limitCount;
            }
            return _db.SaveChanges();
        }

        public int GetSchoolLimit(string schoolNo, string specialtyId)
        {
            var limit = _db.Set<SchoolPhoneUserLimit>()
            .FirstOrDefault(s => s.SchoolNo == schoolNo && s.SpecialtyId == specialtyId);
            if (limit == null)
            {
                return 0;
            }
            return limit.LimitCount ?? 0;
        }

        public bool AuthVerify(string schoolNo, string gpCode)
        {
            Db0905Context db = DbContextFactory.GetDb0905Context();

            return db.Base_School
                .Count(s => s.SchoolNo == schoolNo && s.GPCode == gpCode) > 0;
        }

        public ResultModel UploadStudent(UploadUserServiceModel uploadUser)
        {
            foreach (var item in uploadUser.Specialties)
            {
                int limitCount = GetSchoolLimit(uploadUser.SchoolNo, item.SpecialtyId);
                int useCount = GetSchoolHasUploadCount(uploadUser.SchoolNo, int.Parse(item.SpecialtyId));
                if (item.Students.Count > (limitCount - useCount))
                {
                    return new ResultModel { code = 0, message = item.SpecialtyName + "超出上传数量限制，请删除部分学生后再重试" };
                }
            }

            if (!GlabolData.Instance.Datas.ContainsKey(QueueDataType.Upload))
            {
                GlabolData.Instance.Datas.Add(QueueDataType.Upload, new PhoneUserQueueData());
            }
            GlabolData.Instance.Datas[QueueDataType.Upload].Enqueue(uploadUser);

            return new ResultModel { code = 1 };

        }

        public int GetSchoolHasUploadCount(string schoolNo, int specialtyId)
        {
            return
                _db.Set<UserTable>()
                .Count(s => s.FK_SchoolID == schoolNo && s.FK_Specialty == specialtyId &&
                s.IsDelete == 0 && s.UserType == 1 && s.Roid == 5);
        }

        public List<PhoneUserModel> GetPhoneUser(string schoolNo, string specialtyCode, string studentName)
        {

            var query = _db.Set<UserTable>().Where(s => s.FK_SchoolID == schoolNo);

            if (!string.IsNullOrEmpty(specialtyCode))
            {
                var sId = _db.Set<Base_specialtyType>().FirstOrDefault(s => s.SpecialtyCode == specialtyCode).No;
                int specId = int.Parse(sId);
                query = query.Where(s => s.FK_Specialty == specId);
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                query = query.Where(s => s.UserName.Contains(studentName));
            }

            return query
                .Where(s => s.IsDelete == 0 && s.UserType == 1 && s.Roid == 5)
                .Select(s => new PhoneUserModel
                {
                    ClassCode = s.UserClassCode,
                    ClassName = s.UserClass,
                    UserName = s.UserName,
                    Password = s.UserPassword,
                    Lexueid = s.lexueid,
                    Kaohao = s.KaoHao,
                    SpecialtyName = s.FK_SpecialtyName
                }).ToList();
        }

        public ResultModel UserEditInfo(string lexueid, string password, string userName)
        {
            if (string.IsNullOrEmpty(password))
                return new ResultModel { code = 0, message = "新密码不能为空" };

            if (string.IsNullOrEmpty(userName))
                return new ResultModel { code = 0, message = "姓名不能为空" };

            var user = _db.Set<UserTable>().FirstOrDefault(s => s.lexueid == lexueid && s.IsDelete == 0);

            if (user == null)
                return new ResultModel { code = 0, message = "用户不存在或已删除" };

            user.UserPassword = password;
            user.UserName = userName;
            _db.SaveChanges();

            return new ResultModel { code = 1 };
        }
    }
}
