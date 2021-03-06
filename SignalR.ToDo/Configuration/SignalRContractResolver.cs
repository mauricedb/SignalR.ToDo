﻿using System;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace SignalR.ToDo.Configuration
{
    /// <summary>
    /// From <a href="https://github.com/SignalR/SignalR/issues/500">SignalR Github</a>
    /// </summary>
    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly _assembly;
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            _assembly = typeof(Connection).Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.Equals(_assembly))
            {
                return _defaultContractSerializer.ResolveContract(type);
            }

            return _camelCaseContractResolver.ResolveContract(type);
        }
    }
}