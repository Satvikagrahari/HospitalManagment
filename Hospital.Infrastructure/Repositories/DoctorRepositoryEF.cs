using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;

public class DoctorRepositoryEF : IRepository<Doctor>
{
    private readonly AppDbContext _context;

    public DoctorRepositoryEF(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Doctor entity)
    {
        _context.Doctors.Add(entity);
        _context.SaveChanges();
    }

    public IEnumerable<Doctor> GetAll()
    {
        return _context.Doctors.ToList();
    }

    public Doctor GetById(int id)
    {
        return _context.Doctors.Find(id);
    }

    public void Update(Doctor entity)
    {
        _context.Doctors.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
        }
    }
}