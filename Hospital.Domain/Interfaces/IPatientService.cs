using System;
using System.Collections.Generic;
using System.Text;
using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces
{
    public interface IPatientService 
    {
        void AddPatient(Patient patient);
        List<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
        Patient FindPatientByName(string name);
        IEnumerable<Patient> GetPatientsByDoctor(int doctorId);
        List<Patient> GetPagedPatients(int pageNumber, int pageSize = 5);
        List<Patient> SearchPatients(string name, int minAge, int maxAge, string condition, int? doctorId);
    }

}
