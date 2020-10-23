namespace IDAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysRole")]
    public partial class SysRole
    {
        [Key]
        public int RID { get; set; }

        [StringLength(100)]
        public string RNAME { get; set; }

        public int? ROID { get; set; }
    }
}
