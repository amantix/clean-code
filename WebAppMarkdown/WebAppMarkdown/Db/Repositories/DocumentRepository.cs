using Microsoft.EntityFrameworkCore;
using WebAppMarkdown.Core.Models;
using WebAppMarkdown.Db.Interfaces;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddMarkdownEntryAsync(DocumentEntity documentEntity)
    {
        await _context.MarkdownEntries.AddAsync(documentEntity);
        await _context.SaveChangesAsync();
    }
    
    
    public async Task<List<DocumentEntity>> GetEntriesByUserIdAsync(Guid userId)
    {
        return await _context.MarkdownEntries
            .Where(m => m.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<List<DocumentEntity>> GetDocsByIdAsync(Guid id)
    {
        return await _context.MarkdownEntries
            .Where(m => m.Id == id)
            .ToListAsync();
    }
    

    public async Task DeleteMarkdownEntryAsync(Guid userId, Guid id)
    {
        var documentEntity = await _context.MarkdownEntries
            .FirstOrDefaultAsync(m => m.UserId == userId && m.Id == id);
    }
}