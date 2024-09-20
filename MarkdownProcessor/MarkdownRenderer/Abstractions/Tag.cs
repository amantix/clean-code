namespace MarkdownRenderer.Abstractions
{
    /// <summary>
    /// Базовый абстрактный класс, для всех тегов
    /// </summary>
    public abstract class Tag
    {
        /// <summary>
        /// Символ, представляющий тег в markdown.
        /// </summary>
        public abstract string MarkdownSymbol { get; }
        /// <summary>
        /// Символ, представляющий тег в html.
        /// </summary>
        public abstract string HtmlTag { get; }

        /// <summary>
        /// Преобразует содержимое между тегами в html.
        /// </summary>
        /// <param name="content">Текст между тегами.</param>
        /// <returns>Строка, обернутая в html теги.</returns>
        public virtual string ConvertToHtmlTag(string content)
        {
            return $"<{HtmlTag}>{content}</{HtmlTag}>";
        }
    }
}
