using Application.Entities;
using Application.Ports;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases;

public class TodoUseCases
{
    private readonly IAppDbContext _dbContext;

    public TodoUseCases(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Todo>> GetAllActive()
    {
        List<Todo> todos = await _dbContext.Todos.Where(m => m.CompletedDate == default).ToListAsync();
        todos = todos.OrderByDescending(m => m.GetUrgency()).ToList();

        return todos;
    }

    public async Task<Result<Todo>> Add(string name, DateTime dueDate = default, Priority priority = default)
    {
        if (name == "") return Result.Fail("name cannot be empty");

        Todo todo = new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            CreatedDate = DateTime.Now,
            DueDate = dueDate,
            Priority = priority,
        };

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        return todo;
    }

    public async Task<Result<Todo>> Complete(string id)
    {
        Todo? todo = await _dbContext.Todos.SingleOrDefaultAsync(m => m.Id == id);
        if (todo == null) return Result.Fail($"todo with id {id} could not be found");

        todo.CompletedDate = DateTime.Now;
        await _dbContext.SaveChangesAsync();

        return todo;
    }
}
