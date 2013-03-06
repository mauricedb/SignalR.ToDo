window.todoApp = window.todoApp || {};

window.todoApp.datacontext = (function () {

    var datacontext = {
        getTodoLists: getTodoLists,
        createTodoItem: createTodoItem,
        createTodoList: createTodoList,
        saveNewTodoItem: saveNewTodoItem,
        saveNewTodoList: saveNewTodoList,
        saveChangedTodoItem: saveChangedTodoItem,
        saveChangedTodoList: saveChangedTodoList,
        deleteTodoItem: deleteTodoItem,
        deleteTodoList: deleteTodoList
    };

    var todoListHub = $.connection.todoListHub;
    var todoHub = $.connection.todoHub;
    
    return datacontext;

    function createTodoItem(data) {
        return new datacontext.todoItem(data); // todoItem is injected by todo.model.js
    }
    function createTodoList(data) {
        return new datacontext.todoList(data); // todoList is injected by todo.model.js
    }


    function getTodoLists(todoListsObservable, errorObservable) {
        return todoListHub.server.getTodoLists()
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedTodoLists = $.map(data, function (list) { return new createTodoList(list); });
            todoListsObservable(mappedTodoLists);
        }

        function getFailed() {
            errorObservable("Error retrieving todo lists.");
        }
    }

    function saveNewTodoList(todoList) {
        clearErrorMessage(todoList);
        return todoListHub.server.postTodoList(ko.toJS(todoList))
            .done(function (result) {
                todoList.todoListId = result.todoListId;
                todoList.userId = result.userId;
            })
            .fail(function () {
                todoList.errorMessage("Error adding a new todo list.");
            });
    }

    function saveChangedTodoList(todoList) {
        clearErrorMessage(todoList);
        return todoListHub.server.putTodoList(ko.toJS(todoList))
            .fail(function () {
                todoList.errorMessage("Error updating the todo list title. Please make sure it is non-empty.");
            });
    }

    function deleteTodoList(todoList) {
        return todoListHub.server.deleteTodoList(todoList.todoListId)
            .fail(function () {
                todoList.errorMessage("Error removing todo list.");
            });
    }



    function saveNewTodoItem(todoItem) {
        clearErrorMessage(todoItem);
        //return ajaxRequest("post", todoItemUrl(), todoItem)
        return todoHub.server.postTodoItem(ko.toJS(todoItem))
            .done(function (result) {
                todoItem.todoItemId = result.todoItemId;
            })
            .fail(function () {
                todoItem.errorMessage("Error adding a new todo item.");
            });
    }
    function deleteTodoItem(todoItem) {
        //return ajaxRequest("delete", todoItemUrl(todoItem.todoItemId))
        return todoHub.server.deleteTodoItem(ko.toJS(todoItem.todoItemId))
            .fail(function () {
                todoItem.errorMessage("Error removing todo item.");
            });
    }
    function saveChangedTodoItem(todoItem) {
        clearErrorMessage(todoItem);
        //return ajaxRequest("put", todoItemUrl(todoItem.todoItemId), todoItem, "text")
        return todoHub.server.putTodoItem(ko.toJS(todoItem))
            .fail(function () {
                todoItem.errorMessage("Error updating todo item.");
            });
    }

    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
    //function ajaxRequest(type, url, data, dataType) { // Ajax helper
    //    var options = {
    //        dataType: dataType || "json",
    //        contentType: "application/json",
    //        cache: false,
    //        type: type,
    //        data: data ? data.toJson() : null
    //    };
    //    var antiForgeryToken = $("#antiForgeryToken").val();
    //    if (antiForgeryToken) {
    //        options.headers = {
    //            'RequestVerificationToken': antiForgeryToken
    //        }
    //    }
    //    return $.ajax(url, options);
    //}
    // routes
    //function todoListUrl(id) { return "/api/todolist/" + (id || ""); }
    //function todoItemUrl(id) { return "/api/todo/" + (id || ""); }

})();