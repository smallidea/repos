using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repos.Lib
{
    public class JasonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertToObj<T>(string json)
        {
            if (string.IsNullOrEmpty(json) || json == "{}" || json == "[]")
            {
                return default(T);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            json = json.TrimStart('\"');
            json = json.TrimEnd('\"');
            json = json.Replace("\\", ""); //????
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToStr(object obj)
        {
            //return JsonConvert.SerializeObject(obj);

            var timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, timeConverter); //Formatting.None 防止带有转义字符的json字符串
        }
    }
}
