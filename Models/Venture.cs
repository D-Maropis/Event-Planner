using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamOne.Models
{
    public class Venture
    {
        [Key]
        public int VentureId {get; set;}

        [Required(ErrorMessage="You must have a Title")]
        [Display(Name="Title: ")]
        public string Title {get; set;}

        [Required(ErrorMessage="Activity Date is required.")]
        [FutureDate]
        public DateTime Date {get; set;}

        [Required(ErrorMessage="Duration is required.")]
        [Range(1,1000, ErrorMessage="Duration has to be a postive amount of time.")]
        public int Duration {get; set;}

        [Required(ErrorMessage="Description is required.")]
        [Display(Name="Description: ")]
        public string Description {get; set;}

        /////////////one to many a venture can only have one planner

        public int UserId {get; set;}
        public List<Rsvp> GuestList {get; set;}
        public User Planner {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;

        public DateTime UpdatedAt {get; set;} = DateTime.Now;




        ////////////////////////////

        public class FutureDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime check;
                if(value is DateTime)
                {
                    check = (DateTime)value;
    
                    if(check < DateTime.Now)
                    {
                        return new ValidationResult("Activity must be in the future.");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    return new ValidationResult("Enter a real date.");
                }
            }
        }
    }
}