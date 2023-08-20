using PolyglotHelper.Database;
using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Services;

public static class TagService
{
    private static DatabaseService _databaseService;

    public static void Init(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public static async void Add(TagDbItem tag)
    {
        await _databaseService.SaveItemAsync(tag);
    }

    public static Task<List<TagDbItem>> GetAll()
    {
        return _databaseService.GetItemsAsync<TagDbItem>();
    }
}
