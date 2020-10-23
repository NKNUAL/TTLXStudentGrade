using Application;
using Application.Common;
using Application.Enum;
using IBLL.Helper;
using IBLL.ServiceModels;
using IDAL;
using IDAL.Enum;
using IDAL.Impl;
using IDAL.ServerModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBLL.Impl
{
    public class MonthService : IMonthService
    {

        public IDbServerEntity _db { get; set; }

        public MonthService() { }

        public MonthService(IDbServerEntity db)
        {
            this._db = db;
        }

        public TreeModel GetSchoolTree(int planId)
        {

            TreeModel tree = new TreeModel
            {
                id = "-1",
                text = "湖北省",
                children = new List<Children>(),
                attributes = new Attributes { Level = 0 }
            };

            string key = "plan_school_" + planId;

            List<SqlQuerySchoolModel> schools;
            if (RedisHelper.Instance.IsSet(key))
            {
                schools = RedisHelper.Instance.GetModel<List<SqlQuerySchoolModel>>(key);
            }
            else
            {
                schools = GetSchool(planId, false);
                RedisHelper.Instance.SetModel(key, schools);
            }


            foreach (var item in schools)
            {
                Children chil = tree.children.Find(c => c.id == item.AreaNo);
                if (chil == null)
                {
                    chil = new Children
                    {
                        id = item.AreaNo,
                        text = item.AreaName,
                        children = new List<Children>(),
                        attributes = new Attributes { Level = 1 }
                    };
                    tree.children.Add(chil);
                }
                chil.children.Add(new Children
                {
                    id = item.SchoolNo,
                    text = item.SchoolName,
                    attributes = new Attributes { Level = 2 }
                });
            }
            tree.children = tree.children.Where(c => c.id != "17").ToList();
            return tree;

        }

        /// <summary>
        /// 通过考试计划获取所有参加考试的学校
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        private List<SqlQuerySchoolModel> GetSchool(int planId, bool include = true)
        {
            string viewName = include ? "V_MonthExamSchools" : "V_MonthExamSchools_Old";
            string sql = $@"select SchoolNo,SchoolName,AreaNo,AreaName from {viewName} where PlanId='{planId}' order by SchoolNo";
            var schools = _db.QueryBySql<SqlQuerySchoolModel>(sql);
            return schools;
        }

        public Dictionary<int, string> GetMonthSpecialty()
        {
            var user = CookieHelper.GetUserData();
            string sql = "";
            if (user.UserRole == UserRole.Admin)
            {
                sql = "select distinct SpecialtyId SpecialtyType,SpecialtyName from ExaminationPlan where IsEnd=1 and ExamType=6" +
                    "union select SpecialtyId SpecialtyType,SpecialtyName from CulturalExamPlan ";
            }
            else if (user.UserRole == UserRole.SchoolAdmin)
            {
                sql = $@"select SpecialtyType,SpecialtyName from V_MonthExamSpecialty where SchoolNo='{user.SchoolCode}' order by SpecialtyType";
            }
            if (sql == "") return new Dictionary<int, string>();
            List<QuerySpecialtyModel> specialties = _db.QueryBySql<QuerySpecialtyModel>(sql);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in specialties)
            {
                if (item.SpecialtyType != null && !dic.ContainsKey((int)item.SpecialtyType))
                {
                    dic.Add((int)item.SpecialtyType, item.SpecialtyName);
                }
            }
            return dic;

        }

        public Dictionary<int, string> GetPlanBySpecialty(int specialtyType)
        {
            var user = CookieHelper.GetUserData();
            string sql = "";
            if (user.UserRole == UserRole.Admin)
            {
                sql = $"select Id PlanId,ksjhName PlanName from ExaminationPlan where  SpecialtyId = '{specialtyType}' and ExamType=6 and IsEnd=1 order by Id desc";
            }
            else if (user.UserRole == UserRole.SchoolAdmin)
            {
                sql = $@"select PlanId,PlanName from V_MonthExamPlan where SchoolNo='{user.SchoolCode}' and SpecialtyId='{specialtyType}' group by PlanId,PlanName order by PlanId desc";

            }
            else if (user.UserRole == UserRole.Student)
            {
                sql = $@"select PlanId,PlanName from V_MonthExamPlan where SchoolNo='{user.SchoolCode}' and SpecialtyId='{specialtyType}' and StudentId='{user.Lexueid}' group by PlanId,PlanName order by PlanId desc";
            }
            if (sql == "") return new Dictionary<int, string>();
            var specialties = _db.QueryBySql<QueryPlanModel>(sql);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in specialties)
            {
                if (item.PlanId != null && !dic.ContainsKey((int)item.PlanId))
                {
                    dic.Add((int)item.PlanId, item.PlanName);
                }
            }
            return dic;
        }

        public Dictionary<string, string> GetTotalExamSchool(int planId)
        {
            var user = CookieHelper.GetUserData();
            if (user.UserRole == UserRole.SchoolAdmin)
            {
                return new Dictionary<string, string>
                {
                    { user.SchoolCode, user.SchoolName }
                };
            }

            if (user.UserRole == UserRole.Admin)
            {

                string key = "plan_school_" + planId;

                List<SqlQuerySchoolModel> schools;
                if (RedisHelper.Instance.IsSet(key))
                {
                    schools = RedisHelper.Instance.GetModel<List<SqlQuerySchoolModel>>(key);
                }
                else
                {
                    schools = GetSchool(planId);
                    RedisHelper.Instance.SetModel(key, schools);
                }


                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (var school in schools)
                {
                    if (!string.IsNullOrEmpty(school.SchoolNo) && !dic.ContainsKey(school.SchoolNo))
                    {
                        dic.Add(school.SchoolNo, school.SchoolName);
                    }
                }
                return dic;
            }

            return new Dictionary<string, string>();
        }

        private static readonly object stulock = new object();

        public List<StudentScoreModel> GetStudentScore(int planId, string schoolCode, ref SchoolGradeModel model)
        {
            GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, planId);

            //studentScores = studentScores.ConvertAll(s => s.Clone()).ToList();
            //studentCultureScores = studentCultureScores.ConvertAll(s => s.Clone()).ToList();

            SchoolGradeModel modelProvince = new SchoolGradeModel();

            if (!string.IsNullOrEmpty(schoolCode))
            {
                studentScores = studentScores.Where(s => s.SchoolCode == schoolCode).ToList();
                studentCultureScores = studentCultureScores.Where(s => s.SchoolCode == schoolCode).ToList();
            }

            model.JoinSpecialtyCount = studentScores.Count;
            if (studentScores != null && studentScores.Count > 0)
            {
                model.SpecialtyMaxScore = studentScores.Max(s => s.StudentScore);
                model.SpecialtyMinScore = studentScores.Min(s => s.StudentScore);
                model.SpecialtyAvgScore = Math.Round(studentScores.Average(s => s.StudentScore), 2);
            }

            model.JoinCultureCount = studentCultureScores.Count;
            if (studentCultureScores != null && studentCultureScores.Count > 0)
            {
                model.CultureMaxScore = studentCultureScores.Max(s => s.CultureStudentScore);
                model.CultureMinScore = studentCultureScores.Min(s => s.CultureStudentScore);
                model.CultureAvgScore = Math.Round(studentCultureScores.Average(s => s.CultureStudentScore), 2);
                foreach (var item in studentScores)
                {
                    var temp = studentCultureScores.Find(s => s.Lexueid == item.Lexueid);
                    if (temp != null)
                    {
                        item.ChineseScore = temp.ChineseScore;
                        item.EnglishScore = temp.EnglishScore;
                        item.MathScore = temp.MathScore;
                        item.CultureStudentScore = temp.CultureStudentScore;
                        studentCultureScores.Remove(temp);
                    }
                }
                model.JoinBothCount = model.JoinCultureCount - studentCultureScores.Count;
                studentScores.AddRange(studentCultureScores);
            }

            studentScores = studentScores.OrderByDescending(s => s.StudentTotalScore).ToList();

            Dictionary<string, StudentScoreModel> dicPrevious = new Dictionary<string, StudentScoreModel>();

            int provinceRank = 1;
            if (studentScores.Count > 0)
            {
                StudentScoreModel previous = studentScores[0];
                previous.Rank = provinceRank;
                int i = 0;
                var arrString = new string[]
                {
                    ((int)QuestionsTypes.Danxuan).ToString(),
                    ((int)QuestionsTypes.Duoxuan).ToString(),
                    ((int)QuestionsTypes.Panduan).ToString()
                };
                foreach (var student in studentScores)
                {
                    if (!string.IsNullOrEmpty(student.AnswerDetail))
                    {
                        var answerDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreResultDetail>>(student.AnswerDetail);
                        student.SelectScore = answerDetails.Where(s => arrString.Contains(s.QueType)).Sum(s => double.Parse(s.GetScore));
                        student.ProgramScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Biancheng).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.WinScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Windows).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.WordScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Word).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.NetScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Wangluo).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.PptScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.PowerPoint).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.ExcelScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Excel).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.AccessScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Access).ToString()).Sum(s => double.Parse(s.GetScore));
                        student.AnswerDetail = null;
                    }
                    if (i != 0)
                    {
                        if (student.StudentTotalScore == previous.StudentTotalScore)
                        {
                            student.Rank = previous.Rank;
                        }
                        else
                        {
                            student.Rank = provinceRank;
                        }
                        previous = student;
                    }
                    if (i == 0) i = 1;
                    provinceRank++;
                }
            }
            return studentScores;

        }

        private void GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, int planId)
        {
            lock (stulock)
            {
                studentScores = MonthHelper.Instance.GetSpecialtyStudentScore(planId);
                studentCultureScores = MonthHelper.Instance.GetCultureStudentScore(planId);

                if (studentScores == null)
                {
                    string key_zy = planId + "_studentDetail_zy";


                    if (RedisHelper.Instance.IsSet(key_zy))
                    {
                        studentScores = RedisHelper.Instance.GetModel<List<StudentScoreModel>>(key_zy);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        if (RedisHelper.Instance.IsSet(key_zy))
                        {
                            studentScores = RedisHelper.Instance.GetModel<List<StudentScoreModel>>(key_zy);
                        }
                        else
                        {
                            string sqlQuery = "select Lexueid,UserName,StudentScore,SchoolCode,SchoolName,SpecialtyName,AnswerTime,AnswerDetail" +
                                        $" from V_StudentScoreDetail where PlanId={planId} order by StudentScore desc";
                            studentScores = _db.QueryBySql<StudentScoreModel>(sqlQuery);

                            RedisHelper.Instance.SetModel(key_zy, studentScores);
                        }
                        MonthHelper.Instance.SetSpecialtyStudentScore(planId, studentScores);

                    }

                }

                if (studentCultureScores == null)
                {
                    string key_wh = planId + "_studentDetail_wh";
                    if (RedisHelper.Instance.IsSet(key_wh))
                    {
                        studentCultureScores = RedisHelper.Instance.GetModel<List<StudentScoreModel>>(key_wh);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        if (RedisHelper.Instance.IsSet(key_wh))
                        {
                            studentCultureScores = RedisHelper.Instance.GetModel<List<StudentScoreModel>>(key_wh);
                        }
                        else
                        {
                            string sqlCultureQuery = "select Lexueid,UserName,CultureStudentScore,ChineseScore,EnglishScore,MathScore," +
                                                    "SchoolCode,SchoolName,SpecialtyName" +
                                                $" from V_CultureExamStudentScore where PlanId={planId}";
                            studentCultureScores = _db.QueryBySql<StudentScoreModel>(sqlCultureQuery);

                            RedisHelper.Instance.SetModel(key_wh, studentCultureScores);
                        }
                        MonthHelper.Instance.SetCultureStudentScore(planId, studentCultureScores);
                    }
                }



            }
        }

        public List<StudentScoreModel> GetStudentScore(int? page, int? rows, int planId, out int total)
        {

            GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, planId);

            //studentScores = studentScores.ConvertAll(s => s.Clone()).ToList();
            //studentCultureScores = studentCultureScores.ConvertAll(s => s.Clone()).ToList();

            if (studentCultureScores != null && studentCultureScores.Count > 0)
            {
                foreach (var item in studentScores)
                {
                    var temp = studentCultureScores.Find(s => s.Lexueid == item.Lexueid);
                    if (temp != null)
                    {
                        item.ChineseScore = temp.ChineseScore;
                        item.EnglishScore = temp.EnglishScore;
                        item.MathScore = temp.MathScore;
                        item.CultureStudentScore = temp.CultureStudentScore;
                        studentCultureScores.Remove(temp);
                    }
                }
                studentScores.AddRange(studentCultureScores);
            }

            int page1 = page ?? 1;
            int pageSize1 = rows ?? 20;

            total = studentScores.Count;

            studentScores = studentScores.OrderByDescending(s => s.StudentTotalScore)
                .Skip(pageSize1 * (page1 - 1))
                .Take(pageSize1)
                .ToList();

            return studentScores;

        }

        public SchoolResultModel GetSchoolBaseMessage(int planId, string schoolCode)
        {
            SchoolGradeModel msgSchool = new SchoolGradeModel();
            SchoolResultModel model = new SchoolResultModel
            {
                studentScores = GetStudentScore(planId, schoolCode, ref msgSchool),
                schoolScore = new List<SchoolScoreModel>()
            };
            if (model.studentScores.Count > 0)
            {
                var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
                model.IsComputerSpecialty = plan.SpecialtyId == 0;

                string sqlQuery = $"select PeopleCount from V_MonthExamPeopleCount where PlanId={planId}";
                int sCount = _db.QueryBySql<int>(sqlQuery)[0];

                string sqlQuery2 = $"select count(PlanId) PeopleCount from V_CultureExamStudentScore where PlanId={planId} group by PlanId";
                var counts = _db.QueryBySql<int>(sqlQuery2);

                int cCount = (counts == null || counts.Count == 0) ? 0 : counts[0];

                double? effectiveScore = GetEffectiveScore(planId, plan.SpecialtyId);


                model.schoolScore.Add(new SchoolScoreModel
                {
                    SpecialtyName = plan.SpecialtyName,
                    MaxScore = msgSchool.SpecialtyMaxScore,
                    MinScore = msgSchool.SpecialtyMinScore,
                    AvgScore = msgSchool.SpecialtyAvgScore,
                    ProvinceExamCount = sCount,
                    SchoolExamCount = msgSchool.JoinSpecialtyCount,
                    EffectiveScore = effectiveScore
                });

                model.schoolScore.Add(new SchoolScoreModel
                {
                    SpecialtyName = "文化课成绩",
                    SchoolExamCount = msgSchool.JoinCultureCount,
                    AvgScore = msgSchool.CultureAvgScore,
                    EffectiveScore = 0,
                    MaxScore = msgSchool.CultureMaxScore,
                    MinScore = msgSchool.CultureMinScore,
                    ProvinceExamCount = cCount
                });
            }
            return model;
        }

        private double? GetEffectiveScore(int planId, int specialtyId)
        {
            string queryArgs = "cast(b.StudentScore as float) StudentScore";
            string queryWhere = $"a.Id={planId} and b.ExamStatus=2 and a.IsEnd=1 and a.ExamType=6";
            string sql = $@"select * from ( select ROW_NUMBER() OVER(order by StudentScore desc) rownum,* from (
                            select {queryArgs} from ExaminationPlan a
                            left join ExaminationStudentList b on a.Id=b.KsjhId
                            left join [TTLXExamSystem3_UserAdmin].[dbo].[UserTable] c on b.StudentId=c.lexueid
                            where {queryWhere} ) t )tt ";
            if (MonthHelper.Instance._DicSpecialtyScoreLine.ContainsKey(specialtyId.ToString()))
            {
                var line = MonthHelper.Instance._DicSpecialtyScoreLine[specialtyId.ToString()];
                sql += $" where rownum={line.EffectivePeopleNum["2019"]}";
            }
            else
                return null;
            var result = _db.QueryBySql<QueryModel2>(sql);
            if (result.Count > 0)
            {
                return result[0].StudentScore;
            }
            return null;
        }

        public List<SchoolCompareModel> GetSchoolCompareData(int planId, List<string> schoolCodes)
        {
            if (schoolCodes == null || schoolCodes.Count == 0)
            {
                return new List<SchoolCompareModel>();
            }
            int specialtyId = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId).SpecialtyId;

            string total_key = planId + "_school_student";
            List<QueryModel> querys = new List<QueryModel>();
            if (RedisHelper.Instance.IsSet(total_key, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                querys = RedisHelper.Instance.GetModel<List<QueryModel>>(total_key, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }
            else
            {

                string sqlQuery = string.Format("select KsjhName,PaperQueTotalScore,StudentScore,FK_School,FK_SchoolID from {0} where PlanId={1}",
                    specialtyId == 0 ? "V_MonthExamCompare_Computer" : "V_MonthExamCompare",
                    planId);

                querys = _db.QueryBySql<QueryModel>(sqlQuery);
                RedisHelper.Instance.SetModel(total_key, querys, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }

            querys = querys.Where(s => schoolCodes.Contains(s.FK_SchoolID)).ToList();


            List<SchoolCompareModel> models = new List<SchoolCompareModel>();

            ScoreLine line = null;
            string specialtyType = specialtyId.ToString();
            if (MonthHelper.Instance._DicSpecialtyScoreLine.ContainsKey(specialtyType))
            {
                line = MonthHelper.Instance._DicSpecialtyScoreLine[specialtyType];
            }

            foreach (var school in schoolCodes)
            {
                var currSchoolQuery = querys.Where(q => q.FK_SchoolID == school);
                SchoolCompareModel model = new SchoolCompareModel();
                QueryModel query = currSchoolQuery.FirstOrDefault();

                if (query != null)
                {
                    model.SchoolName = query.FK_School;
                    model.JoinExamCount = currSchoolQuery.Count();
                    if (line != null)
                    {
                        double passScore = query.PaperQueTotalScore * line.PassLine;
                        model.PassCount = currSchoolQuery.Count(q => q.StudentScore >= passScore);//及格人数
                        model.PassingRate = Math.Round(model.PassCount / model.JoinExamCount, 2);//及格率
                        model.PassingRate_P = model.PassingRate.ToString("P");
                        double a = query.PaperQueTotalScore * line.A;
                        double b = query.PaperQueTotalScore * line.B;
                        double c = query.PaperQueTotalScore * line.C;
                        model.A = currSchoolQuery.Count(q => q.StudentScore >= a);//优
                        model.B = currSchoolQuery.Count(q => q.StudentScore >= b && q.StudentScore < a);//良
                        model.C = currSchoolQuery.Count(q => q.StudentScore >= c && q.StudentScore < b);//中
                        model.D = currSchoolQuery.Count(q => q.StudentScore < c);//差
                    }
                    model.MaxScore = currSchoolQuery.Max(q => q.StudentScore);//最高分
                    model.MinScore = currSchoolQuery.Min(q => q.StudentScore);//最低分
                    model.AvgScore = Math.Round(currSchoolQuery.Average(q => q.StudentScore), 2);//平均分
                    List<double> scores = currSchoolQuery.Select(q => q.StudentScore).ToList();
                    double standard = 0;
                    foreach (var score in scores)
                    {
                        standard += Math.Pow(score - model.AvgScore, 2.0);
                    }
                    model.StandardDeviation = Math.Round(Math.Sqrt(standard / model.JoinExamCount), 2);//标准差
                    models.Add(model);
                }

            }
            return models;
        }

        public List<QuestionDetailModel> GetPaperQuestionDetail(int planId)
        {

            var details = MonthHelper.Instance.GetPlanQuestionDetail(planId);
            if (details != null)
                return details;

            var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
            List<QuestionQueryModel> result = null;
            string sqlQuery = "";
            if (plan.SpecialtyId == 0)
            {
                sqlQuery = $"select QueNo,QueName,CorrectAnswer from V_MonthExamZuodaQuestion_Computer where PlanId={planId}";

            }
            else
            {
                sqlQuery = $"select QueNo,QueName,CorrectAnswer from V_MonthExamZuodaQuestion where PlanId={planId}";

            }
            result = _db.QueryBySql<QuestionQueryModel>(sqlQuery);
            List<QuestionDetailModel> models = new List<QuestionDetailModel>();

            List<string> answers = _db.QueryBySql<string>($"select AnswerDetail from ExaminationStudentList where KsjhID = {planId}");
            List<ScoreResultDetail> resultList = new List<ScoreResultDetail>();
            foreach (var answer in answers)
            {
                if (!string.IsNullOrEmpty(answer))
                    resultList.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreResultDetail>>(answer));
            }

            foreach (var item in result)
            {
                string planIdString = planId.ToString();

                List<StuAnswerModel> list;

                if (resultList == null || resultList.Count == 0)
                {
                    list = _db.QueryBySql<StuAnswerModel>($@"select StuAnswer,COUNT(StuAnswer) AnswerCount from ScoreResultDetail 
                                                                    where KsjhID = {planId}  and QueNo = '{item.QueNo}'
                                                                    group by StuAnswer
                                                                    order by AnswerCount desc").ToList();
                }
                else
                {
                    list = resultList.Where(r => r.QueNo == item.QueNo).GroupBy(r => r.StuAnswer).Select(r => new StuAnswerModel
                    {
                        StuAnswer = r.Key,
                        AnswerCount = r.Count()
                    }).ToList();
                }
                double totalAnswerCount = list.Sum(s => s.AnswerCount);
                var model = new QuestionDetailModel
                {
                    CorrectAnswer = item.CorrectAnswer,
                };
                if (item.QueName.Length > 30)
                    model.QueName = item.QueName.Substring(0, 30) + "……";
                else
                    model.QueName = item.QueName;
                double answerCount3 = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    answerCount3 += list[i].AnswerCount;
                    if (i == 0)
                    {
                        model.FirstAnswer = list[i].StuAnswer + "(" + (list[i].AnswerCount / totalAnswerCount).ToString("0.##%") + ")";
                    }
                    else if (i == 1)
                    {
                        model.SecondAnswer = list[i].StuAnswer + "(" + (list[i].AnswerCount / totalAnswerCount).ToString("0.##%") + ")";
                    }
                    else if (i == 2)
                    {
                        model.ThirdAnswer = list[i].StuAnswer + "(" + (list[i].AnswerCount / totalAnswerCount).ToString("0.##%") + ")";
                        break;
                    }

                }
                model.OtherAnswer = "其他答案" + "(" + ((totalAnswerCount - answerCount3) / totalAnswerCount).ToString("0.##%") + ")";
                models.Add(model);
            }

            MonthHelper.Instance.SetPlanQuestionDetail(planId, models);
            return models;
        }

        public List<StudentQueDetailModel> GetStuQueDetail(int planId, string studentId, int specialtyId)
        {
            List<StudentQueDetailModel> que_detail = new List<StudentQueDetailModel>();
            string strPlanId = planId.ToString();
            string quetionsKey = strPlanId + "_questions";
            if (RedisHelper.Instance.IsSet(quetionsKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {

                que_detail = RedisHelper.Instance.GetModel<List<StudentQueDetailModel>>(quetionsKey, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }
            else
            {
                que_detail = GetPaperQuestions(planId);
                RedisHelper.Instance.SetModel(quetionsKey, que_detail, RedisIndex.STUDENT_MANAGER_SYSTEM);
            }

            var scores = (from e in _db.Set<ExaminationPlan>()
                          join s in _db.Set<ExaminationStudentList>()
                          on e.Id equals s.KsjhId
                          where s.StudentId == studentId && e.Id == planId
                          select new { s.StudentScore, s.AnswerDetail }).ToList();
            List<ScoreResultDetail> detail;
            if (!string.IsNullOrEmpty(scores[0].AnswerDetail))
            {
                detail = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreResultDetail>>(scores[0].AnswerDetail);
            }
            else
            {
                detail = _db.Set<ScoreResultDetail>().Where(s => s.KsjhID == strPlanId && s.LexueID == studentId).ToList();
            }
            foreach (var item in detail)
            {
                var find = que_detail.Find(d => d.QueNo == item.QueNo);

                if (find != null)
                {
                    find.QueName = find.QueName.TrimEx();
                    find.CorrectAnswer = find.CorrectAnswer.TrimEx();
                    find.StudentAnswer = item.StuAnswer;
                    if (find.StudentAnswer != find.CorrectAnswer)
                    {
                        find.StudentAnswer = "<span style=\"color:red;\">" + find.StudentAnswer + "</span>";
                        find.QueName = "<span style=\"color:red;\">" + find.QueName + "</span>";
                    }
                }
                else
                {

                }
            }
            return que_detail;
        }

        public int GetSpecialtyIdByPlanId(int planId)
        {
            return _db.QuerySingle<ExaminationPlan>(e => e.Id == planId).SpecialtyId;
        }

        public string GetSpecialtyNameByPlanId(int planId)
        {
            return _db.QuerySingle<ExaminationPlan>(e => e.Id == planId).SpecialtyName;
        }

        public string GetRank(int planId, string studentId, string schoolCode)
        {
            SchoolGradeModel model = new SchoolGradeModel();
            var list = GetStudentScore(planId, schoolCode, ref model);
            return list.FirstOrDefault(s => s.Lexueid == studentId).Rank + "/" + list.Count;
        }

        public double GetTotalScore(int planId, string studentId)
        {
            var list = (from a in _db.Set<ExaminationPlan>()
                        join b in _db.Set<ExaminationStudentList>()
                        on a.Id equals b.KsjhId
                        where a.Id == planId && b.StudentId == studentId
                        select b.StudentScore).ToList();
            if (list != null && list.Count > 0)
                return Convert.ToDouble(list[0]);
            return 0;
        }

        public ImportExcelGradeModel GetSchoolExcel(int planId, string schoolCode)
        {
            bool merge = true;//是否合并专业

            ImportExcelGradeModel import = new ImportExcelGradeModel();

            var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
            PlanModel planModel = new PlanModel
            {
                PlanName = plan.KsjhName,
                SpecialtyName = plan.SpecialtyName,
                SchoolName = _db.QueryBySql<string>($"select SchoolName from [TTLXExamSystem3_UserAdmin].[dbo].[Base_School] where SchoolNo='{schoolCode}'").ToList()[0]
            };
            import.PlanModel = planModel;



            GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, planId);
            studentScores = studentScores.ToList();
            studentCultureScores = studentCultureScores.ToList();

            if (merge && MonthHelper.Instance.DicSpecialtyMerge.ContainsKey(planId))
            {
                var planMerge = MonthHelper.Instance.DicSpecialtyMerge[planId];
                planModel.SpecialtyName = planMerge.specialtyName;
                planModel.PlanName = planMerge.planName;

                foreach (var item in planMerge.planIds)
                {
                    if (item != planId)
                    {
                        GetStudentScore(out List<StudentScoreModel> studentScoresTemp, out List<StudentScoreModel> studentCultureScoresTemp, item);
                        studentScores.AddRange(studentScoresTemp);
                        studentCultureScores.AddRange(studentCultureScoresTemp);
                    }
                }
            }



            //全省
            SchoolGradeModel msgProvince = new SchoolGradeModel();
            msgProvince.JoinSpecialtyCount = studentScores.Count;
            if (studentScores != null && studentScores.Count > 0)
            {
                msgProvince.SpecialtyMaxScore = studentScores.Max(s => s.StudentScore);
                msgProvince.SpecialtyMinScore = studentScores.Min(s => s.StudentScore);
                msgProvince.SpecialtyAvgScore = Math.Round(studentScores.Average(s => s.StudentScore), 2);
            }
            msgProvince.JoinCultureCount = studentCultureScores.Count;
            if (studentCultureScores != null && studentCultureScores.Count > 0)
            {
                msgProvince.CultureMaxScore = studentCultureScores.Max(s => s.CultureStudentScore);
                msgProvince.CultureMinScore = studentCultureScores.Min(s => s.CultureStudentScore);
                msgProvince.CultureAvgScore = Math.Round(studentCultureScores.Average(s => s.CultureStudentScore), 2);
                int tempCount = studentCultureScores.Count(s => studentScores.Select(ss => ss.Lexueid).ToList().Contains(s.Lexueid));
                msgProvince.JoinBothCount = msgProvince.JoinCultureCount - tempCount;
            }


            var specialtyStudentScores_school = studentScores.Where(s => s.SchoolCode == schoolCode).ToList();
            var cultureStudentScores_school = studentCultureScores.Where(s => s.SchoolCode == schoolCode).ToList();

            //学校
            SchoolGradeModel msgSchool = new SchoolGradeModel();
            msgSchool.JoinSpecialtyCount = specialtyStudentScores_school.Count;
            if (specialtyStudentScores_school != null && specialtyStudentScores_school.Count > 0)
            {
                msgSchool.SpecialtyMaxScore = specialtyStudentScores_school.Max(s => s.StudentScore);
                msgSchool.SpecialtyMinScore = specialtyStudentScores_school.Min(s => s.StudentScore);
                msgSchool.SpecialtyAvgScore = Math.Round(specialtyStudentScores_school.Average(s => s.StudentScore), 2);
            }
            msgSchool.JoinCultureCount = cultureStudentScores_school.Count;

            List<StudentScoreModel> tempScoreModel = new List<StudentScoreModel>();

            if (cultureStudentScores_school != null && cultureStudentScores_school.Count > 0)
            {
                msgSchool.CultureMaxScore = cultureStudentScores_school.Max(s => s.CultureStudentScore);
                msgSchool.CultureMinScore = cultureStudentScores_school.Min(s => s.CultureStudentScore);
                msgSchool.CultureAvgScore = Math.Round(cultureStudentScores_school.Average(s => s.CultureStudentScore), 2);

                foreach (var item in specialtyStudentScores_school)
                {
                    var student = cultureStudentScores_school.Find(s => s.Lexueid == item.Lexueid);
                    if (student != null)
                    {
                        item.ChineseScore = student.ChineseScore;
                        item.EnglishScore = student.EnglishScore;
                        item.MathScore = student.MathScore;
                        item.CultureStudentScore = student.CultureStudentScore;
                        cultureStudentScores_school.Remove(student);
                    }
                }
                msgSchool.JoinBothCount = msgSchool.JoinCultureCount - cultureStudentScores_school.Count;
                specialtyStudentScores_school.AddRange(cultureStudentScores_school);

            }

            specialtyStudentScores_school = specialtyStudentScores_school.OrderByDescending(s => s.StudentTotalScore).ToList();
            var arrString = new string[]
            {
                ((int)QuestionsTypes.Danxuan).ToString(),
                ((int)QuestionsTypes.Duoxuan).ToString(),
                ((int)QuestionsTypes.Panduan).ToString()
            };
            foreach (var student in specialtyStudentScores_school)
            {
                if (!string.IsNullOrEmpty(student.AnswerDetail))
                {
                    var answerDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreResultDetail>>(student.AnswerDetail);
                    student.SelectScore = answerDetails.Where(s => arrString.Contains(s.QueType)).Sum(s => double.Parse(s.GetScore));
                    student.ProgramScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Biancheng).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.WinScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Windows).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.WordScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Word).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.NetScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Wangluo).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.PptScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.PowerPoint).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.ExcelScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Excel).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.AccessScore = answerDetails.Where(s => s.QueType == ((int)QuestionsTypes.Access).ToString()).Sum(s => double.Parse(s.GetScore));
                    student.AnswerDetail = null;
                }
            }

            import.StudentModels = specialtyStudentScores_school.OrderByDescending(s => s.StudentTotalScore).ToList();
            import.SpecialtySchoolScore = new List<SchoolScoreModel2>();
            import.CultureSchoolScore = new List<SchoolScoreModel2>();

            SchoolScoreModel2 schoolScore = new SchoolScoreModel2
            {
                RegionName = planModel.SchoolName,
                MaxScore = msgSchool.SpecialtyMaxScore,
                MinScore = msgSchool.SpecialtyMinScore,
                AvgScore = msgSchool.SpecialtyAvgScore
            };
            schoolScore.ExamCount = msgSchool.JoinSpecialtyCount;
            schoolScore.EffectiveScore = GetEffectiveScore(planId, plan.SpecialtyId);
            import.SpecialtySchoolScore.Add(schoolScore);

            import.SpecialtySchoolScore.Add(new SchoolScoreModel2
            {
                RegionName = "全省",
                MaxScore = msgProvince.SpecialtyMaxScore,
                MinScore = msgProvince.SpecialtyMinScore,
                AvgScore = msgProvince.SpecialtyAvgScore,
                ExamCount = msgProvince.JoinSpecialtyCount,
                EffectiveScore = schoolScore.EffectiveScore
            });

            import.CultureSchoolScore.Add(new SchoolScoreModel2
            {
                RegionName = planModel.SchoolName,
                MaxScore = msgSchool.CultureMaxScore,
                MinScore = msgSchool.CultureMinScore,
                AvgScore = msgSchool.CultureAvgScore,
                ExamCount = msgSchool.JoinCultureCount,
                EffectiveScore = 0
            });

            import.CultureSchoolScore.Add(new SchoolScoreModel2
            {
                RegionName = "全省",
                MaxScore = msgProvince.CultureMaxScore,
                MinScore = msgProvince.CultureMinScore,
                AvgScore = msgProvince.CultureAvgScore,
                ExamCount = msgProvince.JoinCultureCount,
                EffectiveScore = 0
            });


            return import;
        }

        public string GetPlanNameById(int planId)
        {
            var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
            return plan == null ? "" : plan.KsjhName.Trim('\r', '\n');
        }

        public List<SchoolScoreModel> GetProvinceBaseMessage(int planId)
        {
            SchoolGradeModel msg = new SchoolGradeModel();
            var studentScores = GetStudentScore(planId, "", ref msg);
            var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
            SchoolScoreModel specialtyModel = new SchoolScoreModel
            {
                AvgScore = msg.SpecialtyAvgScore,
                MaxScore = msg.SpecialtyMaxScore,
                MinScore = msg.SpecialtyMinScore,
                ProvinceExamCount = msg.JoinSpecialtyCount,
                EffectiveScore = GetEffectiveScore(planId, plan.SpecialtyId),
                SpecialtyName = plan.SpecialtyName,
            };
            SchoolScoreModel cultureModel = new SchoolScoreModel
            {
                AvgScore = msg.CultureAvgScore,
                MaxScore = msg.CultureMaxScore,
                MinScore = msg.CultureMinScore,
                ProvinceExamCount = msg.JoinCultureCount,
                EffectiveScore = 0,
                SpecialtyName = "文化课成绩",
            };
            return new List<SchoolScoreModel> { specialtyModel, cultureModel };
        }

        public string GetSchoolName(string studentId)
        {
            string sql = $"select b.SchoolName from [TTLXExamSystem3_UserAdmin].[dbo].[UserTable] a left join [TTLXExamSystem3_UserAdmin].[dbo].[Base_School] b on a.FK_SchoolID = b.SchoolNo where a.lexueid = '{studentId}'";
            var schools = _db.QueryBySql<string>(sql);
            if (schools.Count > 0)
                return schools[0];
            return "";
        }

        public ImportProvinceDataModel GetProvinceExcel(int planId)
        {
            bool merge = true;//是否合并专业

            ImportProvinceDataModel import = new ImportProvinceDataModel();

            var plan = _db.QuerySingle<ExaminationPlan>(e => e.Id == planId);
            PlanModel planModel = new PlanModel
            {
                PlanName = plan.KsjhName,
                SpecialtyName = plan.SpecialtyName,
                SchoolName = "湖北省"
            };
            import.PlanModel = planModel;

            //GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, planId);
            //studentScores = studentScores.ConvertAll(s => s.Clone()).ToList();
            //studentCultureScores = studentCultureScores.ConvertAll(s => s.Clone()).ToList();

            GetStudentScore(out List<StudentScoreModel> studentScores, out List<StudentScoreModel> studentCultureScores, planId);
            studentScores = studentScores.ToList();
            studentCultureScores = studentCultureScores.ToList();

            if (merge && MonthHelper.Instance.DicSpecialtyMerge.ContainsKey(planId))
            {
                var planMerge = MonthHelper.Instance.DicSpecialtyMerge[planId];
                planModel.SpecialtyName = planMerge.specialtyName;
                planModel.PlanName = planMerge.planName;

                foreach (var item in planMerge.planIds)
                {
                    if (item != planId)
                    {
                        GetStudentScore(out List<StudentScoreModel> studentScoresTemp, out List<StudentScoreModel> studentCultureScoresTemp, item);
                        studentScores.AddRange(studentScoresTemp);
                        studentCultureScores.AddRange(studentCultureScoresTemp);
                    }
                }
            }


            SchoolGradeModel msgProvince = new SchoolGradeModel();
            msgProvince.JoinSpecialtyCount = studentScores.Count;
            if (studentScores != null && studentScores.Count > 0)
            {
                msgProvince.SpecialtyMaxScore = studentScores.Max(s => s.StudentScore);
                msgProvince.SpecialtyMinScore = studentScores.Min(s => s.StudentScore);
                msgProvince.SpecialtyAvgScore = Math.Round(studentScores.Average(s => s.StudentScore), 2);
            }
            msgProvince.JoinCultureCount = studentCultureScores.Count;
            if (studentCultureScores != null && studentCultureScores.Count > 0)
            {
                msgProvince.CultureMaxScore = studentCultureScores.Max(s => s.CultureStudentScore);
                msgProvince.CultureMinScore = studentCultureScores.Min(s => s.CultureStudentScore);
                msgProvince.CultureAvgScore = Math.Round(studentCultureScores.Average(s => s.CultureStudentScore), 2);
                //int tempCount = studentCultureScores.Count(s => studentScores.Select(ss => ss.Lexueid).ToList().Contains(s.Lexueid));
                //msgProvince.JoinBothCount = msgProvince.JoinCultureCount - tempCount;
                foreach (var item in studentScores)
                {
                    var temp = studentCultureScores.Find(s => s.Lexueid == item.Lexueid);
                    if (temp != null)
                    {
                        item.ChineseScore = temp.ChineseScore;
                        item.EnglishScore = temp.EnglishScore;
                        item.MathScore = temp.MathScore;
                        item.CultureStudentScore = temp.CultureStudentScore;
                        studentCultureScores.Remove(temp);
                    }
                }
                msgProvince.JoinBothCount = msgProvince.JoinCultureCount - studentCultureScores.Count;
                studentScores.AddRange(studentCultureScores);
            }

            import.SpecialtyProvinceScore = new ProvinceScoreModel
            {
                AvgScore = msgProvince.SpecialtyAvgScore,
                ExamCount = msgProvince.JoinSpecialtyCount,
                MaxScore = msgProvince.SpecialtyMaxScore,
                MinScore = msgProvince.SpecialtyMinScore,
                EffectiveScore = GetEffectiveScore(planId, plan.SpecialtyId)
            };

            import.CultureProvinceScore = new ProvinceScoreModel
            {
                AvgScore = msgProvince.CultureAvgScore,
                ExamCount = msgProvince.JoinCultureCount,
                MaxScore = msgProvince.CultureMaxScore,
                MinScore = msgProvince.CultureMinScore
            };

            import.StudentModels = studentScores.OrderByDescending(s => s.StudentTotalScore).ToList();

            return import;

        }

        public List<StudentPaperData> GetStudentMonthExamData(int year)
        {
            var userData = CookieHelper.GetUserData();
            string sqlQuery = "select ExamDate,PlanID,PlanName,ExamPaperID,ExamPaperName,StudentScore from V_MonthExamStudentData " +
                "where Lexueid = @lexueid AND StartTime > @start_year AND StartTime < @end_year order by PlanID desc";

            var list_studentData = _db.QueryBySql<StudentPaperData>(sqlQuery,
                new SqlParameter("@lexueid", userData.Lexueid),
                new SqlParameter("@start_year", year.ToString()),
                new SqlParameter("@end_year", (year + 1).ToString()));


            for (int i = 0; i < list_studentData.Count; i++)
            {
                list_studentData[i].ExamPaperName = list_studentData[i].ExamPaperName.TrimEx();
                list_studentData[i].PlanName = list_studentData[i].PlanName.TrimEx();
            }
            return list_studentData;
        }

        public List<string> GetStudentExamDate(string lexueid)
        {

            List<string> times = null;
            try
            {
                times = (from s in _db.Set<ExaminationStudentList>()
                         join e in _db.Set<ExaminationPlan>()
                         on s.KsjhId equals e.Id
                         where s.StudentId == lexueid
                         select e.StartTime).ToList();

                string sql = $@"select b.StartTime from V_CultureExamStudentScore a
                        left join ExaminationPlan b on a.PlanId = b.Id where a.Lexueid = '{lexueid}' ";
                var temp = _db.QueryBySql<string>(sql);
                if (temp != null)
                {
                    foreach (var t in temp)
                    {
                        if (!times.Contains(t))
                        {
                            times.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }

            int count = times.Count;
            for (int i = 0; i < count;)
            {
                if (DateTime.TryParse(times[i], out DateTime time))
                {
                    string year = time.Year.ToString();
                    if (times.Exists(s => s == year))
                    {
                        times[i] = null;
                    }
                    else
                    {
                        times[i] = year;
                    }
                    i++;
                }
                else
                {
                    times.RemoveAt(i);
                    count = times.Count;
                }
            }
            times.RemoveAll(s => s == null);
            return times;
        }

        public StudentQueDetail GetStudentPaperDetail(int planId)
        {
            var userData = CookieHelper.GetUserData();

            StudentQueDetail stuDetail = new StudentQueDetail();

            int isHasCount = _db.Set<CultureSpecialtyExamRelation>().Count(c => c.SpecialtyExamId == planId);

            bool isCultureScore = false;

            string answerDetail = null;
            if (isHasCount == 0)
            {
                var scores = (from e in _db.Set<ExaminationPlan>()
                              join s in _db.Set<ExaminationStudentList>() on e.Id equals s.KsjhId
                              //join cs in cultureScores on e.Id equals cs.Id
                              where s.StudentId == userData.Lexueid && e.Id == planId
                              select new
                              {
                                  SpecialtyScore = s.StudentScore,
                                  s.AnswerDetail,
                              }).ToList();
                if (scores == null || scores.Count == 0)
                    return null;
                stuDetail.StudentSpecialtyScore = Convert.ToDouble(scores[0].SpecialtyScore);
                answerDetail = scores[0].AnswerDetail;
            }
            else
            {
                var cultureScores = from e in _db.Set<ExaminationPlan>()
                                    join r in _db.Set<CultureSpecialtyExamRelation>() on e.Id equals r.SpecialtyExamId
                                    join c in _db.Set<CulturalExamPlan>() on r.CultureExamId equals c.Id
                                    join cs in _db.Set<CulturalCoursesScore>() on c.Id equals cs.ExamPlanId
                                    where cs.IDCard == userData.IDCard && e.Id == planId
                                    select new { cs, e.Id };

                var scores = (from e in _db.Set<ExaminationPlan>()
                              join s in _db.Set<ExaminationStudentList>() on e.Id equals s.KsjhId
                              join cs in cultureScores on e.Id equals cs.Id
                              where s.StudentId == userData.Lexueid && e.Id == planId
                              select new
                              {
                                  SpecialtyScore = s.StudentScore,
                                  s.AnswerDetail,
                                  cs.cs.ChineseScore,
                                  cs.cs.EnglishScore,
                                  cs.cs.MathScore,
                                  CultureScore = cs.cs.StudentScore
                              }).ToList();
                if (scores == null || scores.Count == 0)
                {
                    string sql = $"select CultureStudentScore,MathScore,EnglishScore,ChineseScore from V_CultureExamStudentScore where PlanId={planId} and Lexueid='{userData.Lexueid}'";
                    var tempResult = _db.QueryBySql<CultureScoreModel>(sql);
                    if (tempResult == null || tempResult.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        isCultureScore = true;
                        stuDetail.ChineseScore = tempResult[0].ChineseScore;
                        stuDetail.EnglishScore = tempResult[0].EnglishScore;
                        stuDetail.MathScore = tempResult[0].MathScore;
                        stuDetail.StudentCultureScore = tempResult[0].CultureStudentScore;
                    }
                }
                else
                {
                    stuDetail.StudentSpecialtyScore = Convert.ToDouble(scores[0].SpecialtyScore);
                    stuDetail.ChineseScore = scores[0].ChineseScore ?? 0;
                    stuDetail.EnglishScore = scores[0].EnglishScore ?? 0;
                    stuDetail.MathScore = scores[0].MathScore ?? 0;
                    stuDetail.StudentCultureScore = scores[0].CultureScore ?? 0;
                    answerDetail = scores[0].AnswerDetail;
                }

            }


            string strPlandId = planId.ToString();
            string avgKey = strPlandId + "_avgScore";
            if (RedisHelper.Instance.IsSet(avgKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                stuDetail.ProvinceAvgScore = Convert.ToDouble(RedisHelper.Instance.StringGet(avgKey, RedisIndex.STUDENT_MANAGER_SYSTEM));
            }
            else
            {
                stuDetail.ProvinceAvgScore = GetProvinceAvgScore(planId);
                RedisHelper.Instance.StringSet(avgKey, stuDetail.ProvinceAvgScore.ToString(), RedisIndex.STUDENT_MANAGER_SYSTEM);
            }

            string maxKey = strPlandId + "_maxScore";
            if (RedisHelper.Instance.IsSet(maxKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                stuDetail.ProvinceMaxScore = Convert.ToDouble(RedisHelper.Instance.StringGet(maxKey, RedisIndex.STUDENT_MANAGER_SYSTEM));
            }
            else
            {
                stuDetail.ProvinceMaxScore = GetProvinceMaxScore(planId);
                RedisHelper.Instance.StringSet(maxKey, stuDetail.ProvinceMaxScore.ToString(), RedisIndex.STUDENT_MANAGER_SYSTEM);
            }

            string minKey = strPlandId + "_minScore";
            if (RedisHelper.Instance.IsSet(minKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
            {
                stuDetail.ProvinceMinScore = Convert.ToDouble(RedisHelper.Instance.StringGet(minKey, RedisIndex.STUDENT_MANAGER_SYSTEM));
            }
            else
            {
                stuDetail.ProvinceMinScore = GetProvinceMinScore(planId);
                RedisHelper.Instance.StringSet(minKey, stuDetail.ProvinceMinScore.ToString(), RedisIndex.STUDENT_MANAGER_SYSTEM);
            }

            stuDetail.QueDetail = new List<StudentQueDetailModel>();
            if (!isCultureScore)
            {
                string quetionsKey = strPlandId + "_questions";
                if (RedisHelper.Instance.IsSet(quetionsKey, RedisIndex.STUDENT_MANAGER_SYSTEM))
                {

                    stuDetail.QueDetail = RedisHelper.Instance.GetModel<List<StudentQueDetailModel>>(quetionsKey, RedisIndex.STUDENT_MANAGER_SYSTEM);
                }
                else
                {
                    stuDetail.QueDetail = GetPaperQuestions(planId);
                    RedisHelper.Instance.SetModel(quetionsKey, stuDetail.QueDetail, RedisIndex.STUDENT_MANAGER_SYSTEM);
                }
            }


            List<ScoreResultDetail> detail;
            if (!string.IsNullOrEmpty(answerDetail))
            {
                detail = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreResultDetail>>(answerDetail);
            }
            else
            {
                detail = _db.Set<ScoreResultDetail>().Where(s => s.KsjhID == strPlandId && s.LexueID == userData.Lexueid).ToList();
            }

            foreach (var item in stuDetail.QueDetail)
            {
                var find = detail.Find(d => d.QueNo == item.QueNo);
                if (find != null)
                {
                    item.QueName = item.QueName.TrimEx();
                    item.CorrectAnswer = item.CorrectAnswer.TrimEx();
                    item.StudentAnswer = find.StuAnswer ?? "";
                    if (item.StudentAnswer != item.CorrectAnswer)
                    {
                        item.StudentAnswer = "<span style=\"color:red;\">" + item.StudentAnswer + "</span>";
                        item.QueName = "<span style=\"color:red;\">" + item.QueName + "</span>";
                    }
                }
                item.StudentAnswer = item.StudentAnswer ?? "";
            }
            return stuDetail;
        }

        private double GetProvinceAvgScore(int planId)
        {
            var avgScores = _db.QueryBySql<double>(@"select AVG(cast(StudentScore as float)) as avgScore from ExaminationStudentList where KsjhId = @planId", new SqlParameter("@planId", planId));
            return Math.Round(avgScores[0], 2);
        }

        private double GetProvinceMaxScore(int planId)
        {
            var maxScores = _db.QueryBySql<double>(@"select MAX(cast(StudentScore as float)) as avgScore from ExaminationStudentList where KsjhId = @planId", new SqlParameter("@planId", planId));
            return Math.Round(maxScores[0], 2);
        }

        private double GetProvinceMinScore(int planId)
        {
            var minScores = _db.QueryBySql<double>(@"select MIN(cast(StudentScore as float)) as avgScore from ExaminationStudentList where KsjhId = @planId", new SqlParameter("@planId", planId));
            return Math.Round(minScores[0], 2);
        }

        private List<StudentQueDetailModel> GetPaperQuestions(int planId)
        {
            var plan = _db.Set<ExaminationPlan>().FirstOrDefault(e => e.Id == planId);
            string strPlanId = plan.ExamPaperId.ToString();
            if (plan.SpecialtyId == 0)
            {

                return (from r in _db.Set<MonthExamTestPaperQuestionRelation_Computer>()
                        join q in _db.Set<Questionsinfo_New_Computer>()
                        on r.QuestionID equals q.No
                        where r.PaperID == strPlanId
                        select new StudentQueDetailModel
                        {
                            QueName = q.Name,
                            QueNo = q.No,
                            CorrectAnswer = q.StandardAnwser
                        }).ToList();
            }
            else
            {
                return (from r in _db.Set<MonthExamTestPaperQuestionRelation>()
                        join q in _db.Set<Questionsinfo_New>()
                        on r.QuestionID equals q.No
                        where r.PaperID == strPlanId
                        select new StudentQueDetailModel
                        {
                            QueName = q.Name,
                            QueNo = q.No,
                            CorrectAnswer = q.StandardAnwser
                        }).ToList();
            }
        }
    }
}
