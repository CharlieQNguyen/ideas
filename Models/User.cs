using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace wedding_planner.Models
{
    public class User
    {
        [Required(ErrorMessage = "this is required")]
        public int userId {get; set;} 
        [Required(ErrorMessage = "this is required")]
        public string name {get; set;} //was first name
        [Required(ErrorMessage = "this is required")]
        public string alias {get; set;} // was last name
        [Required(ErrorMessage = "this is required")]
        public string email {get; set;}
        [Required(ErrorMessage = "this is required")]
        public string password {get; set;}
        public List<Idea> ideas {get; set;}
        public List<Like> likes { get; set; }
        public User()
        {
            ideas = new List<Idea>();
            likes = new List<Like>();
            
        }
        
    }
}