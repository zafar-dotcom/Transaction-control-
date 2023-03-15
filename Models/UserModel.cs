using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SendEmailViaSMTP.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "User Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email formate is wrong")]
        [Display(Name = "Email ADdress")]
        public string? Email { get; set; }
        public DateTime Join_date { get; set; }
        public DateTime End_date { get; set; }
        [Range(10, 60, ErrorMessage = "age must be in 20 to 60")]
        public int Age { get; set; }

    }
}
