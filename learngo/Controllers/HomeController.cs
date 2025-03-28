using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models; // Добавляем пространство имен для моделей
using Backend.Controllers; // Для TopicDto

namespace Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Catalog()
        {
            var sections = await _context.Sections
                .Select(s => new
                {
                    s.Id,
                    s.SectionName,
                    TopicCount = _context.Topics.Count(t => t.SectionId == s.Id)
                })
                .ToListAsync();
            return View(sections);
        }

        public IActionResult AddTopic()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(TopicDto topicDto)
        {
            if (!ModelState.IsValid)
            {
                return View(topicDto);
            }

            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.SectionName == topicDto.SectionName);

            if (section == null)
            {
                section = new Section { SectionName = topicDto.SectionName };
                _context.Sections.Add(section);
                await _context.SaveChangesAsync();
            }

            var topic = new Topic
            {
                TopicName = topicDto.TopicName,
                Description = topicDto.Description,
                Difficulty = int.Parse(topicDto.Complexity),
                TimeLimit = topicDto.ReadingTime,
                AuthorName = topicDto.AuthorName,
                AuthorEmail = topicDto.Email,
                CreatedAt = DateTime.Parse(topicDto.CreationDate),
                SectionId = section.Id
            };

            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return RedirectToAction("Catalog");
        }

        public async Task<IActionResult> TopicDetails(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Section)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }
    }
}