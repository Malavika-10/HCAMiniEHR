using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int LabOrderId { get; set; }

        // ✅ Correct validation for dropdown (int)
        [Range(1, int.MaxValue, ErrorMessage = "Appointment is required")]
        public int AppointmentId { get; set; }

        // ✅ Navigation property (THIS FIXES YOUR ERROR)
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } = null!;

        [Required(ErrorMessage = "Test Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$",
            ErrorMessage = "Test Name must contain only alphabets")]
        public string TestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ordered Date is required")]
        public DateTime OrderedDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "Pending";
    }
}
