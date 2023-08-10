using PolyglotHelper.Models;

namespace PolyglotHelper.Services;

public interface INextGuessService
{
    public Task<bool> CanGuessNow(Card card);
}

public class NextGuessService : INextGuessService
{
    private readonly ILoggingService _loggingService;

    public NextGuessService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<bool> CanGuessNow(Card card)
    {
        int attempts = await _loggingService.CountLastSuccessfulAttempts(card);
        var lastSuccessfulAttempt = await _loggingService.LastSuccessfulAttempt(card);

        if (attempts >= Settings.AttemptsToCompleteWord || lastSuccessfulAttempt == null)
            return true;

        var nextDate = CalculateNextDate(lastSuccessfulAttempt.Date, attempts);

        return DateTime.Now > nextDate;
    }

    private DateTime CalculateNextDate(DateTime previousDate, int attempts)
    {
        DateTime nextDate = previousDate;

        if (attempts == 1)
            nextDate = nextDate.AddHours(1);
        else if (attempts == 2)
            nextDate = nextDate.AddDays(1);
        else if (attempts == 3)
            nextDate = nextDate.AddDays(3);
        else if (attempts == 4)
            nextDate = nextDate.AddDays(7);

        return nextDate;
    }
}
