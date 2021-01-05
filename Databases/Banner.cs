using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BANNER")]
    public partial class Banner
    {
        public int Id { get; set; }
        [Required]
        public string Activity { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public int? Priority { get; set; }
        [Required]
        public string Image { get; set; }
        public bool Active { get; set; }
    }
}
