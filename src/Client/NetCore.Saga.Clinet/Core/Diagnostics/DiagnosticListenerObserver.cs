using System.Diagnostics;
using Kaytune.Crm.Core.Abstraction.Diagnostics;

namespace Kaytune.Crm.Core.Core.Diagnostics
{
    /// <summary>
    /// DiagnosticListenerObserver
    /// </summary>
    public class DiagnosticListenerObserver: IObserver<DiagnosticListener>
    {
        private  IEnumerable<IDiagnosticIntercept> _intercepts;
        /// <summary>
        /// DiagnosticListenerObserver
        /// </summary>
        /// <param name="intercepts"></param>
        public DiagnosticListenerObserver(IEnumerable<IDiagnosticIntercept> intercepts)
        {
            _intercepts = intercepts;
        }
        public void OnCompleted()
        {
         
        }

        public void OnError(Exception error)
        {
           
        }

        public void OnNext(DiagnosticListener diagnosticListener)
        {
           var intercept= _intercepts.FirstOrDefault(c => c.ListenerName == diagnosticListener.Name);
           if (intercept==null)
           {
               return;
           }

           lock (intercept)
           {
               diagnosticListener.Subscribe(new DiagnosticObserverIntercept(intercept)!);
           }
        }
    }
}
