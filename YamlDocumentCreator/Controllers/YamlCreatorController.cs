
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YamlDocumentCreator.Models;

public class YamlCreator : Controller
{
    private readonly YamlDocumentDbContext _context;

    public YamlCreator(YamlDocumentDbContext context)
    {
        _context = context;
    }

    // GET: YAMLDOCUMENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.YamlDocument.ToListAsync());
    }

    // GET: YAMLDOCUMENTS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yamldocument = await _context.YamlDocument
            .FirstOrDefaultAsync(m => m.Id == id);
        if (yamldocument == null)
        {
            return NotFound();
        }

        return View(yamldocument);
    }

    // GET: YAMLDOCUMENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: YAMLDOCUMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Price")] YamlDocument yamldocument)
    {
        if (ModelState.IsValid)
        {
            _context.Add(yamldocument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(yamldocument);
    }

    // GET: YAMLDOCUMENTS/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yamldocument = await _context.YamlDocument.FindAsync(id);
        if (yamldocument == null)
        {
            return NotFound();
        }
        return View(yamldocument);
    }

    // POST: YAMLDOCUMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("Id,Title,ReleaseDate,Genre,Price")] YamlDocument yamldocument)
    {
        if (id != yamldocument.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(yamldocument);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YamlDocumentExists(yamldocument.Id))
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
        return View(yamldocument);
    }

    // GET: YAMLDOCUMENTS/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var yamldocument = await _context.YamlDocument
            .FirstOrDefaultAsync(m => m.Id == id);
        if (yamldocument == null)
        {
            return NotFound();
        }

        return View(yamldocument);
    }

    // POST: YAMLDOCUMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var yamldocument = await _context.YamlDocument.FindAsync(id);
        if (yamldocument != null)
        {
            _context.YamlDocument.Remove(yamldocument);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool YamlDocumentExists(string? id)
    {
        return _context.YamlDocument.Any(e => e.Id == id);
    }
}
