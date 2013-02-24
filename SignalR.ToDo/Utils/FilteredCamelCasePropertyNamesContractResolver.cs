using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SignalR.ToDo.Utils
{
    public class FilteredCamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public FilteredCamelCasePropertyNamesContractResolver()
        {
            AssembliesToInclude = new HashSet<Assembly>();
            TypesToInclude = new HashSet<Type>();
        }
        /// <summary>
        /// Identifies assemblies to include from camel-casing
        /// </summary>
        public HashSet<Assembly> AssembliesToInclude { get; set; }
        /// <summary>
        /// Identifies types to include from camel-casing 
        /// </summary>
        public HashSet<Type> TypesToInclude { get; set; } 

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            Type declaringType = member.DeclaringType;
            if (declaringType != null && (TypesToInclude != null && (TypesToInclude.Contains(declaringType) || AssembliesToInclude.Contains(declaringType.Assembly))))
            {
                jsonProperty.PropertyName = jsonProperty.PropertyName.ToCamelCase();
            }
            return jsonProperty;
        }
    }
}