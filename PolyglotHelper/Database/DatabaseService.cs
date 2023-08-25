using PolyglotHelper.Database.Models;
using PolyglotHelper.Extensions;
using SQLite;
using System.Linq.Expressions;

namespace PolyglotHelper.Database;

public class DatabaseService
{
    private SQLiteAsyncConnection Database;

    public DatabaseService()
    {
    }

    private async Task Init()
    {
        if (Database is not null)
            return;

        string databasePath = DatabaseConfiguration.DatabasePath;
        
        this.Log("\n");
        this.Log("Database path is " + databasePath);
        this.Log("\n");
        
        Database = new SQLiteAsyncConnection(databasePath, DatabaseConfiguration.Flags);

        _ = await Database.CreateTableAsync<WordDbItem>();
        _ = await Database.CreateTableAsync<SentenceDbItem>();
        _ = await Database.CreateTableAsync<TagDbItem>();
        _ = await Database.CreateTableAsync<ActivityDbItem>();
    }

    public async Task<List<T>> Select<T>() where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().ToListAsync();
    }

    public async Task<int> GetWordsPracticedToday()
    {
        await Init();
        var words = await Database.Table<WordDbItem>().ToListAsync();

        return words
            .Where(w => w.RepeatTime != null)
            .Where(w => w.RepeatTime.Value.Date == DateTime.Today)
            .Count();
    }

    public async Task<List<T>> GetItemsAsync<T>() where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().ToListAsync();
    }

    public async Task<T> GetItemAsync<T>(Expression<Func<T, bool>> expr) where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().Where(expr).FirstOrDefaultAsync();
    }

    public async Task<T> GetItemAsync<T>(int id) where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync<T>(T item) where T : DbItem, new()
    {
        await Init();
        if (item.Id != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync<T>(T item) where T : DbItem, new()
    {
        await Init();
        return await Database.DeleteAsync(item);
    }

    public async Task DeleteAllItems<T>() where T : DbItem, new()
    {
        await Init();
        await Database.DeleteAllAsync<T>();
    }

    public async Task<SentenceDbItem> SaveSentenceAsync(SentenceDbItem item)
    {
        await SaveItemAsync(item);

        return await GetItemAsync<SentenceDbItem>(x => x.Sentence == item.Sentence);
    }

    public async Task<WordDbItem> SaveWordAsync(WordDbItem item)
    {
        await SaveItemAsync(item);

        return await GetItemAsync<WordDbItem>(x => x.Word == item.Word);
    }

    public async Task LogActivity(ActivityDbItem item)
    {
        await SaveItemAsync(item);
    }

    public async Task<IEnumerable<ActivityDbItem>> GetLogsForWord(int id)
    {
        var logs = await GetItemsAsync<ActivityDbItem>();
        return logs.Where(x => x.WordId == id).ToList();
    }
}
