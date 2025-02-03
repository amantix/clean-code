using MarkDown.DataBase.models;
using MarkDown.DataBase.repository;

namespace WebAPI.Services
{
    public class DocumentService
    {
        public readonly DocumentsRepository _documentsRepository;
        public DocumentService(
            DocumentsRepository documentsRepository) 
        {
            _documentsRepository = documentsRepository;
        }

        public async Task Create(Guid userId, string text, string nameFile) 
        {
            await _documentsRepository.Add(userId, nameFile, text);
        }

        public async Task Delete(Guid userId, string nameFile)
        {
            await _documentsRepository.RemoveDocument(userId, nameFile);
        }

        public async Task<List<string>> GetById(Guid userId) 
        {
            return await _documentsRepository.GetById(userId);
        }
    }
}
