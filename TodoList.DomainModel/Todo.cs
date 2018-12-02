using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.DomainModel
{
    public class Todo
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public TodoStatus Status { get; set; }
    }
}
