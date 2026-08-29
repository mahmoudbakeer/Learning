using System.Diagnostics.Contracts;

namespace CQRSInAction.Domain.Todos;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public bool Completed { get; set; }

}