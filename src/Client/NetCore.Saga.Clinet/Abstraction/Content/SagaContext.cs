using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaytune.Crm.Saga.Abstraction.Content
{
    public class SagaContext
    {
        public static readonly string GlobalTxIdKey = "X-Pack-Global-Transaction-Id";
        public static readonly string LocalTxIdKey = "X-Pack-Local-Transaction-Id";
        private AsyncLocal<string> GlobalTxId = new AsyncLocal<string>();
        private AsyncLocal<string> LocalTxId = new AsyncLocal<string>();

        public SagaContext()
        {

        }

        /// <summary>
        /// NewGlobalId
        /// </summary>
        /// <returns></returns>
        public string NewGlobalId()
        {
            string id=Guid.NewGuid().ToString();
            GlobalTxId.Value = id;
            return id;
        }

        public void SetGlobalId(string id)
        {
            GlobalTxId.Value = id;
        }

        public string GetGlobalId()
        {
            return GlobalTxId.Value;
        }


        public string NewLocalTxId()
        {
            string id = Guid.NewGuid().ToString();
            LocalTxId.Value = id;
            return id;
        }

        public string GetLocalTxId()
        {
            return LocalTxId.Value;
        }


        public void SetLocalTxId(string localTxId)
        {
            LocalTxId.Value = localTxId;
        }
    }
}
