using ex6.Data;
using ex6.Models;
using ex6.Models.DTOs;
using ex6.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ex6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> getDoctors()
        {
            var doctors = await _doctorService.GetDoctors();
            
            return Ok(doctors);
        }
    }

    [Route("api/doctors/remove")]
    [ApiController]
    public class DoctorsControllerDel : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsControllerDel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpDelete("{idDoctor}")]
        public async Task<IActionResult> removeDoctor(int idDoctor)
        {
            var context = _doctorService.GetContext();
            var doctor = context.Doctors.FirstOrDefault(e => e.IdDoctor == idDoctor);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            context.Remove<Doctor>(doctor);

            await context.SaveChangesAsync();

            return Ok(doctor);            
        }
    }

    [Route("api/doctors/add")]
    [ApiController]
    public class DoctorsControllerAdd : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsControllerAdd(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        public async Task<IActionResult> addDoctor(NewDoctor newDoctor)
        {
            var context = _doctorService.GetContext();

            var doesDoctorExist = context.Doctors.Any(e => e.IdDoctor == newDoctor.IdDoctor);

            if (!doesDoctorExist)
            {
                var doctor = new Doctor
                {
                    IdDoctor = newDoctor.IdDoctor,
                    FirstName = newDoctor.FirstName,
                    LastName = newDoctor.LastName,
                    Email = newDoctor.Email,
                    Prescriptions = new List<Prescription>()

                };

                try
                {
                    context.Add<Doctor>(doctor);

                    await context.SaveChangesAsync();
                } catch (Exception ex)
                {
                    BadRequest(ex.InnerException?.Message ?? ex.Message);
                }

                return Ok(newDoctor);
            }
            else
            {
                return Conflict("The doctor is already added!");
            }
        }
    }
}
