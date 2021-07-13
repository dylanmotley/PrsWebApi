using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi.Models {
    public class User {

        public int Id { get; set; }
        [StringLength(30), Required]
        public string Username { get; set; }
        [StringLength(30), Required]
        public string Password { get; set; }
        [StringLength(30), Required]
        public string Firstname { get; set; }
        [StringLength(30), Required]
        public string Lastname { get; set; }
        [StringLength(30), Required]
        public string Phone { get; set; }
        [StringLength(30), Required]
        public string Email { get; set; }
        public bool Reviewer { get; set; }
        public bool Admin { get; set; }
    }
}
