using PolyglotHelper.Database;
using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Import;

public interface IImporter
{
    Task Import(string text);
}

public class Importer : IImporter
{
    public DatabaseService _databaseService;
    public ITextProcessor _textProcessor;

    public Importer(DatabaseService databaseService,
        ITextProcessor textProcessor)
    {
        _databaseService = databaseService;
        _textProcessor = textProcessor;
    }
    
    public async Task Import(string text)
    {
        // load known words
        var knownWords = await _databaseService.Select<WordDbItem>();
        var knownWordsStrings = knownWords.Select(x => x.Word);

        // load known sentences
        var knownSentences = await _databaseService.Select<SentenceDbItem>();

        var wordsWithSentences = _textProcessor.Run(text);
        if (knownSentences.Any(s => s.Sentence == text))
            throw new SentenceAlreadyImportedException();

        wordsWithSentences = wordsWithSentences.Where(x => !knownWordsStrings.Contains(x.Word));

        // todo distinct sentences
        foreach (var newPair in wordsWithSentences)
        {
            var sentenceDbItem = new SentenceDbItem()
            {
                Blocked = false,
                Note = null,
                Sentence = newPair.Sentence
            };

            sentenceDbItem = await _databaseService.SaveSentenceAsync(sentenceDbItem);

            //int sentenceId = await _databaseService.GetItemAsync<SentenceDbItem>(x => x.Sentence == sentenceDbItem.Sentence);

            var wordDbItem = new WordDbItem()
            {
                Blocked = false,
                Note = null,
                RepeatTime = null,
                SentenceId = sentenceDbItem.Id,
                State = States.NotShown,
                Word = newPair.Word
            };

            wordDbItem = await _databaseService.SaveWordAsync(wordDbItem);        
        }
    }
}
