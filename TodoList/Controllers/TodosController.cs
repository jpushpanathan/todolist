using System.Collections.Generic;
using System.Web.Http;
using TodoList.Core.Interfaces;
using TodoList.DomainModel;

namespace TodoList.Controllers
{
    public class TodosController : ApiController

    {
        private readonly ITodoListService todoListService;

        public TodosController(ITodoListService todoListService)
        {
            this.todoListService = todoListService;
        }

        public IHttpActionResult GetTodos()
        {
            var result = this.todoListService.GetTodos();

            return Ok(result);
        }

        public IHttpActionResult Post(Todo todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = this.todoListService.AddTodo(todo);
            return Ok(result);
        }

        public IHttpActionResult Put(Todo todo)
        {
            var result = false;

            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            if (todo == null)
            {
                result= this.todoListService.MarkAllAsComplete();
            }
            else
            {
                result= this.todoListService.UpdateTodo(todo);
            }
            return Ok(result);
        }

        public IHttpActionResult Delete(int? todoId = null)
        {
            var result = false;
            if (todoId.HasValue == false)
            {
                result =this.todoListService.DeleteTodos();
            }
            else
            {
                result= this.todoListService.DeleteTodo(todoId.Value);
            }
            return Ok(result);
        }
    }
}