using MarkdownRealisation.Abstractions;

namespace MarkdownRealisation.Interfaces
{
    public interface ITagsResolver
    {
        /// <summary>
        /// Обрабатывает теги, обозначает их как открытые/закрытые
        /// </summary>
        /// <param name="tokens">Массив тегов на обработку</param>
        /// <returns>Обработанный массив тегов</returns>
        public Token[] ResolveTokens(Token[] tokens);
        public Token[] ResolveTokensLines(Token[][] tokensLines);
    }
}
