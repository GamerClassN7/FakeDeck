using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FakeeDeck.Class
{
    internal class AbstractionHelper
    {
        public static Type? resolvType(string className)
        {
            string cleanClass = "FakeeDeck.ButtonType." + className.Trim('/');

            Type? buttonClass = Type.GetType(cleanClass, true);

            if (buttonClass is null)
                return null;

            return buttonClass;
        }

        public static string getButtonVisual(JsonElement button)
        {
            string calssName = button.GetProperty("function").ToString();
            MethodInfo? renderMethod = AbstractionHelper.resolvType(calssName).GetMethod("getButton");
            ParameterInfo[] pars = renderMethod.GetParameters();
            List<object> parameters = new List<object>();

            foreach (ParameterInfo p in pars)
            {
                JsonElement parameter = button.GetProperty("parameters").EnumerateArray().SingleOrDefault(item => item.GetProperty("name").ToString() == p.Name);
                parameters.Insert(p.Position, parameter.GetProperty("value").ToString());
            }

            return renderMethod.Invoke(null, [.. parameters]).ToString();
        }
    }
}
