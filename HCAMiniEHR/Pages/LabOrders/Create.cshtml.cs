using HCAMiniEHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Bound fields
        [BindProperty]
        public int AppointmentId { get; set; }

        [BindProperty]
        public string TestName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime OrderedDate { get; set; }

        [BindProperty]
        public string Status { get; set; } = "Pending";

        public List<SelectListItem> AppointmentList { get; set; } = new();

        // ================= GET =================
        public async Task OnGetAsync()
        {
            AppointmentList = await _context.Appointments
                .Include(a => a.Patient)
                .Select(a => new SelectListItem
                {
                    Value = a.AppointmentId.ToString(),
                    Text = $"{a.Patient.FullName} - {a.AppointmentDate:dd-MM-yyyy}"
                })
                .ToListAsync();
        }

        // ================= POST =================
        public async Task<IActionResult> OnPostAsync()
        {
            // 🔴 Appointment required
            if (AppointmentId <= 0)
            {
                ModelState.AddModelError("AppointmentId", "Appointment is required");
            }

            // 🔴 Fetch appointment date
            var appointmentDate = await _context.Appointments
                .Where(a => a.AppointmentId == AppointmentId)
                .Select(a => a.AppointmentDate)
                .FirstOrDefaultAsync();

            // 🔴 Business rule: LabOrder date >= Appointment date
            if (OrderedDate < appointmentDate.Date)
            {
                ModelState.AddModelError(
                    "OrderedDate",
                    "Lab Order date cannot be before the appointment date"
                );
            }

            // 🔴 Stop if validation fails
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // reload dropdown
                return Page();
            }

            // ✅ Save using stored procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Healthcare.CreateLabOrder @AppointmentId, @TestName, @OrderedDate, @Status",
                new SqlParameter("@AppointmentId", AppointmentId),
                new SqlParameter("@TestName", TestName),
                new SqlParameter("@OrderedDate", OrderedDate),
                new SqlParameter("@Status", Status)
            );

            return RedirectToPage("/LabOrders/Index");
        }
    }
}
