using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraffiGo.Modeles.Data
{
    static class Jsonify
    {
        public static string ToJson(object o)
        {

            StringBuilder s = new StringBuilder();

            foreach(var a in o.GetType().GetProperties())
            {
                if(a.GetType().GetProperties().Length > 1)
                {

                }
            }

            return s.ToString();
        }
    }
}
