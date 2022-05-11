using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kaytune.Crm.Core.Abstraction.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Kaytune.Crm.Core.Core.Diagnostics
{
    public class DiagnosticObserverIntercept:IObserver<KeyValuePair<string, object>>
    {
        private readonly IDiagnosticIntercept _diagnosticIntercept;
        private readonly Dictionary<string, string> _diagnosticNameDictionary = new Dictionary<string, string>()
        {
            { "Microsoft.AspNetCore.Hosting.BeginRequest","httpContext"},
            {"System.Net.Http.Request", "Request"},
        };

        private Dictionary<string, MethodInfo> _methods;
        private static readonly object Object = new object();
        /// <summary>
        /// DiagnosticObserverIntercept
        /// </summary>
        /// <param name="diagnosticIntercept"></param>
        public DiagnosticObserverIntercept(IDiagnosticIntercept diagnosticIntercept)
        {
            _diagnosticIntercept = diagnosticIntercept;
            _methods = new Dictionary<string, MethodInfo>();

            LoadMethodInfo();
        }

        private void LoadMethodInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
           var methodInfos= assembly.GetTypes().SelectMany(c => c.GetMethods())
                .Where(c => c.GetCustomAttribute<DiagnosticNameAttribute>() != null).ToList();
            foreach (var methodInfo in methodInfos)
            {
                var name =methodInfo?.GetCustomAttribute<DiagnosticNameAttribute>()?.Name;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                if (_diagnosticNameDictionary.ContainsKey(name))
                {
                    _methods.TryAdd(name, methodInfo);
                }
            }
           
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            lock (Object)
            {
                if (_diagnosticNameDictionary.ContainsKey(value.Key))
                {
                    var propertyInfo = value.Value.GetType().GetProperty(_diagnosticNameDictionary[value.Key]);
                    _methods[value.Key].Invoke(_diagnosticIntercept, new []{ propertyInfo?.GetValue(value.Value) });
                }
            }
        }
    }
}
