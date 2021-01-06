using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("TOPIC_IMAGE")]
    public partial class TopicImage
    {
        [Key]
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Required]
        public string Image { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(TopicId))]
        [InverseProperty("TopicImage")]
        public virtual Topic Topic { get; set; }
    }
}
