namespace PolyglotHelper.Import;

public interface ITextProcessor
{
    IEnumerable<WordInContextDto> Run(string text);
}

public class TextProcessor : ITextProcessor
{
    public IEnumerable<WordInContextDto> Run(string text)
    {
        return MapWordsToSentences(text)
            .DistinctBy(x => x.Word);
    }

    private static IEnumerable<WordInContextDto> MapWordsToSentences(string text)
    {
        var sentences = ExtractSentences(text);
        return sentences
            .SelectMany(sentence => ExtractWords(sentence)
            .Select(word => new WordInContextDto(word, sentence)));
    }

    private static string[] ExtractSentences(string text)
    {
        return text.Split(new char[] { '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] ExtractWords(string sentence)
    {
        return sentence.Split(new char[] { ' ', ',', '?', '(', ')', '/', '=', ':', '-', ';' },
            StringSplitOptions.RemoveEmptyEntries);
    }
}

public record WordInContextDto(string Word, string Sentence);

