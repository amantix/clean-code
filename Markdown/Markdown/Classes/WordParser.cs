using Markdown.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class WordParser : IParser<Token>
    {
        private readonly Dictionary<string, TagType> tagDictionary = new Dictionary<string, TagType>();

        public WordParser(Dictionary<string, TagType> tagDictionary)
        {
            this.tagDictionary = tagDictionary;
        }

        public List<Token> Parse(string line)
        {
            var tokens = new List<Token>();
            var globalTags = new List<GlobalTag>();
            var words = line.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                var word = GetWordWithoutEscapedSymbols(words[i]);
                var localTags = ParseWordTags(word, globalTags, i);

                localTags = localTags.OrderBy(x => x.Index).ToList();
                localTags = CheckTagOrder(localTags, "__ _ _ __ ");

                tokens.Add(new Token(word, localTags));
            }

            globalTags = GetPairsGlobalTag(globalTags);
            globalTags = globalTags.OrderBy(x => x.GlobalIndex).ToList();
            globalTags = CheckGlobalTagOrder(globalTags, "__ _ _ __ ");
            JoinGlobalTags(globalTags, tokens);

            return tokens;
        }

        private string GetWordWithoutEscapedSymbols(string word)
        {
            var result = new StringBuilder();
            var escapedSymbols = @"\#_";
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Equals('\\') && i + 1 < word.Length)
                {
                    if (escapedSymbols.Contains(word[i + 1]))
                    {
                        if (word[i + 1] == '_' && i + 2 < word.Length && word[i + 2] == '_')
                            i++;
                        i++;
                        continue;
                    }
                }
                result.Append(word[i]);
            }

            return result.ToString();
        }

        private List<Tag> ParseWordTags(string word, List<GlobalTag> globalTags, int globalIndex)
        {
            var tags = new List<Tag>();
            var dict = new Dictionary<string, Tag>();

            for (int i = 0; i < word.Length;i++)
            {
                string mdTag;
                if (IsTag(word[i].ToString()))
                {
                    mdTag = DefineTag(word, i);
                }
                else
                    continue;

                if (!NumberCheck(mdTag, word, i))
                    continue;

                var tag = new Tag(i, tagDictionary[mdTag]);
                
                if (dict.ContainsKey(tag.Type.MarkdownTag))
                {
                    var oldTag = dict[tag.Type.MarkdownTag];
                    dict.Remove(tag.Type.MarkdownTag);
                    tags.Add(oldTag);
                    tags.Add(tag);
                }
                else
                {
                    tag.IsStart = true;
                    if (tag.Type.MarkdownTag == "#")
                    {
                        tags.Add(tag);
                        return tags;
                    }

                    dict.Add(tag.Type.MarkdownTag, tag);
                }

                if (mdTag.Length == 2)
                    i++;
            }

            if (dict.Count > 0)
            {
                foreach (var globalTag in dict)
                {
                    if (globalTag.Value.Index == 0 || globalTag.Value.Index == word.Length - globalTag.Key.Length)
                    {
                        if (globalTag.Value.Index == 0)
                            globalTag.Value.IsStart = true;
                        else
                            globalTag.Value.IsStart = false;
                        globalTags.Add(new GlobalTag(globalIndex, globalTag.Value));
                    }
                }
            }

            return tags;
        }

        private List<Tag> CheckTagOrder(List<Tag> tags, string tagOrderTemplate)
        {
            var result = new List<Tag>();

            for (int i = 0; i < tags.Count; i++)
            {
                if (!tags[i].Type.IsDoubleTag)
                {
                    result.Add(tags[i]);
                    continue;
                }
                if (tags[i].Type.MarkdownTag.Equals(tags[i + 1].Type.MarkdownTag))
                {
                    if (!(tags[i].Type.MarkdownTag == "__" && tags[i].Index + 2 == tags[i + 1].Index))
                    {
                        result.Add(tags[i]);
                        result.Add(tags[i + 1]);
                    }
                    i++;
                }
                else
                {
                    var subsequence = GetShortEntry(tags, i, 4);
                    if (subsequence.Equals(tagOrderTemplate))
                    {
                        AddSequenceToResult(tags, result, i, 4);
                    }
                    i = i + 3;
                }
            }
            
            return result;
        }

        private bool IsTag (string tag)
        {
            return tagDictionary.ContainsKey(tag);
        }

        private string DefineTag(string word, int index)
        {
            var symbol = word[index].ToString();

            if (index + 1 < word.Length && IsTag(symbol + word[index + 1]))
                return symbol + word[index + 1];
            return symbol;
        }

        private bool NumberCheck(string mdTag, string word, int index)
        {
            if (index - 1 >= 0 && char.IsDigit(word[index - 1]))
            {
                return false;
            }
            if (index + mdTag.Length < word.Length && char.IsDigit(word[index + mdTag.Length]))
            {
                return false;
            }

            return true;
        }

        private string GetShortEntry(List<Tag> tags, int index, int amount)
        {
            var result = new StringBuilder();
            for (int i = index; i < index + amount; i++)
            {
                result.Append($"{tags[i].Type.MarkdownTag} ");
            }
            return result.ToString();
        }

        private void AddSequenceToResult(List<Tag> tags, List<Tag> result, int index, int amount)
        {
            for (int i = index; i < index + amount; i++)
            {
                result.Add(tags[i]);
            }
        }

        private List<GlobalTag> GetPairsGlobalTag(List<GlobalTag> tags)
        {
            var result = new List<GlobalTag>();
            var dict = new Dictionary<string, GlobalTag>();

            foreach (var tag in tags)
            {
                if (dict.ContainsKey(tag.Type.MarkdownTag))
                {
                    if (!tag.IsStart)
                    {
                        var oldTag = dict[tag.Type.MarkdownTag];
                        dict.Remove(tag.Type.MarkdownTag);
                        result.Add(oldTag);
                        result.Add(tag);
                    }
                    else
                    {
                        dict[tag.Type.MarkdownTag] = tag;
                    }
                }
                else
                {
                    if (tag.IsStart)
                    {
                        dict.Add(tag.Type.MarkdownTag, tag);
                    }
                }
            }

            return result;
        }

        private List<GlobalTag> CheckGlobalTagOrder(List<GlobalTag> tags, string tagOrderTemplate)
        {
            var result = new List<GlobalTag>();
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Type.MarkdownTag.Equals(tags[i + 1].Type.MarkdownTag))
                {
                    result.Add(tags[i]);
                    result.Add(tags[i + 1]);
                    i++;
                }
                else
                {
                    if (GetShortEntry(tags, i, 4).Equals(tagOrderTemplate))
                        AddSequenceToResult(tags, result, i, 4);
                    i = i + 3;
                }
            }

            return result;
        }

        private string GetShortEntry(List<GlobalTag> tags, int index, int amount)
        {
            var result = new StringBuilder();
            for (int i = index; i < index + amount; i++)
            {
                result.Append($"{tags[i].Type.MarkdownTag} ");
            }
            return result.ToString();
        }

        private void AddSequenceToResult(List<GlobalTag> tags, List<GlobalTag> result, int index, int amount)
        {
            for (int i = index; i < index + amount; i++)
            {
                result.Add(tags[i]);
            }
        }

        private void JoinGlobalTags(List<GlobalTag> globalTags, List<Token> tokens)
        {
            foreach (var tag in globalTags)
            {
                tokens[tag.GlobalIndex].Tags.Add(tag);
            }
        }
    }
}
