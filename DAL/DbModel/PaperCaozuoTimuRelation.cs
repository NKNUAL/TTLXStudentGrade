namespace IDAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaperCaozuoTimuRelation")]
    public partial class PaperCaozuoTimuRelation
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string ExamPaperID { get; set; }

        [StringLength(20)]
        public string CaozuoTimuID { get; set; }

        [StringLength(50)]
        public string Point { get; set; }

        public int? QuestionType { get; set; }
    }
}
