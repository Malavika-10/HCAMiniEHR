using HCAMiniEHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================
        // Form Fields
        // =====================
        [BindProperty]
        public int PatientId { get; set; }

        [BindProperty]
        public int DoctorId { get; set; }

        [BindProperty]
        public DateTime AppointmentDate { get; set; }

        [BindProperty]
        public string Status { get; set; } = "Pending";

        // =====================
        // Dropdown Lists
        // =====================
        public List<SelectListItem> PatientList { get; set; } = new();
        public List<SelectListItem> DoctorList { get; set; } = new();

        // =====================
        // Load dropdown data
        // =====================
        public async Task OnGetAsync()
        {
            PatientList = await _context.Patients
                .Select(p => new SelectListItem
                {
                    Value = p.PatientId.ToString(),
                    Text = p.FullName
                })
                .ToListAsync();

            DoctorList = await _context.Doctors
                .Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }

        // =====================
        // Save Appointment
        // =====================
        public async Task<IActionResult> OnPostAsync()
        {
            if (PatientId == 0 || DoctorId == 0)
            {
                await OnGetAsync();
                ModelState.AddModelError("", "Please select both patient and doctor.");
                return Page();
            }
            if (AppointmentDate < DateTime.Now)
            {
                await OnGetAsync();
                ModelState.AddModelError("AppointmentDate",
                    "Appointment date and time must be in the future.");
                return Page();
            }


            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Healthcare.CreateAppointment @PatientId, @DoctorId, @AppointmentDate, @Status",
                new SqlParameter("@PatientId", PatientId),
                new SqlParameter("@DoctorId", DoctorId),
                new SqlParameter("@AppointmentDate", AppointmentDate),
                new SqlParameter("@Status", Status)
            );

            return RedirectToPage("/Appointments/Index");
        }
    }
}
