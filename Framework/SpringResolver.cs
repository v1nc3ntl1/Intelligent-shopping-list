using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context;
using Spring.Context.Support;

namespace Framework
{
    public static class SpringResolver
    {
        public static T GetObject<T>(string id) where T : class
        {
            IApplicationContext context = ContextRegistry.GetContext();
            object obj = context.GetObject(id);
            T castObj = obj as T;
            if (castObj == null)
            {
                castObj = default(T);
                //TODO : Log error
            }
            return castObj;
        }
    }
}
