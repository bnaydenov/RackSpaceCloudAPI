using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.Collections;

namespace RackSpaceCloudServersAPI
{
    internal static class ExpandoObjectExtensions
    {
        internal static string ToJson(this ExpandoObject expando)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StringBuilder json = new StringBuilder();
            List<string> keyPairs = new List<string>();
            IDictionary<string, object> dictionary = expando as IDictionary<string, object>;
            json.Append("{");

            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                if (pair.Value is ExpandoObject)
                {
                    keyPairs.Add(String.Format(@"""{0}"": {1}", pair.Key, (pair.Value as ExpandoObject).ToJson()));
                }
                else
                {
                    keyPairs.Add(String.Format(@"""{0}"": {1}", pair.Key, serializer.Serialize(pair.Value)));
                }
            }

            json.Append(String.Join(",", keyPairs.ToArray()));
            json.Append("}");

            return json.ToString();
        }

        internal static ExpandoObject ToExpando(this string json)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IDictionary<string, object> dictionary = serializer.Deserialize<IDictionary<string, object>>(json);
            return dictionary.Expando();

        }


        internal static ExpandoObject Expando(this IDictionary<string, object> dictionary)
        {
            ExpandoObject expandoObject = new ExpandoObject();
            IDictionary<string, object> objects = expandoObject;

            foreach (var item in dictionary)
            {
                bool processed = false;

                if (item.Value is IDictionary<string, object>)
                {
                    objects.Add(item.Key, Expando((IDictionary<string, object>)item.Value));
                    processed = true;
                }
                else if (item.Value is ICollection)
                {
                    List<object> itemList = new List<object>();

                    foreach (var item2 in (ICollection)item.Value)

                        if (item2 is IDictionary<string, object>)
                            itemList.Add(Expando((IDictionary<string, object>)item2));
                        else
                            itemList.Add(Expando(new Dictionary<string, object> { { "Unknown", item2 } }));

                    if (itemList.Count > 0)
                    {
                        objects.Add(item.Key, itemList);
                        processed = true;
                    }
                }

                if (!processed)
                    objects.Add(item);
            }

            return expandoObject;
        }
    }

    
}
