using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        // GET: Load patient
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patient = await _context.Patients.FindAsync(id);

            if (Patient == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: Update patient
        public async Task<IActionResult> OnPostAsync()
        {
            var patientFromDb = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == Patient.PatientId);

            if (patientFromDb == null)
            {
                return NotFound();
            }

            // Update fields manually (SAFE WAY)
            patientFromDb.FullName = Patient.FullName;
            patientFromDb.Gender = Patient.Gender;
            patientFromDb.DateOfBirth = Patient.DateOfBirth;
            patientFromDb.Phone = Patient.Phone;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Patients/Index");
        }
    }
}
