using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace bright_idea.Models{
    public class User
    {
        [Key]
        public int Userid {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }
        
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Alias { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<Idea> Ideas {get; set;}
        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [NotMapped]
        [CompareAttribute("Password")]
        public string ConfirmPassword { get; set; }

        public User()
        {
            Ideas = new List<Idea>();

        }
    }
        public class UserLog
        {

        [Required(ErrorMessage = "Email for login doesn't exist")]
        [EmailAddress]
        [Column("email")]
        public string LogEmail { get; set; }
                
        [Required(ErrorMessage = "Password is incorrect")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [Column("password")]
        public string LogPassword { get; set; }
}

    }

    
