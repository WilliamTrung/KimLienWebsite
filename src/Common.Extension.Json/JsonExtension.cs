using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public static JsonDocument ToDocument(this object value)
        {
            if (value is string stringValue)
            {
                return JsonDocument.Parse(stringValue);
            }
            return System.Text.Json.JsonSerializer.SerializeToDocument(value);
        }
        /// <summary>
        /// Convert json to object
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TModel TryDeserializeObject<TModel>(this string json, TModel defaultValue = null) where TModel : class
        {
            var result = defaultValue;

            if (!string.IsNullOrWhiteSpace(json))
            {
                result = JsonConvert.DeserializeObject<TModel>(json);
            }

            return result;
        }
        public static TModel TryDeserializeObject<TModel>(this JsonDocument doc)
            where TModel : class
        {
            if (doc is null) return default;
            try
            {
                var result = doc.RootElement.GetRawText().TryDeserializeObject<TModel>();
                return result;
            }
            catch { return default; }
        }

        /// <summary>
        /// Convert object to json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string TrySerializeObjectWithSystemLibrary(this object obj)
        {
            if (obj == null)
                return null;

            var options = new JsonSerializerOptions
            {
                // Bỏ qua cycle references
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }
    }
}
