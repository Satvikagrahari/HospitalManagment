using System;
using System.Collections.Generic;
using System.Text;
using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces
{
    public interface IDoctorService 
    {
        void AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int id);
        void UpdateDoctor(Doctor doctor);
        void DeleteDoctor(int id);
    }
}
