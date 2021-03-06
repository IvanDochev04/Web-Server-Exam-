using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Git.Data.Models
{
    public class Repository
    {
        public Repository()
        {
            this.Commits = new HashSet<Commit>();
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public virtual ICollection<Commit> Commits { get; set; }
    }
}