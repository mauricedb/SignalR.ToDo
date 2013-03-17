using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalR.ToDo.Models;

namespace SignalR.ToDo.Hubs
{
    [Authorize]
    public class TodoHub : Hub
    {
        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnected();
        }


        // PUT api/Todo/5
        public void PutTodoItem(TodoItemDto todoItemDto)
        {
            var context = new ValidationContext(todoItemDto, null, null);
            // ToDo: Get the actual error message to the client
            Validator.ValidateObject(todoItemDto, context);

            using (var db = new TodoItemContext())
            {
                TodoItem todoItem = todoItemDto.ToEntity();
                TodoList todoList = db.TodoLists.Find(todoItem.TodoListId);
                if (todoList == null)
                {
                    throw new InvalidOperationException();
                }

                AuthenticateUser(todoList);

                // Need to detach to avoid duplicate primary key exception when SaveChanges is called
                db.Entry(todoList).State = EntityState.Detached;
                db.Entry(todoItem).State = EntityState.Modified;

                db.SaveChanges();

                Clients.OthersInGroup(Context.User.Identity.Name).TodoItemUpdated(todoItemDto);
            }
        }

        // POST api/Todo
        public TodoItemDto PostTodoItem(TodoItemDto todoItemDto)
        {
            var context = new ValidationContext(todoItemDto, null, null);
            // ToDo: Get the actual error message to the client
            Validator.ValidateObject(todoItemDto, context);

            using (var db = new TodoItemContext())
            {
                TodoList todoList = db.TodoLists.Find(todoItemDto.TodoListId);
                if (todoList == null)
                {
                    throw new InvalidOperationException();
                }

                AuthenticateUser(todoList);

                TodoItem todoItem = todoItemDto.ToEntity();

                // Need to detach to avoid loop reference exception during JSON serialization
                db.Entry(todoList).State = EntityState.Detached;
                db.TodoItems.Add(todoItem);
                db.SaveChanges();
                todoItemDto.TodoItemId = todoItem.TodoItemId;

                Clients.OthersInGroup(Context.User.Identity.Name).TodoItemUpdated(todoItemDto);

                return todoItemDto;
            }
        }

        // DELETE api/Todo/5

        public TodoItemDto DeleteTodoItem(int id)
        {
            using (var db = new TodoItemContext())
            {
                TodoItem todoItem = db.TodoItems.Find(id);
                if (todoItem == null)
                {
                    throw new InvalidOperationException();
                }

                AuthenticateUser(db.Entry(todoItem.TodoList).Entity);

                var todoItemDto = new TodoItemDto(todoItem);
                db.TodoItems.Remove(todoItem);

                db.SaveChanges();

                Clients.OthersInGroup(Context.User.Identity.Name).TodoItemDeleted(todoItem.TodoListId, todoItem.TodoItemId);

                return todoItemDto;
            }
        }

        private void AuthenticateUser(TodoList todoList)
        {
            if (todoList.UserId != Context.User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new SecurityException("Trying to modify a record that does not belong to the user");
            }
        }
    }
}