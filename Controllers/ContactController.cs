using Microsoft.AspNetCore.Mvc;
using ISMTCollege.Models;
using ISMTCollege.Data;
using Microsoft.EntityFrameworkCore;

namespace ISMTCollege.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Name,Email,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.UtcNow;
                _context.Add(contact);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you soon.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(contact);
        }
    }
}
