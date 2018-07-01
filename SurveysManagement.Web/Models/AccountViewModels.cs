using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveysManagement.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public int? Id;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        //[Display(Name = "Name")]
        public string Name { get; set; }

        public bool? isDeleted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Phone { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string City { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Area { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Street { get; set; }

        public string Role { get; set; }

        //[StringLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Gender { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Education { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Major { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftStart { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftEnd { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
    }

    public class ViewUserInfoViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        public int Id { get; set; }

        [Required]
        //[Display(Name = "Name")]
        public string Name { get; set; }

        public bool? isDeleted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime? CreationDate { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Phone { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string City { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Area { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Street { get; set; }

        //[StringLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Gender { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Education { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Major { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftStart { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftEnd { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftDescription { get; set; }

        public string RoleId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDate { get; set; }
    }
    public class EditUserViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        public int Id { get; set; }

        [Required]
        //[Display(Name = "Name")]
        public string Name { get; set; }

        public bool? isDeleted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime? CreationDate { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Phone { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string City { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Area { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Street { get; set; }

        //[StringLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Gender { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Education { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string Major { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftStart { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftEnd { get; set; }

        //[StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MaxLengthExceeded")]
        public string ShiftDescription { get; set; }

        public string RoleId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDate { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
