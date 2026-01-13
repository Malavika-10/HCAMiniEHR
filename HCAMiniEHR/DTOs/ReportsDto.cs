using System;
using System.Collections.Generic;

namespace HCAMiniEHR.DTOs
{
    // ✅ ONE DTO FOR ALL REPORTS
    public class ReportsDto
    {
        // 1️⃣ Pending Lab Orders
        public List<PendingLabOrder> PendingLabOrders { get; set; }
            = new();

        // 2️⃣ Patients Without Follow-up
        public List<PatientWithoutFollowUp> PatientsWithoutFollowUp { get; set; }
            = new();

        // 3️⃣ Appointments Per Day
        public List<AppointmentsPerDay> AppointmentsPerDay { get; set; }
            = new();

        // 4️⃣ Doctor Appointment Count
        public List<DoctorAppointmentCount> DoctorAppointmentCounts { get; set; }
            = new();
    }

    // ================= CHILD MODELS =================

    public class PendingLabOrder
    {
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public DateTime OrderedDate { get; set; }
    }

    public class PatientWithoutFollowUp
    {
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class AppointmentsPerDay
    {
        public DateTime Date { get; set; }
        public int AppointmentCount { get; set; }
    }

    public class DoctorAppointmentCount
    {
        public string DoctorName { get; set; } = string.Empty;
        public int AppointmentCount { get; set; }
    }
}
