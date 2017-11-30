using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CustomActionBasedAuthenticationInASPNetMVC.CommonCode
{
    public static class GetAllControllerAndActionNames
    {
        static readonly Assembly myAssembly = Assembly.GetExecutingAssembly();
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

        public static List<string> GetControllerNames2()
        {
            List<string> controllerNames = new List<string>();

            
            var types = myAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).OrderBy(x => x.Name).ToList();
            types.ForEach(type => controllerNames.Add(type.Name.Replace("Controller","")));
            return controllerNames;
        }

        public static List<string> GetActionNamesByController2(string controllerName)
        {
            List<string> actionNames = new List<string>();
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var controllerMethods = myAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Name == controllerName + "Controller").SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)).ToList();

            controllerMethods.ForEach(type => actionNames.Add(type.Name));
            return actionNames;
        }


        public static List<string> GetActionNamesByController(string controllerName)
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