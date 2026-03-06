using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Infrastructure.Repositories
{
    public class PatientRepositoryEF : IRepository<Patient>
    {
        private readonly AppDbContext _context;

        public PatientRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Patient entity)
        {
            _context.Patients.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients
                .Include(p => p.Doctor) // join doctor
                .ToList();
        }

        public Patient GetById(int id)
        {
            return _context.Patients
                .Include(p => p.Doctor)
                .FirstOrDefault(p => p.PatientId == id);
        }

        public void Update(Patient entity)
        {
            _context.Patients.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }
    }
}