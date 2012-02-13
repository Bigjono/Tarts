using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace Bronson.Utils
{
    public class Templating
    {

        public static string ProcessVelocityTemplate(string content, object model)
        {
            var ctx = new Dictionary<string, object> {{"model", model}};
            return ProcessVelocityTemplate(content, ctx);
        }

        public static string ProcessVelocityTemplate(string content, Dictionary<string, object> dict)
        {
            var engine = new VelocityEngine();
            engine.Init();

            var ctx = new Hashtable();
            foreach (string key in dict.Keys)
                ctx.Add(key, dict[key]);

            var context = new VelocityContext(ctx);

            using (var writer = new StringWriter())
            {
                engine.Evaluate(context, writer, "", content);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
