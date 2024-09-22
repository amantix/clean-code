namespace MarkdownContract
{
    public interface IParse
    {
        /// <summary>
        /// Ищет следющий тег
        /// </summary>
        /// <returns>true, если найден следущий тэг, и false, если не найден</returns>
        public bool FindTag();

        /// <summary>
        /// Конвертирует MD тег в HTML
        /// </summary>
        /// <returns>HTML тег, соответствующий </returns>
        public string ConvertTag();
    }
}
