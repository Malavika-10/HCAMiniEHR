using HCAMiniEHR.Data;
using HCAMiniEHR.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Reports
{
    public class PendingLabsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PendingLabsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ReportsDto Report { get; set; } = new ReportsDto();

        public async Task OnGetAsync()
        {
            // 1️⃣ Pending Lab Orders
            Report.PendingLabOrders = await _context.LabOrders
                .Include(l => l.Appointment).ThenInclude(a => a.Patient)
                .Include(l => l.Appointment).ThenInclude(a => a.Doctor)
                .Where(l => l.Status == "Pending")
                .Select(l => new PendingLabOrder
                {
                    PatientName = l.Appointment.Patient.FullName,
                    DoctorName = l.Appointment.Doctor.Name,
                    TestName = l.TestName,
                    OrderedDate = l.OrderedDate
                })
                .ToListAsync();

            // 2️⃣ Patients Without Follow-up
            Report.PatientsWithoutFollowUp = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status != "Completed")
                .Select(a => new PatientWithoutFollowUp
                {
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.Name,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .ToListAsync();

            // 3️⃣ Appointments Per Day
            Report.AppointmentsPerDay = await _context.Appointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new AppointmentsPerDay
                {
                    Date = g.Key,
                    AppointmentCount = g.Count()
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            // 4️⃣ Doctor Appointment Count
            Report.DoctorAppointmentCounts = await _context.Appointments
                .Include(a => a.Doctor)
                .GroupBy(a => a.Doctor.Name)
                .Select(g => new DoctorAppointmentCount
                {
                    DoctorName = g.Key,
                    AppointmentCount = g.Count()
                })
                .OrderByDescending(x => x.AppointmentCount)
                .ToListAsync();
        }
    }
}
