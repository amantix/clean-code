namespace MarkdownRenderer.Interfaces
{
    /// <summary>
    /// Интерфейс для сущности, считывающей текст, с которым надо будет работать MdProcessor'у.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Считывает и сохраняет содержимое md файла.
        /// </summary>
        /// <param name="pathToFile">Путь до md файла.</param>
        /// <returns>Содержимое md файла со всеми тегами.</returns>
        string ParseTextFromFile(string pathToFile);
    }
}
