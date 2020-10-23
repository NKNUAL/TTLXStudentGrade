namespace IDAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorQuestionsModifyRecord")]
    public partial class ErrorQuestionsModifyRecord
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string QuestionId { get; set; }

        [StringLength(50)]
        public string ModifyDate { get; set; }

        [StringLength(100)]
        public string ModifyUserId { get; set; }

        [StringLength(4000)]
        public string ModifyDesc { get; set; }
    }
}
