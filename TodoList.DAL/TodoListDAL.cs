using System.Collections.Generic;
using System.Linq;
using TodoList.Core.Interfaces;
using TodoList.DomainModel;

namespace TodoList.DAL
{
    public class TodoListDAL : ITodoListDAL
    {
        private readonly IMemoryCacheEx memoryCache;
        public TodoListDAL(IMemoryCacheEx memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        private const string todoListKey = "todoListKey";

        public Todo AddTodo(Todo todo)
        {
            var todoCounter = 0;
            if(this.memoryCache.Exists(todoListKey) == false)
            {
                todo.Id = todoCounter + 1;
                this.memoryCache.Add<List<Todo>>(todoListKey, new List<Todo> { todo });
                return todo;
            }

            var todos = this.memoryCache.Get<List<Todo>>(todoListKey);
            todo.Id = todos.OrderByDescending(x => x.Id).First().Id + 1;
            todos.Add(todo);
            this.memoryCache.Update<List<Todo>>(todoListKey, todos);
            return todo;
        }

        public bool DeleteTodos()
        {
            if (this.memoryCache.Exists(todoListKey) == false)
                return false;
            this.memoryCache.Remove(todoListKey);
            return true;
        }
        
        public bool DeleteTodo(int todoId)
        {
            if (this.memoryCache.Exists(todoListKey) == false)
                return false;

            var todos = this.memoryCache.Get<List<Todo>>(todoListKey);
            todos.RemoveAll(x => x.Id == todoId);
            if (todos.Count == 0)
            {
                this.memoryCache.Remove(todoListKey);
            }
            else
            {
                this.memoryCache.Update<List<Todo>>(todoListKey, todos);
            }
            return true;
        }

        public IEnumerable<Todo> GetTodos()
        {
            if (this.memoryCache.Exists(todoListKey) == false)
                return null;

            return this.memoryCache.Get<IEnumerable<Todo>>(todoListKey);
        }

        public bool MarkAllAsComplete()
        {
            if (this.memoryCache.Exists(todoListKey) == false)
                return false;

            var todos = this.memoryCache.Get<List<Todo>>(todoListKey);
            todos.All(x => { x.Status = TodoStatus.Complete; return true; });
            this.memoryCache.Update<List<Todo>>(todoListKey, todos);
            return true;
        }

        public bool UpdateTodo(Todo todo)
        {
            if (this.memoryCache.Exists(todoListKey) == false)
                return false;

            var todos = this.memoryCache.Get<List<Todo>>(todoListKey);
            var oldTodoIdx = todos.FindIndex(x => x.Id == todo.Id);
            todos[oldTodoIdx] = todo;

            this.memoryCache.Update<List<Todo>>(todoListKey, todos);
            return true;
        }
    }
}
