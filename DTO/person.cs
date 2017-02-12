namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("person")]
    public partial class Person : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string name { get; set; }

        [Required]
        [StringLength(1)]
        public string sex { get; set; }

        public bool developer { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        public int? id_job { get; set; }

        public virtual Job job { get; set; }
    }
}
