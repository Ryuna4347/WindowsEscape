using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace KZLib.Data.Player
{
    public abstract class PLHandler<T> : IPLHandler where T : PLDataBase
    {
        protected T DB { get; private set; }

        public void InitDataBase()
        {
            DB.Init();
        }

        public void SetDataBase(object _param)
        {
            DB = (T) _param;
        }

        public JObject ParseDataBase()
        {
            return JObject.FromObject(DB);
        }
    }

    public interface IPLHandler
    {
        void SetDataBase(object _param);
        void InitDataBase();
        JObject ParseDataBase();
    }

    public abstract class PLDataBase
    {
        public abstract void Init();
    }
}