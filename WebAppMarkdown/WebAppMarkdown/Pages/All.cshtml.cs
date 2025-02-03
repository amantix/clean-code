using WebAppMarkdown.Services;
using WebAppMarkdown.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMarkdown.Pages
{
    public class DocumentsModel : PageModel
    {
        private readonly DocumentService _documentService;
        private readonly UsersService _usersService;
        public List<Document> Documents { get; set; }


        // Конструктор для внедрения зависимостей
        public DocumentsModel(DocumentService documentService, UsersService usersService)
        {
            _documentService = documentService;
            _usersService = usersService;
            Documents = new List<Document>();
        }

        // Список документов, который будет передан в представление

        // Метод для загрузки документов
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Замените на реальный способ получения userId, например, из контекста пользователя
                var a = await _documentService.GetDocumentsById(_usersService.GetUserIdFromToken(User).Result.Value);
                Documents = a;
            }
            catch (Exception ex)
            {
                // Логируем или обрабатываем исключения
                Console.WriteLine($"Error: {ex.Message}");
            }
            return Page();
        }
    }
}