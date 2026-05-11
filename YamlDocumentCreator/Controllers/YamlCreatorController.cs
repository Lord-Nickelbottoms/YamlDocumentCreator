
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YamlDocumentCreator.Models;
using YamlDocumentCreator.Models.ViewModels;
using YamlDocumentCreator.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class YamlCreator : Controller
{
    private readonly AttachmentDbContext _context;
    private readonly IAttachmentService _attachmentService;

    public YamlCreator(AttachmentDbContext context, IAttachmentService attachmentService)
    {
        _context = context;
        _attachmentService = attachmentService;
    }

    // GET: YAMLDOCUMENTS
    public async Task<IActionResult> Index()
    {
        return View(await _context.Attachment.ToListAsync());
    }

    // GET: YAMLDOCUMENTS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attachment = await _context.Attachment
            .FirstOrDefaultAsync(m => m.Id == id);
        if (attachment == null)
        {
            return NotFound();
        }

        return View(attachment);
    }

    // GET: YAMLDOCUMENTS/Create
    public IActionResult Create()
    {
        var yesNoList = new List<SelectListItem>
        {
            new SelectListItem { Value = "yes", Text = "Yes"},
            new SelectListItem { Value = "no", Text = "No"}
        };

        ViewBag.yesNoList = yesNoList;

        return View();
    }

    // POST: YAMLDOCUMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Group,UserCanDelete,AccessSftp")] YamlDocument document)
    {
        document.Id = Guid.NewGuid().ToString();
        if (ModelState.IsValid)
        {
            if (document.UserCanDelete.ToString() == "yes")
            {
                document.UserCanDelete = "true";
            }
            else if (document.UserCanDelete.ToString() == "no")
            {
                document.UserCanDelete = "false";
            }

            string filePath = "./YAMLDocuments/";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var serializer = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            var yamlDocument = serializer.Serialize(document);

            // using var fileWriter = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            // using var streamWriter = new StreamWriter(fileWriter);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, $"{document.Name}.yml")))
            {
                using (var stream = System.IO.File.OpenRead(Path.Combine(filePath, $"{document.Name}.yml")))
                {
                    if (stream.CanSeek)
                    {
                        Debug.Print($"\n\nLength of document is: ${stream.Length} bytes\n\n");
                    }

                    AttachmentVM vm = new()
                    {
                        Id = document.Id,
                        File = new FormFile(stream, 0, stream.Length, null, document.Name)
                    };

                    await outputFile.WriteAsync(yamlDocument);
                    await outputFile.FlushAsync();

                    await _attachmentService.UploadAttachment(vm);
                }
            }

            // using (var stream = System.IO.File.OpenRead(filePath))
            // {
            //     AttachmentVM vm = new()
            //     {
            //         Id = Guid.NewGuid().ToString(),
            //         File = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            //     };
            //     await _attachmentService.UploadAttachment(vm);
            // }



            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: YAMLDOCUMENTS/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attachment = await _context.Attachment.FindAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }
        return View(attachment);
    }

    // POST: YAMLDOCUMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Attachment attachment)
    {
        if (id != attachment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(attachment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttachmentExists(attachment.Id))
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
        return View(attachment);
    }

    // GET: YAMLDOCUMENTS/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var attachment = await _context.Attachment
            .FirstOrDefaultAsync(m => m.Id == id);
        if (attachment == null)
        {
            return NotFound();
        }

        return View(attachment);
    }

    // POST: YAMLDOCUMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var attachment = await _context.Attachment.FindAsync(id);
        if (attachment != null)
        {
            _context.Attachment.Remove(attachment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AttachmentExists(string? id)
    {
        return _context.Attachment.Any(e => e.Id == id);
    }
}
