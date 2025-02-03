using Data.Abstracts;
using Data.Utils;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Data.Repositories
{
    public class DocumentRepository(ApplicationContext dbContext) : IDocumentRepository
    {
        public async Task<ActionResult<int>> CreateDocumentAsync(int userId, string title, bool isPublic, string Link)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
                return ActionResult<int>.Fail("User not found");

            var documentEntity = new Document
            {
                UserId = userId,
                User = user!,
                Title = title,
                isPublic = isPublic,
                Link = Link,
            };

            await dbContext.Documents.AddAsync(documentEntity);
            await dbContext.SaveChangesAsync();
            documentEntity.Link = documentEntity.Link + documentEntity.Id;
            await dbContext.SaveChangesAsync();

            return ActionResult<int>.Ok(documentEntity.Id);
        }

        public async Task<ActionResult<string>> DeleteDocumentAsync(int documentId)
        {
            var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.Id == documentId);

            if (documentEntity == null)
                return ActionResult<string>.Fail("Document not found")!;

            dbContext.Documents.Remove(documentEntity!);
            await dbContext.SaveChangesAsync();

            return ActionResult<string>.Ok(documentEntity.Title);
        }

        public async Task<ActionResult<Document>> GetDocumentByIdAsync(int documentId)
        {
            var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.Id == documentId)!;

            if (documentEntity == null)
                return ActionResult<Document>.Fail("Document not found")!;

            return ActionResult<Document>.Ok(documentEntity);
        }

        public async Task<ActionResult<ICollection<Document>?>> GetDocumentsAsync(int userId)
        {
            var documentEntities = await dbContext.Documents
                .Where(d => d.UserId == userId).ToListAsync();
            if (documentEntities.Count == 0)
                return ActionResult<ICollection<Document>>.Fail("Documents doesn't exists");
            return ActionResult<ICollection<Document>?>.Ok(documentEntities);
        }

        public async Task<ActionResult<string>> ChangeDocumentAsync(int documentId, string newTitle, bool isPublic, string Link)
        {
            var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.Id == documentId);

            if (documentEntity == null)
                return ActionResult<string>.Fail("Document not found")!;

            documentEntity.Title = newTitle;
            documentEntity.isPublic = isPublic;
            documentEntity.Link = Link;
            await dbContext.SaveChangesAsync();

            return ActionResult<string>.Ok(newTitle);
        }

        public async Task<bool> IsDocumentExistsByIdAsync(int documentId)
        {
            return await dbContext.Documents.AnyAsync(a => a.Id == documentId);
        }

        public async Task<ActionResult<string>> GetAuthorNameAsync(int documentId)
        {
            var documentEntity = await dbContext.Documents.Include(documentEntity => documentEntity.User).FirstOrDefaultAsync(d => d.Id == documentId);

            if (documentEntity == null)
                return ActionResult<string>.Fail("Document not found")!;

            var user = await dbContext.Users
                .FirstOrDefaultAsync(a => a.Id == documentEntity.UserId);

            return ActionResult<string>.Ok(user!.Username!);
        }

    }
}
