using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("NEWS")]
    public partial class News
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Thumbnail { get; set; }
        [Required]
        public string Title { get; set; }
        public string Url { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
