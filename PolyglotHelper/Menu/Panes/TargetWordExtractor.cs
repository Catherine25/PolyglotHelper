using PolyglotHelper.Extensions;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PolyglotHelper.Menu.Panes;

public class TargetWordExtractor
{
    private const string CzechRegex = "[a-zA-ZěĚšŠčČřŘžŽýÝáÁíÍéÉúÚůŮďĎťŤ]*";

    public IEnumerable<string> ProduceWords(string text, string studiedWord)
    {
        Regex regex = new(CzechRegex);
        MatchCollection matchCollection = regex.Matches(text);

        var studiedMatches = matchCollection.Where(m => m.Value == studiedWord)
            .Select(m => new MyMatch(m.Index, m.Length, m.Value)).ToList();

        var resultingMatches = studiedMatches.Where(m => m.Value == studiedWord).ToList();

        for (int i = 0; i < studiedMatches.Count() - 1; i++)
        {
            var result = AddBetween(text, studiedMatches[i], studiedMatches[i + 1]);
            resultingMatches.AddRange(result);
        }

        resultingMatches = PrependFirst(studiedMatches, text).ToList();
        resultingMatches = AppendLast(text, resultingMatches).ToList();

        IEnumerable<string> extractedWords = matchCollection.Select(w => w.Value);

        this.Log($"extracted words: {string.Join(' ', extractedWords)}");

        return resultingMatches.Select(s => s.Value);
    }

    private static IEnumerable<MyMatch> AddBetween(string text, MyMatch match1, MyMatch match2)
    {
        if (match1.Index == match2.Index)
            return new List<MyMatch> { match1, match2 };

        int start = match1.Index + match1.Length;
        int end = match2.Index;

        return new List<MyMatch> { match1, match2, new MyMatch(start, end, text[start..end]) };
    }

    private static IEnumerable<MyMatch> AppendLast(string text, IEnumerable<MyMatch> matches)
    {
        var lastMatch = matches.Last();

        int newWordIndex = lastMatch.Index + lastMatch.Length;
        int newWordLength = text.Length - newWordIndex;

        if (newWordIndex != text.Length)
        {
            MyMatch match = new(
                newWordIndex,
                newWordLength,
                text.Substring(newWordIndex, newWordLength)
            );

            matches = matches.Append(match);
        }

        return matches;
    }

    private IEnumerable<MyMatch> PrependFirst(IEnumerable<MyMatch> matches, string text)
    {
        var firstMatch = matches.First();

        if (firstMatch.Index != 0)
        {
            MyMatch element = new(0, firstMatch.Index, text.Substring(0, firstMatch.Index));
            matches = matches.Prepend(element);
        }

        return matches;
    }
}

public class MyMatch
{
    public MyMatch(int index, int length, string value)
    {
        Index = index;
        Length = length;
        Value = value;
    }

    public int Index { get; set; }
    public int Length { get; set; }
    public string Value { get; set; }
}
