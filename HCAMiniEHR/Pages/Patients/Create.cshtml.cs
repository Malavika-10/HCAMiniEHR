using HCAMiniEHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        public IActionResult OnPost()
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC Healthcare.CreatePatient @FullName, @Gender, @DateOfBirth, @Phone",
                new SqlParameter("@FullName", FullName),
                new SqlParameter("@Gender", Gender),
                new SqlParameter("@DateOfBirth", DateOfBirth),
                new SqlParameter("@Phone", Phone)
            );

            return RedirectToPage("/Patients/Index");
        }
    }
}
