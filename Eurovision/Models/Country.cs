using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eurovision.Models
{
    public class Country
    {
        [Key]
        public int id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }

        [NotMapped]
        public virtual string image
        {
            get
            {
                return this.Name.Replace(" ", "_");
            }
        }
        [NotMapped]
        public virtual string largeimage
        {
            get
            {
                return string.Format("{0} lrg",this.Name.Replace(" ", "_"));
            } 
        }
    }
}