using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EAP_C2108G1_ABC.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [Required]
        [Range(8, 20)]
        public string? FullName { get; set; }
        [Required]
        public String? Birthday { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
        [Required]
        [ForeignKey(nameof(Class.ClassId))]
        public int ClassId { get; set; }
        public virtual Class? ClassEntity { get; set; }
    }
}
