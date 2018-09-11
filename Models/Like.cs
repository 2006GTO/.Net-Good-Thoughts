using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace bright_idea.Models{
    public class Like
    {
        [Key]
        public int Likeid {get;set;}
        
        public int Userid {get; set;}
        public User User {get; set;}

        public int Ideaid {get; set;}
        public Idea Idea { get; set; }
    
    }
}