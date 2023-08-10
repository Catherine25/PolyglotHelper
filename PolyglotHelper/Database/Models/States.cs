namespace PolyglotHelper.Database.Models;

public enum States
{
    // word that hasn't been shown yet
    NotShown,

    // word that was not guessed during first attempt
    Studying,

    // word that was guessed during first attempt
    ImmediatelyGuessed,

    // word that was not guessed during first attempt,
    // but the studying is completed
    StudyingDone
}