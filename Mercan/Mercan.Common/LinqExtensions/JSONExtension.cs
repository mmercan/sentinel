using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace System.Linq
{
    public static class JSONExtension
    {
        public static string ToJSON<T>(this T item) where T : new()
        {
            //MemoryStream stream1 = new MemoryStream();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            //ser.WriteObject(stream1, item);
            //stream1.Position = 0;
            //StreamReader sr = new StreamReader(stream1);
            //return sr.ReadToEnd();
            var result = JsonConvert.SerializeObject(item);
            return result;
        }

        public static T FromJSON<T>(this string json)
        {
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            //var msnew = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //T item = (T)ser.ReadObject(msnew);

            T item = JsonConvert.DeserializeObject<T>(json);

            return item;
        }
    }
}
