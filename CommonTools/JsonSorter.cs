using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CommonTools
{
    /// <summary>
    /// 根据json信息的key值排序，常用于接口调用中的签名操作。
    /// </summary>
    public static class JsonSorter
    {

        public static string Sort(string jsonString, Direction direction)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(jsonString);
            Sort(jObj, direction);
            return jObj.ToString();
        }

        public static void Sort(JObject jObj, Direction direction)
        {
            var props = jObj.Properties().ToList();
            foreach (var prop in props)
            {
                prop.Remove();
            }

            var sortedList = direction == Direction.ASC ? props.OrderBy(p => p.Name) : props.OrderByDescending(p => p.Name);
            foreach (var prop in sortedList)
            {
                jObj.Add(prop);
                if (prop.Value is JObject)
                    Sort((JObject)prop.Value, direction);
            }
        }


    }

    public enum Direction
    {
        ASC, DESC
    }
}
