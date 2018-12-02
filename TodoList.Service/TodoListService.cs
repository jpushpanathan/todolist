
using System.Collections.Generic;
using TodoList.Core.Interfaces;
using TodoList.DomainModel;

namespace TodoList.Service
{
    public class TodoListService : ITodoListService
    {
        private ITodoListDAL todoListDAL;
        private List<Todo> todos = new List<Todo>();

        public TodoListService(ITodoListDAL todoListDAL)
        {
            this.todoListDAL = todoListDAL;
        }
        public Todo AddTodo(Todo todo)
        {
            return todoListDAL.AddTodo(todo);
        }

        public IEnumerable<Todo> GetTodos()
        {
            return todoListDAL.GetTodos();
        }

        public bool DeleteTodos()
        {
            return todoListDAL.DeleteTodos();
        }

        public bool DeleteTodo(int todoId)
        {
            return todoListDAL.DeleteTodo(todoId);
        }


        public bool UpdateTodo(Todo todo)
        {
            return todoListDAL.UpdateTodo(todo);
        }

        public bool MarkAllAsComplete()
        {
            return todoListDAL.MarkAllAsComplete();
        }
    }
}
