using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicaionDbContext dbContext;

        public StudentsController(ApplicaionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {

            //Console.WriteLine(viewModel.Name);
            //Console.WriteLine(viewModel.Email);
            //Console.WriteLine(viewModel.Phone);
            //Console.WriteLine(viewModel.Subscribed);

            var student = new Student
            {

                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed

            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await dbContext.Students.ToListAsync();

            return View(students);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid Id)
        {
            var student = await dbContext.Students.FindAsync(Id);

            return View(student);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await dbContext.Students.FindAsync(viewModel.Id);

            if(student is not null)
            {

                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;    
                student.Subscribed = viewModel.Subscribed;

                await dbContext.SaveChangesAsync();

            }

            return RedirectToAction("List", "Students");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var studnet = await dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if(studnet is not null)
            {
                dbContext.Students.Remove(viewModel);
                await dbContext.SaveChangesAsync();

            }

            return RedirectToAction("List", "Students");

        }
    }
}
