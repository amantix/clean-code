using Markdown.Enums;
using System.Text;

namespace Markdown.Classes
{
    public static class MdToHtml
    {
        private static readonly Dictionary<Tags, string> tagToString = new()
    {
        { Tags.Italic, "em" },
        { Tags.Bold, "strong" },
        { Tags.Escape, "" },
        { Tags.H1, "h1" }
    };

        public static string Convert(string text, List<PairTag> pairTags, List<Tag> notPairTag)
        {
            var tagsStart = new Dictionary<int, Tag>();
            var tagsEnd = new Dictionary<int, Tag>();
            var isH1 = notPairTag.Count > 0 && notPairTag[0].Name == Tags.H1;
            var escapePositions = new HashSet<int>();

            foreach (var tag in notPairTag)
            {
                if (tag.Name == Tags.Escape) escapePositions.Add(tag.Position);

            }

            foreach (var pairTag in pairTags)
            {
                tagsStart[pairTag.Start.Position] = pairTag.Start;
                tagsEnd[pairTag.End.Position] = pairTag.End;
            }

            return Convert(text, isH1, tagsStart, tagsEnd, escapePositions);
        }

        private static string Convert(string text,
            bool isH1,
            Dictionary<int, Tag> tagsStart,
            Dictionary<int, Tag> tagsEnd,
            HashSet<int> escapePosition)
        {
            var html = isH1 ? new StringBuilder("<h1>") : new StringBuilder();
            for (var i = isH1 ? 2 : 0; i < text.Length; i++)
            {
                if (tagsStart.TryGetValue(i, out var tag))
                {
                    html.Append($"<{tagToString[tag.Name]}>");
                    i += tag.Length - 1;
                }
                else if (tagsEnd.TryGetValue(i, out tag))
                {
                    html.Append($"</{tagToString[tag.Name]}>");
                    i += tag.Length - 1;
                }

                else if (!escapePosition.Contains(i))
                    html.Append(text[i]);
            }

            if (isH1)
                html.Append("</h1>");
            return html.ToString();
        }
    }
}
