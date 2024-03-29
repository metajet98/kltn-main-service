﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("BRANCH")]
    public partial class Branch
    {
        public Branch()
        {
            BranchServicePrice = new HashSet<BranchServicePrice>();
            CustomerCalender = new HashSet<CustomerCalender>();
            Maintenance = new HashSet<Maintenance>();
            User = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        public string? Logo { get; set; }

        [InverseProperty("Branch")]
        public virtual ICollection<BranchServicePrice> BranchServicePrice { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<CustomerCalender> CustomerCalender { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<Maintenance> Maintenance { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<User> User { get; set; }
    }
}
