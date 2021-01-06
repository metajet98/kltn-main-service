using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("TOPIC_REPLY")]
    public partial class TopicReply
    {
        [Key]
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(TopicId))]
        [InverseProperty("TopicReply")]
        public virtual Topic Topic { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("TopicReply")]
        public virtual User User { get; set; }
    }
}
