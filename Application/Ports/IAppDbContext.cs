using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Ports;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Todo> Todos { get; }
}
