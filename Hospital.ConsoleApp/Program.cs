using Hospital.Application;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Hospital.Infrastructure.Logging;
using Hospital.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace Hospital.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IRepository<Doctor>, DoctorRepositoryEF>();
            services.AddScoped<IRepository<Patient>, PatientRepositoryEF>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();

            var serviceProvider = services.BuildServiceProvider();

            var doctorService = serviceProvider.GetRequiredService<IDoctorService>();
            var patientService = serviceProvider.GetRequiredService<IPatientService>();


            while (true)
            {
                Console.WriteLine("\n===== HOSPITAL MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add Doctor");
                Console.WriteLine("2. List Doctors");
                Console.WriteLine("3. Delete Doctor");
                Console.WriteLine("4. Update Doctor");
                Console.WriteLine("5. Add Patient");
                Console.WriteLine("6. List Patients");
                Console.WriteLine("7. Delete Patient");
                Console.WriteLine("8. Update Patient");
                Console.WriteLine("9. Find Patient By Name");
                Console.WriteLine("10. Patients By Doctor");
                Console.WriteLine("11. Search Patients (Advanced)");
                Console.WriteLine("12. Paged Patients");
                Console.WriteLine("0. Exit");

                Console.Write("Enter choice: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Doctor Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Specialization: ");
                            string spec = Console.ReadLine();

                            Console.Write("Consultation Fee: ");
                            decimal fee = Convert.ToDecimal(Console.ReadLine());

                            doctorService.AddDoctor(new Doctor
                            {
                                Name = name,
                                Specialization = spec,
                                ConsultationFee = fee
                            });

                            Console.WriteLine("Doctor added successfully.");
                            break;


                        case "2":
                            var doctors = doctorService.GetAllDoctors();

                            foreach (var d in doctors)
                            {
                                Console.WriteLine($"{d.DoctorId} | {d.Name} | {d.Specialization} | ₹{d.ConsultationFee:F2}");
                            }
                            break;


                        case "3":
                            Console.Write("Doctor ID to delete: ");
                            int deleteDoc = Convert.ToInt32(Console.ReadLine());

                            doctorService.DeleteDoctor(deleteDoc);
                            Console.WriteLine("Doctor deleted.");
                            break;


                        case "4":
                            Console.Write("Doctor ID to update: ");
                            int updateId = Convert.ToInt32(Console.ReadLine());

                            var doctor = doctorService.GetDoctorById(updateId);

                            Console.Write($"New Name ({doctor.Name}): ");
                            string newName = Console.ReadLine();

                            Console.Write($"New Specialization ({doctor.Specialization}): ");
                            string newSpec = Console.ReadLine();

                            Console.Write($"New Fee ({doctor.ConsultationFee}): ");
                            string feeInput = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(newName))
                                doctor.Name = newName;

                            if (!string.IsNullOrWhiteSpace(newSpec))
                                doctor.Specialization = newSpec;

                            if (!string.IsNullOrWhiteSpace(feeInput))
                                doctor.ConsultationFee = Convert.ToDecimal(feeInput);

                            doctorService.UpdateDoctor(doctor);

                            Console.WriteLine("Doctor updated.");
                            break;


                        case "5":
                            Console.Write("Patient Name: ");
                            string pname = Console.ReadLine();

                            Console.Write("Age: ");
                            int age = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Condition: ");
                            string condition = Console.ReadLine();

                            Console.Write("Appointment Date (yyyy-mm-dd): ");
                            DateTime date = DateTime.Parse(Console.ReadLine());

                            Console.Write("Doctor ID: ");
                            int doctorId = Convert.ToInt32(Console.ReadLine());

                            patientService.AddPatient(new Patient
                            {
                                Name = pname,
                                Age = age,
                                Condition = condition,
                                AppointmentDate = date,
                                DoctorId = doctorId
                            });

                            Console.WriteLine("Patient added.");
                            break;


                        case "6":
                            var patients = patientService.GetAllPatients();

                            foreach (var p in patients)
                            {
                                Console.WriteLine($"{p.PatientId} | {p.Name} | {p.Age} | {p.Condition} | {p.AppointmentDate:yyyy-MM-dd} | Doctor:{p.DoctorId}");
                            }
                            break;


                        case "7":
                            Console.Write("Patient ID to delete: ");
                            int deletePatient = Convert.ToInt32(Console.ReadLine());

                            patientService.DeletePatient(deletePatient);

                            Console.WriteLine("Patient deleted.");
                            break;


                        case "8":
                            Console.Write("Patient ID to update: ");
                            int pid = Convert.ToInt32(Console.ReadLine());

                            var patient = patientService.GetPatientById(pid);

                            Console.Write($"Name ({patient.Name}): ");
                            string pnameNew = Console.ReadLine();

                            Console.Write($"Age ({patient.Age}): ");
                            string ageInput = Console.ReadLine();

                            Console.Write($"Condition ({patient.Condition}): ");
                            string condInput = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(pnameNew))
                                patient.Name = pnameNew;

                            if (!string.IsNullOrWhiteSpace(ageInput))
                                patient.Age = Convert.ToInt32(ageInput);

                            if (!string.IsNullOrWhiteSpace(condInput))
                                patient.Condition = condInput;

                            patientService.UpdatePatient(patient);

                            Console.WriteLine("Patient updated.");
                            break;


                        case "9":
                            Console.Write("Patient name to search: ");
                            string searchName = Console.ReadLine();

                            var found = patientService.FindPatientByName(searchName);

                            Console.WriteLine($"{found.PatientId} | {found.Name} | {found.Age}");
                            break;


                        case "10":
                            Console.Write("Doctor ID: ");
                            int did = Convert.ToInt32(Console.ReadLine());

                            var doctorPatients = patientService.GetPatientsByDoctor(did);

                            foreach (var p in doctorPatients)
                            {
                                Console.WriteLine($"{p.PatientId} | {p.Name} | {p.Condition}");
                            }
                            break;


                        case "11":
                            Console.Write("Name: ");
                            string n = Console.ReadLine();

                            Console.Write("Min Age: ");
                            int min = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Max Age: ");
                            int max = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Condition: ");
                            string cond = Console.ReadLine();

                            Console.Write("Doctor ID (optional): ");
                            string docInput = Console.ReadLine();

                            int? docId = string.IsNullOrWhiteSpace(docInput) ? null : Convert.ToInt32(docInput);

                            var result = patientService.SearchPatients(n, min, max, cond, docId);

                            foreach (var p in result)
                            {
                                Console.WriteLine($"{p.PatientId} | {p.Name} | {p.Age}");
                            }

                            break;


                        case "12":
                            Console.Write("Page number: ");
                            int page = Convert.ToInt32(Console.ReadLine());

                            var paged = patientService.GetPagedPatients(page);

                            foreach (var p in paged)
                            {
                                Console.WriteLine($"{p.PatientId} | {p.Name}");
                            }

                            break;


                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}