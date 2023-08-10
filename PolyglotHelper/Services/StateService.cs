using PolyglotHelper.Database.Models;
using PolyglotHelper.Models;

namespace PolyglotHelper.Services;

public interface IStateService
{
    Task<States> GetNextState(Card card, bool isAnsweredCorrect);
    Task<bool> ReadyToForRepeating(Card card);
}

public class StateService : IStateService
{
    private readonly ILoggingService _loggingService;
    private readonly INextGuessService _nextGuessService;

    public StateService(ILoggingService loggingService,
        INextGuessService nextAttemptService)
    {
        _loggingService = loggingService;
        _nextGuessService = nextAttemptService;
    }

    public async Task<States> GetNextState(Card card, bool isAnsweredCorrect)
    {
        if (card.Word.State == States.NotShown)
            return isAnsweredCorrect ? States.ImmediatelyGuessed : States.Studying;

        if (card.Word.State == States.Studying)
        {
            int attempts = await _loggingService.CountLastSuccessfulAttempts(card);

            if (isAnsweredCorrect && attempts >= Settings.AttemptsToCompleteWord)
                return States.StudyingDone;

            return States.Studying;
        }

        if (card.Word.State == States.ImmediatelyGuessed && card.Word.State == States.StudyingDone)
            throw new Exception("Cannot get next state!");

        return States.NotShown;
    }

    public async Task<bool> ReadyToForRepeating(Card card)
    {
        if (card.Word.State == States.StudyingDone || card.Word.State == States.ImmediatelyGuessed)
            return false;

        if (card.Word.State == States.NotShown)
            return true;

        return await _nextGuessService.CanGuessNow(card);
    }
}
