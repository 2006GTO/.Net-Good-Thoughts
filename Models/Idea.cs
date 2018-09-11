using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace bright_idea.Models{

    public class Idea
    {
        [Key]
        public int Ideaid {get;set;}

        [Required]
        [MaxLength(255)]
        public string Text {get;set;}

        public int Userid { get; set; }
        public User Creator {get; set;} 

        public List<Like> Likes {get; set;}

        public Idea()
        {
            List<Like> Likes = new List<Like>();
        }
    }
}