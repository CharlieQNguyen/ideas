using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models
{
    public class Like
    {
        [Key]
        public int idlikes { get; set; }
        [ForeignKey("user")]
        public int userId {get; set;}
        public User userObject {get; set;}
        [ForeignKey("idea")]
        public int ideasId {get; set;}
        public Idea ideaObject {get; set;}
    }
}