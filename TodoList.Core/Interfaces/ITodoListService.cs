using System.Collections.Generic;
using TodoList.DomainModel;

namespace TodoList.Core.Interfaces
{
    public interface ITodoListService
    {
        Todo AddTodo(Todo todo);
        IEnumerable<Todo> GetTodos();
        bool UpdateTodo(Todo todo);
        bool MarkAllAsComplete();
        bool DeleteTodos();
        bool DeleteTodo(int todoId);
    }
}
