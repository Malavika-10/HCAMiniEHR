using HCAMiniEHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        [BindProperty]
        public int AppointmentId { get; set; }

        [BindProperty]
        public string? TestName { get; set; }

        [BindProperty]
        public DateTime OrderedDate { get; set; }

        [BindProperty]
        public string? Status { get; set; }

        public List<SelectListItem> AppointmentList { get; set; } = new();

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

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Healthcare.CreateLabOrder @AppointmentId, @TestName, @OrderedDate, @Status",
                new SqlParameter("@AppointmentId", AppointmentId),
                new SqlParameter("@TestName", TestName ?? ""),
                new SqlParameter("@OrderedDate", OrderedDate),
                new SqlParameter("@Status", Status ?? "Pending")
            );

            return RedirectToPage("/LabOrders/Index");
        }
    }
}
