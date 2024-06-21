using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoListWebApp.Models;
using ToDoListWebApp.Services;
using ToDoListWebApp.ViewModels;

namespace ToDoListWebApp.Controllers;

public class ToDoListController : Controller
{
    private readonly ToDoListContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ToDoTaskService _toDoTaskService;
    public ToDoListController(ToDoListContext context, UserManager<User> userManager, ToDoTaskService toDoTaskService)
    {
        _context = context;
        _userManager = userManager;
        _toDoTaskService = toDoTaskService;
    }
    [Authorize]
    public IActionResult Index(
        string? title,
        string? description,
        DateTimeOffset? creationDateAfter, 
        DateTimeOffset? creationDateBefore, 
        ToDoTaskPriority? priority, 
        ToDoTaskStatus? status, 
        SortState sortOrder = SortState.IdAsc,
        ToDoTaskType taskType = ToDoTaskType.All)
    {
        ViewBag.IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
        ViewBag.TitleSort = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;
        ViewBag.PrioritySort = sortOrder == SortState.PriorityAsc ? SortState.PriorityDesc : SortState.PriorityAsc;
        ViewBag.StatusSort = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
        ViewBag.CreationDateSort = sortOrder == SortState.CreationDateAsc ? SortState.CreationDateDesc : SortState.CreationDateAsc;
        IQueryable<ToDoTask> query = _context.ToDoList;
        var userId = int.Parse(_userManager.GetUserId(User)!);
        switch(taskType)
        {
            case ToDoTaskType.CreatedByMe:
                query = query.Where(x => x.CreatorId == userId);
                break;
            case ToDoTaskType.TakenByMe:
                query = query.Where(x => x.PerformerId == userId);
                break;
            case ToDoTaskType.Free:
                query = query.Where(x => x.PerformerId == null);
                break;
            default:
                break;
        }
        switch (sortOrder)
        {
            case SortState.IdDesc:
                query = query.OrderByDescending(s => s.Id);
                break;
            case SortState.TitleAsc:
                query = query.OrderBy(s => s.Title);
                break;
            case SortState.TitleDesc:
                query = query.OrderByDescending(s => s.Title);
                break;
            case SortState.PriorityAsc:
                query = query.OrderBy(s => s.Priority);
                break;
            case SortState.PriorityDesc:
                query = query.OrderByDescending(s => s.Priority);
                break;
            case SortState.StatusAsc:
                query = query.OrderBy(s => s.Status);
                break;
            case SortState.StatusDesc:
                query = query.OrderByDescending(s => s.Status);
                break;
            case SortState.CreationDateAsc:
                query = query.OrderBy(s => s.CreatedAt);
                break;
            case SortState.CreationDateDesc:
                query = query.OrderByDescending(s => s.CreatedAt);
                break;
            default:
                query = query.OrderBy(s => s.Id);
                break;
        }

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }
        if (!string.IsNullOrEmpty(description))
        {
            var words = description.Split(' ');
            foreach(var word in words)
            {
                query = query.Where(p => p.Description.Contains(word));
            }
        }
        if (creationDateBefore.HasValue)
        {
            query = query.Where(p => p.CreatedAt <= creationDateBefore.Value);
        }
        if (creationDateAfter.HasValue)
        {
            query = query.Where(p => p.CreatedAt >= creationDateAfter.Value);
        }
        if (priority.HasValue)
        {
            query = query.Where(p => p.Priority == priority);
        }
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status);
        }

        IndexViewModel viewModel = new()
        {
            Title = title,
            CreationDateAfter = creationDateAfter,
            CreationDateBefore = creationDateBefore,
            Priority = priority,
            Status = status,
            ToDoList = query.ToList()
        };
        ViewBag.UserId = _userManager.GetUserId(User);
        return View(viewModel);
    }
    [Authorize]
    [ResponseCache(CacheProfileName = "Caching")]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ToDoTask toDoTask)
    {
        if(ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            toDoTask.CreatorId = user.Id;
            await _toDoTaskService.AddTask(toDoTask);
            return RedirectToAction("Index");
        }
        return View(toDoTask);
    }
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        if(User.IsInRole("admin") || _context.ToDoList.Any(e => e.Id == id && e.CreatorId == int.Parse(_userManager.GetUserId(User))))
        {
            await _toDoTaskService.RemoveTask(id);
        }
        return RedirectToAction("Index");
    }
    [Authorize]
    public async Task<IActionResult> SetInProgress(int id, string redirectUrl)
    {
        var task = await _toDoTaskService.GetTask(id);
        if (task is not null && task.Status == ToDoTaskStatus.New)
        {
            var user = await _userManager.GetUserAsync(User);
            task.Status = ToDoTaskStatus.InProgress;
            task.PerformerId = user.Id;
            await _toDoTaskService.UpdateTask(task);
        }
        if (!string.IsNullOrEmpty(redirectUrl) && Url.IsLocalUrl(redirectUrl))
        {
            return Redirect(redirectUrl);
        }
        return RedirectToAction("Index");
    }
    [Authorize]
    public async Task<IActionResult> SetDone(int id, string redirectUrl)
    {
        if(User.IsInRole("admin") || _context.ToDoList.Any(x => x.Id == id && x.PerformerId == int.Parse(_userManager.GetUserId(User))))
        {
            var task = await _toDoTaskService.GetTask(id);
            if (task is not null && task.Status == ToDoTaskStatus.InProgress)
            {
                task.Status = ToDoTaskStatus.Done;
                task.DoneAt = DateTimeOffset.UtcNow;
                await _toDoTaskService.UpdateTask(task);
            }
        }
        if(!string.IsNullOrEmpty(redirectUrl) && Url.IsLocalUrl(redirectUrl))
        {
            return Redirect(redirectUrl);
        }
        return RedirectToAction("Index");
    }
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var task = await _toDoTaskService.GetTask(id);
        if(task is not null)
        {
            ViewBag.UserId = _userManager.GetUserId(User);
            return View(task);
        }
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
