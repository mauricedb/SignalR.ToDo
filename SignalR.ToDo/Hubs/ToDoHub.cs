using Microsoft.AspNet.SignalR;
using SignalR.ToDo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security;

namespace SignalR.ToDo.Hubs
{
    [Authorize]
    public class ToDoHub : Hub
    {
        public IEnumerable<TodoListDto> GetTodoLists()
        {
            var userName = Context.User.Identity.Name;
            using (var db = new TodoItemContext())
            {
                return db.TodoLists.Include("Todos")
                    .Where(u => u.UserId == userName)
                    .OrderByDescending(u => u.TodoListId)
                    .AsEnumerable()
                    .Select(todoList => new TodoListDto(todoList))
                    .ToArray();
            }
        }

        public void PutTodoList(TodoListDto todoListDto)
        {
            var context = new ValidationContext(todoListDto, null, null);
            // ToDo: Get the actual error message to the client
            Validator.ValidateObject(todoListDto, context);

            TodoList todoList = todoListDto.ToEntity();

            using (var db = new TodoItemContext())
            {
                if (db.Entry(todoList).Entity.UserId != Context.User.Identity.Name)
                {
                    throw new SecurityException("Trying to modify a record that does not belong to the user");
                }

                db.Entry(todoList).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public TodoListDto PostTodoList(TodoListDto todoListDto)
        {
            var context = new ValidationContext(todoListDto, null, null);
            // ToDo: Get the actual error message to the client
            Validator.ValidateObject(todoListDto, context);

            todoListDto.UserId = Context.User.Identity.Name;
            TodoList todoList = todoListDto.ToEntity();

            using (var db = new TodoItemContext())
            {
                db.TodoLists.Add(todoList);
                db.SaveChanges();
                todoListDto.TodoListId = todoList.TodoListId;

                return todoListDto;
            }
        }

        public void DeleteTodoList(int id)
        {
            using (var db = new TodoItemContext())
            {
                TodoList todoList = db.TodoLists.Find(id);
                if (todoList == null)
                {
                    throw new ArgumentException("Not found", "id");
                }

                if (db.Entry(todoList).Entity.UserId != Context.User.Identity.Name)
                {
                    throw new SecurityException("Trying to delete a record that does not belong to the user");
                }

                db.TodoLists.Remove(todoList);
                db.SaveChanges();
            }
        }
    }
}