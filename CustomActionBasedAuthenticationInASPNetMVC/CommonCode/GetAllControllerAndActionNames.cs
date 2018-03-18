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
        static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        private static List<Type> GetSubClasses<T>()
        {
            return CurrentAssembly.GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public static List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();

            GetSubClasses<Controller>().ForEach(type => controllerNames.Add(type.Name.Replace("Controller", "")));
            return controllerNames.OrderBy(x =>x).ToList();
        }

        public static List<string> GetControllerNames2()
        {
            List<string> controllerNames = new List<string>();

            
            var types = CurrentAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).OrderBy(x => x.Name).ToList();
            types.ForEach(type => controllerNames.Add(type.Name.Replace("Controller","")));
            return controllerNames;
        }

        public static List<string> GetActionNamesByController(string controllerName)
        {
            List<string> actionNames = new List<string>();
            var controllerMethods = CurrentAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Name == controllerName + "Controller")
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))).ToList();

            controllerMethods.ForEach(type => actionNames.Add(type.Name));
            return actionNames;
        }
    }
}