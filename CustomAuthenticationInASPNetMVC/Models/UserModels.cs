using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomAuthenticationInASPNetMVC.Models
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
            ActionCategories = new HashSet<ActionCategory>();
            ControllerActions = new HashSet<ControllerAction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(50)]
        [Index("Ix_UserNameUnique", IsUnique = true)]
        [Remote("IsUserNameAlreadyExists","Users", AdditionalFields = "UserId",ErrorMessage = "UserName Already Taken")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<ActionCategory> ActionCategories { get; set; }
        public virtual ICollection<ControllerAction> ControllerActions { get; set; }
    }

    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
            ActionCategories = new HashSet<ActionCategory>();
            ControllerActions = new HashSet<ControllerAction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        [StringLength(50)]
        [Index("Ix_RoleNameUnique", IsUnique = true)]
        public string RoleName { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ActionCategory> ActionCategories { get; set; }
        public virtual ICollection<ControllerAction> ControllerActions { get; set; }
    }

    public class ActionCategory
    {
        public ActionCategory()
        {
            Users = new HashSet<User>();
            Roles = new HashSet<Role>();
            ControllerActions = new List<ControllerAction>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionCategoryId { get; set; }

        [Required]
        [Display(Name = "Action Cateogy Name")]
        [StringLength(100)]
        [Index("Ix_ActionCategoryNameUnique", IsUnique = true)]
        public string ActionCategoryName { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<ControllerAction> ControllerActions { get; set; }
    }

    [Table("Action")]
    public class ControllerAction
    {

        public ControllerAction()
        {
            Users = new HashSet<User>();
            Roles = new HashSet<Role>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        [Required]
        [ForeignKey("ActionCategory")]
        public int ActionCategoryId { get; set; }

        [Required]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ActionCategory ActionCategory { get; set; }
    }
}