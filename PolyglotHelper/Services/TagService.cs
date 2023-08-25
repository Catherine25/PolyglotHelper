using PolyglotHelper.Database;
using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Services;

public class TagService
{
    private readonly DatabaseService _databaseService;

    public TagService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async void Add(TagDbItem tag)
    {
        await _databaseService.SaveItemAsync(tag);
    }

    public Task<List<TagDbItem>> GetAll()
    {
        return _databaseService.GetItemsAsync<TagDbItem>();
    }
}
