using System.ComponentModel.DataAnnotations;

namespace ExamOne.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId {get; set;}
        public int UserId {get; set;}
        public int VentureId {get; set;}
        public User Guest {get; set;}
        public Venture Attending {get; set;}
    }
}