namespace MarkdownContract
{
    public interface IRender
    {
        /// <summary>
        /// Конвертирует текст с Markdown разметкой в HTML
        /// </summary>
        /// <param name="text">Текст с md разметкой</param>
        /// <returns>Текст с html разметкой</returns>
        public string Render(string text);
    }
}
