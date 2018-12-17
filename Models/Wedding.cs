using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LogReg.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId{get;set;}

        
        [Required]
        [Display(Name = "Wedder One")]  
        public string Him {get; set;}


        [Required]
        [Display(Name = "Wedder Two")]  
        public string Her {get; set;}
        
        
        [Required]
        public DateTime Date {get;set;}
        
        
        [Required]
        public string Address {get;set;}
        public int UserId {get;set;}

        public DateTime CreatedAt {get;set;}= DateTime.Now;
        public DateTime UpdatedAt {get;set;}= DateTime.Now;
        public List<Attendance> Attendees {get;set;}

    }
}