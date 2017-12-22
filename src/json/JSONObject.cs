using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace qcloudsms_csharp.json
{
    public class JSONObjectBuilder
    {
        private JObject obj;

        public JSONObjectBuilder()
        {
            obj = new JObject();
        }

        public JSONObjectBuilder Put(string name, object value)
        {
            obj.Add(name, JToken.FromObject(value));
            return this;
        }

        public JSONObjectBuilder PutArray(string name, object[] values)
        {
            obj.Add(name, new JArray(values));
            return this;
        }

        public JSONObjectBuilder PutArray(string name, JArray values)
        {
            obj.Add(name, values);
            return this;
        }

        public JObject Build()
        {
            return obj;
        }
    }

    public class JSONArrayBuilder
    {
        private JArray arr;

        public JSONArrayBuilder()
        {
            arr = new JArray();
        }

        public JSONArrayBuilder Put(object value)
        {
            arr.Add(value);
            return this;
        }

        public JArray Build()
        {
            return arr;
        }
    }
}