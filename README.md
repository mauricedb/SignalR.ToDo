SignalR.ToDo
============

A demo project showing how to do the ASP.NET SPA template using [SignalR](http://signalr.net/).

The application is based on the [ASP.NET Single Page Application](http://www.asp.net/single-page-application) applciation template. The browser server communication, in the original template done using WebAPI, has been replaced with SignalR. Besides the client browser requesting and saving data through the SignalR hub all changes are also pushed to other browsers the same user is logged in. This means that the other browsers will actively update themselfs as soon as change is made.
