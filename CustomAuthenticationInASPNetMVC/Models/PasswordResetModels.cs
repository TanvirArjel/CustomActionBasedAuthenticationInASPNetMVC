using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CustomAuthenticationInASPNetMVC.Models
{
    public class UserPasswordReset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PasswordResetId { get; set; }
        public int UserId { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsReset { get; set; }
    }
}