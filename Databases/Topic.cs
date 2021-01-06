using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("TOPIC")]
    public partial class Topic
    {
        public Topic()
        {
            TopicImage = new HashSet<TopicImage>();
            TopicReply = new HashSet<TopicReply>();
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [InverseProperty("Topic")]
        public virtual ICollection<TopicImage> TopicImage { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<TopicReply> TopicReply { get; set; }
    }
}
