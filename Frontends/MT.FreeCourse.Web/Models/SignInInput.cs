﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Models
{
    public class SignInInput
    {
        [Display(Name="Your Email Address")]
        public string Email { get; set; }
        [Display(Name = "Your Password")]
        public string Password  { get; set; }
        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; }
    }
}
