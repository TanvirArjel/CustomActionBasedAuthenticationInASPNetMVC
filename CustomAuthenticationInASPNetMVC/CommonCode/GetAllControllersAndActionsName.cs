using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CustomAuthenticationInASPNetMVC.CommonCode
{
    public static class GetAllControllersAndActionsName
    {
        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public static List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name.Replace("Controller", "")));
            return controllerNames.OrderBy(x =>x).ToList();
        }


        public static List<string> ActionNames(string controllerName)
        {
            var types =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where typeof(IController).IsAssignableFrom(t) &&
                      string.Equals(controllerName + "Controller", t.Name, StringComparison.OrdinalIgnoreCase)
                select t;

            var controllerType = types.FirstOrDefault();

            if (controllerType == null)
            {
                return Enumerable.Empty<string>().ToList();
            }
            return new ReflectedControllerDescriptor(controllerType)
                .GetCanonicalActions().Select(x => x.ActionName)
                .ToList();
        }
    }
}