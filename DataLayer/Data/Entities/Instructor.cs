using System;
using System.ComponentModel.DataAnnotations;
using ITOps.Shared;

namespace SchoolData.Data.Entities
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
    }
}