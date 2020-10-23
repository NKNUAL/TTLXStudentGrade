using Application.Common;
using IDAL.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DataContext
{
    public class DbUseContext : DbContext
    {
        public DbUseContext()
            : base()
        {
            Database.Connection.ConnectionString = ConfigTools.GetDBConnString("dbUser");
            Database.CommandTimeout = 100;
        }
        public virtual DbSet<Base_Area> Base_Area { get; set; }
        public virtual DbSet<UserTable> UserTable { get; set; }
        public virtual DbSet<Base_School> Base_School { get; set; }
        public virtual DbSet<Base_specialtyType> Base_specialtyType { get; set; }
        public virtual DbSet<MockTestPaperScoreResult> MockTestPaperScoreResult { get; set; }
        public virtual DbSet<MockTestPaper> MockTestPaper { get; set; }
        public virtual DbSet<MockTestPaper_Computer> MockTestPaper_Computer { get; set; }
        public virtual DbSet<LogDetails> LogDetails { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<MockTestPaperQuestionRelation> MockTestPaperQuestionRelation { get; set; }
        public virtual DbSet<MockTestPaperQuestionRelation_Computer> MockTestPaperQuestionRelation_Computer { get; set; }
        public virtual DbSet<MonthExamTestPaper> MonthExamTestPaper { get; set; }
        public virtual DbSet<MonthExamTestPaper_Computer> MonthExamTestPaper_Computer { get; set; }
        public virtual DbSet<MonthExamTestPaperQuestionRelation> MonthExamTestPaperQuestionRelation { get; set; }
        public virtual DbSet<MonthExamTestPaperQuestionRelation_Computer> MonthExamTestPaperQuestionRelation_Computer { get; set; }
        public virtual DbSet<MonthExamTestPaperScoreResult> MonthExamTestPaperScoreResult { get; set; }
        public virtual DbSet<Questionsinfo_New> Questionsinfo_New { get; set; }
        public virtual DbSet<Questionsinfo_New_Computer> Questionsinfo_New_Computer { get; set; }
        public virtual DbSet<ExaminationPlan> ExaminationPlan { get; set; }
        public virtual DbSet<ExaminationStudentList> ExaminationStudentList { get; set; }
        public virtual DbSet<ScoreResultDetail> ScoreResultDetail { get; set; }
        public virtual DbSet<ExamPaper> ExamPaper { get; set; }
        public virtual DbSet<ExamPaper_Computer> ExamPaper_Computer { get; set; }
        public virtual DbSet<ExamPaperQuestionRelation> ExamPaperQuestionRelation { get; set; }
        public virtual DbSet<ExamPaperQuestionRelation_Computer> ExamPaperQuestionRelation_Computer { get; set; }
        public virtual DbSet<LexueidRelationIDCard> LexueidRelationIDCard { get; set; }
        public virtual DbSet<SchoolPhoneUserLimit> SchoolPhoneUserLimit { get; set; }
        public virtual DbSet<UserTable_SchoolUpload> UserTable_SchoolUpload { get; set; }
    }
}
