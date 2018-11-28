using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models
{
    public class Idea
    {   
        [Key]
        public int ideasId { get; set; }
        [Required(ErrorMessage = "this is required")]
        public string post {get; set;}
        public int like {get; set;}
        [ForeignKey("User")]
        public int userId {get; set;}
        public User user {get; set;}

        public List<Like> likes { get; set; }
        public Idea()
        {
            likes = new List<Like>();
        }

    }
}