using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Kaytune.Crm.Saga.Abstraction.Content
{
    /// <summary>
    /// CompensationContext
    /// </summary>
    public class CompoensationContext
    {
        private static Dictionary<string, CompensationContextInternal> _contexts =
            new Dictionary<string, CompensationContextInternal>();

        private readonly IServiceProvider _serviceProvider;
        public CompoensationContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void AddCompensationContext(MethodInfo compensationMethod, Type target)
        {
            _contexts.TryAdd(compensationMethod.Name, new CompensationContextInternal(target, compensationMethod));
        }
        public void Apply(string typeName, string compensationMethod, params byte[] payloads)
        {
            if (!_contexts.TryGetValue(compensationMethod, out var contextInternal))
            {
                var t = Assembly.GetEntryAssembly().GetType(typeName);
                if (t != null)
                {
                    _contexts.TryAdd(compensationMethod, new CompensationContextInternal(t, t.GetMethod(compensationMethod, BindingFlags.NonPublic | BindingFlags.Instance)));
                }
            }
            if (_contexts.TryGetValue(compensationMethod, out contextInternal))
            {

                var classInstance = _serviceProvider.GetService(contextInternal.Target);
                var parameterInfos = contextInternal.MethodInfo.GetParameters();
                var result = JsonSerializer.Deserialize(Encoding.UTF8.GetString(payloads), typeof(object[])) as object[];
                var objects = new object[] { };
                if (result != null)
                {
                    objects = new object[result.Length];
                    for (int index = 0; index < result.Length; index++)
                    {
                        var a = result[index];
                        var value = JsonSerializer.Serialize(a).Substring(2, JsonSerializer.Serialize(result[0]).Length - 4).Replace(@"\", "");
                        objects[index] = JsonConvert.DeserializeObject(value, parameterInfos[0].ParameterType);
                    }
                }

                contextInternal.MethodInfo.Invoke(classInstance, objects);
            }
            else
            {
                throw new NotImplementedException("没有找到方法");
            }
        }
    }

    public class CompensationContextInternal
    {
        public Type Target { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public CompensationContextInternal(Type target, MethodInfo compensationMethod)
        {
            Target = target;
            MethodInfo = compensationMethod;
        }
    }
}
