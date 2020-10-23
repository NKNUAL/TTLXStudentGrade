namespace TTLXStudentGrade.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExamPaper")]
    public partial class ExamPaper
    {
        [Key]
        [Column(Order = 0)]
        public int ExamPaperID { get; set; }

        public int? PaperQueCount { get; set; }

        public double? PaperQueTotalScore { get; set; }

        public int? PaperType { get; set; }

        [StringLength(200)]
        public string ExamPaperName { get; set; }

        [StringLength(100)]
        public string ExamPaperCreateTime { get; set; }

        public double? danxuanNum { get; set; }

        public double? duoxuanNum { get; set; }

        public double? panduanNum { get; set; }

        public double? danxuanScore { get; set; }

        public double? duoxuanScore { get; set; }

        public double? panduanScore { get; set; }

        [StringLength(100)]
        public string CreateUserID { get; set; }

        [StringLength(100)]
        public string CreateUserName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FK_Specialty { get; set; }

        public int? isDelete { get; set; }
    }
}
