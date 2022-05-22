#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain.DatabaseEntryModels;

namespace AdvancedProgrammingProjectsServer.Controllers
{
    public class RatingsController : Controller
    {
        private readonly AdvancedProgrammingProjectsServerContext _context;

        public RatingsController(AdvancedProgrammingProjectsServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all the ratings
        /// </summary>
        /// <returns>A page showing all the ratings in a list</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rating.ToListAsync());
        }

        /// <summary>
        /// Gets details about a single review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        /// <summary>
        /// Goes to the view for creating a new ratings.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new ratings
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Stars,Feedback,Name")] Rating rating)
        {
            rating.TimeSubmitted = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        /// <summary>
        /// Displays the edit page for a single review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return View(rating);
        }

        /// <summary>
        /// Edits a single review
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Stars,Feedback,Name,TimeSubmitted")] Rating rating)
        {
            if (id != rating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        /// <summary>
        /// Displays the page for deleting a review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.Id == id);
        }

        /// <summary>
        /// Searches for reviews with the matching feedback text.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string content) {

            if (content != null) {
                var ratings2 = _context.Rating.Where(r => r.Feedback.Contains(content));
                return PartialView(await ratings2.ToListAsync());
            }
            else {
                return PartialView(await _context.Rating.ToListAsync());
            }
        }
    }
}
