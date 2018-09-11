
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace bright_idea.Models
{
    public class ViewModel
    {
        public Idea Idea {get; set;}
        public Like Like { get; set;}
        public List<User> allUsers {get; set;}
        public List<Idea> allIdeas {get; set;}
        public List<Like> allLikes {get; set;}
        public User regUser {get; set;}
        public UserLog loginUser {get; set;}
    }
}