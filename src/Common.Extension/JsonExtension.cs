using Newtonsoft.Json;

namespace Common.Extension
{
    public static class JsonExtension
    {    
        /// <summary>
         /// Convert object to json
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
        public static string TrySerializeObject(this object obj)
        {
            string result = string.Empty;

            if (obj is not null)
            {
                result = JsonConvert.SerializeObject(obj);
            }

            return result;
        }
    }
}
