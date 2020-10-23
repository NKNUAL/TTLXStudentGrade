using IBLL.Helper;
using IBLL.ServiceModels;
using IDAL;
using IDAL.ServerModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace IBLL.Impl
{
    public class GradeService : IGradeService
    {
        public IDbServerEntity _db { get; set; }

        public GradeService() { }

        public GradeService(IDbServerEntity db)
        {
            this._db = db;
        }


        public IDictionary<string, string> GetClassBySpecialty(string schoolId, int specialtyId)
        {
            var result = _db.Set<UserTable>().Where(u => u.FK_SchoolID == schoolId && u.FK_Specialty == specialtyId && u.UserType == 1)
                .GroupBy(u => u.UserClass)
                .Select(u => u.Key)
                .ToList();
            IDictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item))
                    dic.Add(item, item);
            }
            return dic;
        }

        public List<GradeModel> GetUserGrade(GradeQueryModel query)
        {
            var users = _db.Set<UserTable>().Where(ut => ut.UserType == 1 && ut.FK_SchoolID == query.SchoolId && ut.FK_Specialty == query.SpecialtyId);
            if (!string.IsNullOrEmpty(query.ClassCode) && query.ClassCode != "total")
                users = users.Where(u => u.UserClass == query.ClassCode);
            var result = from ut in users
                         join mp in _db.Set<MockTestPaperScoreResult>().Where(m => m.paperid == query.PaperId)
                             .GroupBy(m => new { m.lexueid })
                             .Select(r => new
                             {
                                 Lexueid = r.Key.lexueid,
                                 AvgScore = r.Average(g => g.studentScore),
                                 MinScore = r.Min(g => g.studentScore),
                                 MaxScore = r.Max(g => g.studentScore),
                                 WorkCount = r.Count()
                             }) on ut.lexueid equals mp.Lexueid
                         select (new GradeModel
                         {
                             Lexueid = ut.lexueid,
                             Kaohao = ut.KaoHao,
                             AvgScore = mp.AvgScore,
                             ClassName = ut.UserClass,
                             MaxScore = mp.MaxScore,
                             MinScore = mp.MinScore,
                             UserName = ut.UserName,
                             WorkCount = mp.WorkCount
                         });
            return result.OrderByDescending(r => r.MaxScore).ToList();
        }

        public IDictionary<int, string> GetPapers(string schoolId, int specialtyId)
        {
            List<PaperModel> result = null;
            if (specialtyId == 0)
            {
                //return _db.MockTestPaper_Computer.ToDictionary(k => k.ExamPaperID, v => v.ExamPaperName);
                result = _db.QueryBySql<PaperModel>(@" select distinct a.paperId PaperId,c.ExamPaperName PaperName from (select distinct paperId,lexueid from MockTestPaperScoreResult )a right join (select * from UserTable where FK_Specialty = @specialtyId and UserType = 1 and FK_SchoolID = @schoolId) b on a.lexueid = b.lexueid left join MockTestPaper_Computer c on a.paperId = c.ExamPaperId where a.paperId is not null", new SqlParameter("@specialtyId", specialtyId), new SqlParameter("@schoolId", schoolId));

            }
            else
            {
                //return _db.MockTestPaper.Where(m => m.FK_Specialty == specialtyId).ToDictionary(k => k.ExamPaperID, v => v.ExamPaperName);
                result = _db.QueryBySql<PaperModel>(@" select distinct a.paperId PaperId,c.ExamPaperName PaperName from (select distinct paperId,lexueid from MockTestPaperScoreResult )a right join (select * from UserTable where FK_Specialty = @specialtyId and UserType = 1 and FK_SchoolID = @schoolId) b on a.lexueid = b.lexueid left join MockTestPaper c on a.paperId = c.ExamPaperId where a.paperId is not null", new SqlParameter("@specialtyId", specialtyId), new SqlParameter("@schoolId", schoolId));
            }
            IDictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in result)
            {
                if (item.PaperId != null)
                    dic.Add(Convert.ToInt32(item.PaperId), item.PaperName);
            }
            return dic;
        }

        public bool OutputExcel(List<GradeModel> grades, string filepath)
        {
            return new GradeHelper().UserConvertToExcel(grades, filepath);
        }

        public string GetPaperNameById(int paperId, int specialtyId)
        {
            if (specialtyId == 0)
                return _db.QuerySingle<MockTestPaper_Computer>(m => m.ExamPaperID == paperId).ExamPaperName;
            else
                return _db.QuerySingle<MockTestPaper>(m => m.ExamPaperID == paperId).ExamPaperName;
        }

        public List<StudentGradeModel> GetStudentGradeDetails(string lexueid, string paperId)
        {
            string name = _db.QuerySingle<UserTable>(u => u.lexueid == lexueid).UserName;
            var result = _db.Set<MockTestPaperScoreResult>().Where(m => m.lexueid == lexueid && m.paperid == paperId)
                .OrderByDescending(m => m.submitTime)
                .Select(m => new StudentGradeModel
                {
                    UserScore = m.studentScore,
                    UserSubmitDate = m.submitTime
                }).ToList();
            foreach (var item in result)
            {
                item.UserName = name;
            }
            return result;
        }
    }
}