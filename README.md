SignalR.ToDo
============

A demo project showing how to do the ASP.NET SPA template using [SignalR](http://signalr.net/).

The application is based on the [ASP.NET Single Page Application](http://www.asp.net/single-page-application) applciation template. The browser server communication, in the original template done using WebAPI, has been replaced with SignalR. Besides the client browser requesting and saving data through the SignalR hub all changes are also pushed to other browsers the same user is logged in. This means that the other browsers will actively update themselfs as soon as change is made.

## Server side changes, the SignalR Hubs

The Hubs folders contains two SignalR Hubs, TodoListHub and TodoHub. The TodoListHub hub is used to retreive and update ToDo lists. The TodoHub is used to update individual ToDo items. As soon as a user connects the user is allso added to a SignalR group with the name of the user. These SignalR groups are used for update notifications to the other browser windows when a change is saved.ttodo.DataContext.js

## Client side changes

The *ToDo.ViewModel.js* file is responsible for starting the SignalR connection. When the connection has been intialized the list of ToDo items is retreived and displayed as normal using [Knockout.js](http://knockoutjs.com).

The biggest changes have been made to *ToDo.DataContext.js*. All REST style request to the server have been replaced with requests to the SignalR Hubs. Besides these requests a number of update notofication functions have been added to update the ViewModel when changes from another browser window where pushed to this browser window. There is also an addition *exceptionHandler* on each Hub to notify the user of any runtime or validation errors on the server. The messages from these *exceptionHandler* are displayed using [Toastr](https://github.com/CodeSeven/toastr).
