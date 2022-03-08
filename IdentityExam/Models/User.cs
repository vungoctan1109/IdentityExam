using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityExam.Models
{
    public class User : IdentityUser
    {
        public string IdentityNumber { get; set; }
        public int Status { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}