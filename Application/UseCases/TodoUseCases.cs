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

    public Task<List<Todo>> GetAllActive()
    {
        return _dbContext.Todos.Where(m => m.CompletedDate != default).ToListAsync();
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
}
