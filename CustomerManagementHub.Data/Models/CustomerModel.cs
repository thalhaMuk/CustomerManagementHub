using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagementHub.Data.Models
{
    [Table("customers")]
    public class CustomerModel
    {
        [Key]
        [StringLength(24)]
        public string _id { get; set; }

        [Required]
        public int number { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(5)]
        public string EyeColor { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(6)]
        public string Gender { get; set; }

        [Required]
        [StringLength(20)]
        public string Company { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(17)]
        public string Phone { get; set; }

        [Required]
        public int AddressNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressStreet { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressCity { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressState { get; set; }

        [Required]
        public int AddressZipCode { get; set; }

        [Required]
        [StringLength(750)]
        public string About { get; set; }

        [Required]
        [StringLength(27)]
        public string Registered { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags0 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags1 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags2 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags3 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags4 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags5 { get; set; }

        [Required]
        [StringLength(15)]
        public string Tags6 { get; set; }
    }
}
