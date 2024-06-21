using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Services;

public class ToDoTaskService
{
    private ToDoListContext db;
    private IMemoryCache cache;
    public ToDoTaskService(ToDoListContext context, IMemoryCache memoryCache)
    {
        db = context;
        cache = memoryCache;
    }
    public async Task<IEnumerable<ToDoTask>> GetTasks()
    {
        return await db.ToDoList.ToListAsync();
    }
    public async Task<ToDoTask?> GetTask(int id)
    {
        if (!cache.TryGetValue(id, out ToDoTask? task))
        {
            task = await db.ToDoList.Include(e => e.Creator).Include(e => e.Performer).FirstOrDefaultAsync(p => p.Id == id);
            if (task != null)
            {
                cache.Set(task.Id, task,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
        }
        return task;
    }
    public async Task AddTask(ToDoTask task)
    {
        db.ToDoList.Add(task);
        int n = await db.SaveChangesAsync();
        if (n > 0)
        {
            cache.Set(task.Id, task, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }
    }
    public async Task UpdateTask(ToDoTask task)
    {
        db.ToDoList.Update(task);
        int n = await db.SaveChangesAsync();
        if (n > 0)
        {
            cache.Set(task.Id, task, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }
    }
    public async Task RemoveTask(ToDoTask task)
    {
        db.ToDoList.Remove(task);
        int n = await db.SaveChangesAsync();
        if (n > 0)
        {
            cache.Remove(task.Id);
        }
    }
    public async Task RemoveTask(int id)
    {
        int n = await db.ToDoList.Where(e => e.Id == id).ExecuteDeleteAsync();
        await db.SaveChangesAsync();
        if(n > 0)
        {
            cache.Remove(id);
        }
    }
}
