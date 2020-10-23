namespace IDAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExercisePaper")]
    public partial class ExercisePaper
    {
        [Key]
        [Column(Order = 0)]
        public int ExamPaperID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QueCount { get; set; }

        [Key]
        [Column(Order = 2)]
        public double TotalScore { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string ExamPaperName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string CreateTime { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DanxuanNum { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DuoxuanNum { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PanduanNum { get; set; }

        [Key]
        [Column(Order = 8)]
        public double DanxuanScore { get; set; }

        [Key]
        [Column(Order = 9)]
        public double DuoxuanScore { get; set; }

        [Key]
        [Column(Order = 10)]
        public double PanduanScore { get; set; }

        public int? Type { get; set; }
    }
}
