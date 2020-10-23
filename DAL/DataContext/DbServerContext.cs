using Application.Common;
using IDAL.ServerModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IDAL.DataContext
{
    public class DbServerContext : DbContext
    {

        public DbServerContext()
            : base()
        {
            Database.Connection.ConnectionString = ConfigTools.GetDBConnString("dbConn");
            Database.CommandTimeout = 100;
        }
        public virtual DbSet<UserTable> UserTable { get; set; }
        public virtual DbSet<MockTestPaperScoreResult> MockTestPaperScoreResult { get; set; }
        public virtual DbSet<MockTestPaper> MockTestPaper { get; set; }
        public virtual DbSet<MockTestPaper_Computer> MockTestPaper_Computer { get; set; }
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
        public virtual DbSet<CulturalCoursesScore> CulturalCoursesScore { get; set; }
        public virtual DbSet<CulturalExamPlan> CulturalExamPlan { get; set; }
        public virtual DbSet<CultureSpecialtyExamRelation> CultureSpecialtyExamRelation { get; set; }
    }
}