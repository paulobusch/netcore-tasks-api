using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks.IntegrationTests._Common.Tools
{
    public static class Mutation
    {
        public static T Mutate<T>(T model, string property, object value) where T : class
        {
            var merge = new Dictionary<string, object> { { property, value } };
            return Mutate(model, merge);
        }

        public static T Mutate<T>(T model, Dictionary<string, object> propertyValues) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(p => p.CanRead);
            var generated = Activator.CreateInstance(type, true) as T;
            foreach (var property in properties)
            {
                if (propertyValues.ContainsKey(property.Name))
                {
                    var valueMerge = propertyValues[property.Name];
                    if (valueMerge != null) property.SetValue(generated, valueMerge);
                }
                else
                {
                    var valueModel = property.GetValue(model, null);
                    if (valueModel != null) property.SetValue(generated, valueModel);
                }
            }
            return generated;
        }
    }
}
