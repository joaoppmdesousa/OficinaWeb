﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Mechanic : IEntity
    {
        public int Id { get ; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Specialty { get; set; }

        [Display(Name = "Professional Contact")]
        [Required]
        [Phone]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Enter a valid phone number.")]
        public string ProfessionalContact { get; set; }

        [Required]
        [Display(Name = "Clock In")]
        public TimeSpan ClockIn { get; set; }

        [Required]
        [Display(Name = "Clock Out")]
        public TimeSpan ClockOut { get; set; }

        public bool Active { get; set; }

        public User User { get; set; }
    }
}
