using PolyglotHelper.Database;
using PolyglotHelper.Database.Models;
using PolyglotHelper.Models;

namespace PolyglotHelper.Services;

public interface ILoggingService
{
    Task LogActivity(Card card, bool guessed);
    Task<int> CountLastSuccessfulAttempts(Card card);
    Task<ActivityDbItem> LastSuccessfulAttempt(Card card);
}

public class LoggingService : ILoggingService
{
    private readonly DatabaseService _databaseService;

    public LoggingService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task LogActivity(Card card, bool guessed)
    {
        await _databaseService.LogActivity(new ActivityDbItem
        {
            WordId = card.Word.Id,
            Date = DateTime.Now,
            Guessed = guessed
        });
    }

    public async Task<int> CountLastSuccessfulAttempts(Card card)
    {
        var logs = await LastSuccessfulAttemptsByWordId(card.Word.Id);
        return logs.Count();
    }

    public async Task<ActivityDbItem> LastSuccessfulAttempt(Card card)
    {
        var logs = await LastSuccessfulAttemptsByWordId(card.Word.Id);
        return logs.FirstOrDefault();
    }

    private async Task<IEnumerable<ActivityDbItem>> LastSuccessfulAttemptsByWordId(int id)
    {
        var logs = await _databaseService.GetLogsForWord(id);
        logs = logs.OrderByDescending(x => x.Date);
        return logs.TakeWhile(x => x.Guessed);
    }
}
