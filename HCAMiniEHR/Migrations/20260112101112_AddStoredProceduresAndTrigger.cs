using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCAMiniEHR.Migrations
{
    public partial class AddStoredProceduresAndTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================
            // CREATE SCHEMA
            // =========================
            migrationBuilder.EnsureSchema(
                name: "Healthcare");

            // =========================
            // CREATE TABLE: AuditLogs
            // =========================
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "Healthcare",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                });

            // =========================
            // CREATE TABLE: Patients
            // =========================
            migrationBuilder.CreateTable(
                name: "Patients",
                schema: "Healthcare",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            // =========================
            // CREATE TABLE: Doctors
            // =========================
            migrationBuilder.CreateTable(
                name: "Doctors",
                schema: "Healthcare",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                });

            // =========================
            // CREATE TABLE: Appointments
            // =========================
            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "Healthcare",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "Healthcare",
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Healthcare",
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                });

            // =========================
            // CREATE TABLE: LabOrders
            // =========================
            migrationBuilder.CreateTable(
                name: "LabOrders",
                schema: "Healthcare",
                columns: table => new
                {
                    LabOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrders", x => x.LabOrderId);
                    table.ForeignKey(
                        name: "FK_LabOrders_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "Healthcare",
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                schema: "Healthcare",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                schema: "Healthcare",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_AppointmentId",
                schema: "Healthcare",
                table: "LabOrders",
                column: "AppointmentId");

            // =========================
            // STORED PROCEDURE: Patient
            // =========================
            migrationBuilder.Sql(@"
            CREATE PROCEDURE Healthcare.CreatePatient
                @FullName NVARCHAR(100),
                @Gender NVARCHAR(20),
                @DateOfBirth DATETIME,
                @Phone NVARCHAR(15)
            AS
            BEGIN
                INSERT INTO Healthcare.Patients
                (FullName, Gender, DateOfBirth, Phone)
                VALUES
                (@FullName, @Gender, @DateOfBirth, @Phone);
            END
            ");

            // =========================
            // STORED PROCEDURE: Appointment (DoctorId based)
            // =========================
            migrationBuilder.Sql(@"
            CREATE PROCEDURE Healthcare.CreateAppointment
                @PatientId INT,
                @DoctorId INT,
                @AppointmentDate DATETIME,
                @Status NVARCHAR(50)
            AS
            BEGIN
                INSERT INTO Healthcare.Appointments
                (PatientId, DoctorId, AppointmentDate, Status)
                VALUES
                (@PatientId, @DoctorId, @AppointmentDate, @Status);
            END
            ");

            // =========================
            // STORED PROCEDURE: LabOrder
            // =========================
            migrationBuilder.Sql(@"
            CREATE PROCEDURE Healthcare.CreateLabOrder
                @AppointmentId INT,
                @TestName NVARCHAR(100),
                @OrderedDate DATETIME,
                @Status NVARCHAR(50)
            AS
            BEGIN
                INSERT INTO Healthcare.LabOrders
                (AppointmentId, TestName, OrderedDate, Status)
                VALUES
                (@AppointmentId, @TestName, @OrderedDate, @Status);
            END
            ");

            // =========================
            // TRIGGER: Appointment Audit
            // =========================
            migrationBuilder.Sql(@"
            CREATE TRIGGER Healthcare.trg_AppointmentAudit
            ON Healthcare.Appointments
            AFTER INSERT, UPDATE, DELETE
            AS
            BEGIN
                INSERT INTO Healthcare.AuditLogs
                (TableName, Operation, RecordId, ChangedOn, ChangedBy)
                SELECT
                    'Appointment',
                    CASE
                        WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN 'UPDATE'
                        WHEN EXISTS (SELECT 1 FROM inserted) THEN 'INSERT'
                        ELSE 'DELETE'
                    END,
                    CAST(ISNULL(i.AppointmentId, d.AppointmentId) AS NVARCHAR),
                    GETDATE(),
                    SYSTEM_USER
                FROM inserted i
                FULL JOIN deleted d
                ON i.AppointmentId = d.AppointmentId;
            END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS Healthcare.trg_AppointmentAudit");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Healthcare.CreateLabOrder");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Healthcare.CreateAppointment");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS Healthcare.CreatePatient");

            migrationBuilder.DropTable(name: "LabOrders", schema: "Healthcare");
            migrationBuilder.DropTable(name: "Appointments", schema: "Healthcare");
            migrationBuilder.DropTable(name: "Doctors", schema: "Healthcare");
            migrationBuilder.DropTable(name: "Patients", schema: "Healthcare");
            migrationBuilder.DropTable(name: "AuditLogs", schema: "Healthcare");
        }
    }
}
