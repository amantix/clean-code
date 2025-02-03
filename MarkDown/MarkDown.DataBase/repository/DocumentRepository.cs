using MarkDown.DataBase.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarkDown.DataBase.repository
{
    public class DocumentsRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<DocumentsRepository> _logger;

        public DocumentsRepository(MyDbContext dbContext,
            ILogger<DocumentsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Documents>> GetDocuments()
        {
            return await _dbContext.Documents
                .ToListAsync();
        }

        public async Task<List<string>> GetById(Guid id)
        {
            try
            {
                var documentEntity = await _dbContext.Documents
                   .AsNoTracking()
                   .Where(x => x.UserId == id)
                   .Select(x => x.NameFile)
                   .ToListAsync();
                return documentEntity;
            } 
            catch (Exception ex) 
            {
                throw new Exception($"Не удалось получить документу по ID: {ex}");
            }
        }

        public async Task Add(Guid userId, string name, string text)
        {
            try 
            { 
                var documentEntity = new Documents
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    NameFile = name,
                    Text = text,
                };

                await _dbContext.AddAsync(documentEntity);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                throw new Exception($"Не удалось добавить документ: {ex}");
            }
        }

        public async Task RemoveDocument(Guid userId, string nameFile)
        {
            try 
            { 
                await _dbContext.Documents
                    .Where(c => c.UserId == userId && c.NameFile == nameFile)
                    .ExecuteDeleteAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception($"Не удалось удалить документ: {ex}");
            }
        }
    }
}
