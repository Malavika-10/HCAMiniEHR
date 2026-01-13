using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ONLY THIS (NO FLAT PROPERTIES)
        [BindProperty]
        public Patient Patient { get; set; } = new Patient();

        public IActionResult OnPost()
        {
            // 🔴 REQUIRED FOR VALIDATION
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ✅ USE MODEL VALUES (VALIDATION APPLIES)
            _context.Database.ExecuteSqlRaw(
                "EXEC Healthcare.CreatePatient @FullName, @Gender, @DateOfBirth, @Phone",
                new SqlParameter("@FullName", Patient.FullName),
                new SqlParameter("@Gender", Patient.Gender),
                new SqlParameter("@DateOfBirth", Patient.DateOfBirth),
                new SqlParameter("@Phone", Patient.Phone)
            );

            return RedirectToPage("/Patients/Index");
        }
    }
}
