using Markdown.Enums;

namespace Markdown.Classes
{
    public class TagParser
    {
        public List<PairTag> PairTags;
        public List<Tag> SingleTags;

        private static readonly List<Tags> pairTagsInfo = new()
    {
        Tags.Italic,
        Tags.Bold
    };

        private TagParser(List<PairTag> pairTags, List<Tag> singleTags)
        {
            PairTags = pairTags;
            SingleTags = singleTags;
        }

        public static TagParser BuildTags(string text)
        {
            var allTags = GetAllTags(text).ToList();
            var tagsWithoutEscape = GetNotEscapedTags(allTags).ToList();
            var singleTags = GetNotPairTags(tagsWithoutEscape).ToList();
            var pairTags = GetPairTags(tagsWithoutEscape, TagParser.pairTagsInfo).OrderBy(x => x.Start.Position)
                .ToList();
            var notIncludeTag = GetNotIncludeTag(pairTags, Tags.Bold, Tags.Italic).ToList();
            var correctTags = GetPairTagsWithCorrectSpaces(text, notIncludeTag).ToList();

            return new TagParser(correctTags, singleTags);
        }

        private static IEnumerable<PairTag> GetPairTagsWithCorrectSpaces(string text, List<PairTag> pairTags)
        {
            foreach (var pairTag in pairTags)
            {
                if (pairTag.Length == pairTag.Start.Length)
                    continue;

                if (IsTagInsideWord(text, pairTag.Start.Position, pairTag.Start.Length)
                    && char.IsDigit(text[pairTag.Start.Position + pairTag.Start.Length]))
                    continue;

                if (text[pairTag.Start.Position + pairTag.Start.Length] != ' ' && text[pairTag.End.Position - 1] != ' ')
                    yield return pairTag;
                else if (IsTagInsideWord(text, pairTag.Start.Position, pairTag.Start.Length)
                         && text.IndexOf(' ', pairTag.Start.Position, pairTag.Length) == -1)
                    yield return pairTag;
            }
        }

        private static bool IsTagInsideWord(string text, int positions, int length) =>
            text.Length > positions + length
            && positions > 0
            && text[positions + length] != ' '
            && text[positions - 1] != ' ';

        private static IEnumerable<PairTag> GetNotIncludeTag(List<PairTag> pairTags, Tags insideTagInfo,
            Tags outTagInfo)
        {
            for (var i = 0; i < pairTags.Count; i++)
            {
                if (pairTags[i].Start.Name == outTagInfo)
                {
                    var pairTag = pairTags[i];
                    yield return pairTags[i];
                    while (i + 1 < pairTags.Count && pairTags[i + 1].Start.Position < pairTag.End.Position)
                    {
                        if (pairTags[i + 1].Start.Name != insideTagInfo)
                            yield return pairTags[i + 1];
                        i++;
                    }
                }
                else
                    yield return pairTags[i];
            }
        }

        private static IEnumerable<Tag> GetNotPairTags(List<Tag> tags) =>
            tags.Where(tag => tag.Name is Tags.H1 or Tags.Escape);

        private static IEnumerable<PairTag> GetPairTags(List<Tag> tags, List<Tags> pairTags)
        {
            var tagStack = new Stack<Tag>();
            var openTags = new HashSet<Tags>();
            var ignoreTags = new HashSet<Tags>();
            foreach (var tag in tags.Where(tag => pairTags.Contains(tag.Name)))
            {
                if (ignoreTags.Remove(tag.Name))
                    continue;

                if (!openTags.Remove(tag.Name))
                {
                    tagStack.Push(tag);
                    openTags.Add(tag.Name);
                    continue;
                }

                var openTag = tagStack.Pop();
                if (openTag.Name == tag.Name)
                {
                    yield return new PairTag(openTag, tag);
                    continue;
                }

                while (openTag.Name != tag.Name)
                {
                    openTags.Remove(openTag.Name);
                    ignoreTags.Add(openTag.Name);
                    openTag = tagStack.Pop();
                }
            }
        }

        private static IEnumerable<Tag> GetNotEscapedTags(List<Tag> allTags)
        {
            for (var i = 0; i < allTags.Count; i++)
            {
                if (allTags[i].Name == Tags.Escape
                    && i + 1 < allTags.Count
                    && allTags[i].Position + 1 == allTags[i + 1].Position)
                {
                    yield return allTags[i];
                    i++;
                }
                else if (allTags[i].Name != Tags.Escape)
                    yield return allTags[i];
            }
        }

        private static IEnumerable<Tag> GetAllTags(string paragraph)
        {
            for (var i = 0; i < paragraph.Length; i++)
            {
                var tag = GetTag(i, paragraph);
                if (tag == null)
                    continue;

                i += tag.Length - 1;
                yield return tag;
            }
        }

        private static Tag? GetTag(int position, string text) 
        {
            switch (text[position])
            {
                case '#':
                    return position == 0
                        ? new Tag(Tags.H1, position, 1)
                        : null; // Так делать нельзя, скоро исправлю
                case '_':
                    if (position + 1 < text.Length && text[position + 1] == '_')
                        return new Tag(Tags.Bold, position, 2);
                    else
                        return new Tag(Tags.Italic, position, 1);
                case '\\':
                    return new Tag(Tags.Escape, position, 1);
            }

            return null;
        }
    }
}
