using Application;
using IBLL.ServiceModels;
using IDAL;
using IDAL.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Impl
{
    public class BaseService : IBaseService
    {
        public IDbUserEntity _db { get; set; }

        public BaseService() { }

        public BaseService(IDbUserEntity db)
        {
            this._db = db;
        }

        public List<AreaVKModel> GetAreas(string provinceNo)
        {
            string key = "base-province_" + provinceNo;
            var list = RedisHelper.Instance.GetModel<List<AreaVKModel>>(key, RedisIndex.STUDENT_MANAGER_SYSTEM);
            if (list == null)
            {
                list = _db.Set<Base_Area>().Where(a => a.FK_Province == provinceNo).Select(a => new AreaVKModel
                {
                    AreaName = a.AreaName,
                    AreaNo = a.AreaNo
                }).ToList();
                RedisHelper.Instance.SetModel(key, list, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }
            return list;
        }

        public List<SchoolKVModel> GetSchoolByArea(string areaNo)
        {
            string key = "base-area_" + areaNo;
            var list = RedisHelper.Instance.GetModel<List<SchoolKVModel>>(key, RedisIndex.STUDENT_MANAGER_SYSTEM);
            if (list == null)
            {
                list = _db.Set<Base_School>().Where(a => a.FK_Area == areaNo).Select(a => new SchoolKVModel
                {
                    SchoolName = a.SchoolName,
                    SchoolNo = a.SchoolNo
                }).ToList();
                RedisHelper.Instance.SetModel(key, list, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }
            return list;
        }

        public List<SchoolKVModel> GetSchools(string schoolNo = null)
        {
            string key = "base-schools";
            if (RedisHelper.Instance.IsSet(key, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                var list = RedisHelper.Instance.GetModel<List<SchoolKVModel>>(key, RedisIndex.STUDENT_MANAGER_SYSTEM);
                if (!string.IsNullOrEmpty(schoolNo))
                {
                    list = list.Where(s => s.SchoolNo == schoolNo).ToList();
                }
                return list;
            }
            else
            {
                var schools = _db.Set<Base_School>().Where(s => s.FK_Province == "17");

                var list = schools.Select(s => new SchoolKVModel
                {
                    SchoolNo = s.SchoolNo,
                    SchoolName = s.SchoolName
                }).ToList();
                RedisHelper.Instance.SetModel(key, list, RedisIndex.STUDENT_MANAGER_SYSTEM);
                if (!string.IsNullOrEmpty(schoolNo))
                {
                    list = list.Where(s => s.SchoolNo == schoolNo).ToList();
                }
                return list;
            }
        }

        public List<SpecialtyKVModel> GetSpecialty()
        {
            List<SpecialtyKVModel> list = new List<SpecialtyKVModel>();
            string key = "base-specialty";
            if (RedisHelper.Instance.IsSet(key, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                list.AddRange(RedisHelper.Instance.GetModel<List<SpecialtyKVModel>>(key, RedisIndex.STUDENT_MANAGER_SYSTEM));
            }
            else
            {
                var temp = _db.Set<Base_specialtyType>().Select(s => new SpecialtyKVModel
                {
                    SpecialtyId = s.No,
                    SpecialtyName = s.Name
                }).ToList();
                list.AddRange(temp);
                RedisHelper.Instance.SetModel(key, temp, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }
            return list;
        }

        public List<AreaSchoolModel> GetAreaSchools()
        {
            List<AreaSchoolModel> list = new List<AreaSchoolModel>();
            var areas = GetAreas("17");
            foreach (var area in areas)
            {
                list.Add(new AreaSchoolModel
                {
                    AreaNo = area.AreaNo,
                    Schools = GetSchoolByArea(area.AreaNo)
                });
            }
            return list;
        }
    }
}
