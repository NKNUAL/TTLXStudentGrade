namespace IDAL.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SchoolPhoneUserLimit")]
    public partial class SchoolPhoneUserLimit
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string SchoolNo { get; set; }

        [StringLength(4)]
        public string SpecialtyId { get; set; }

        public int? LimitCount { get; set; }
    }
}
