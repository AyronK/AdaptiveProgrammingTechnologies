using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.Presentation.ViewModels
{
    public static class ReflectionExtensions
    {
        public static string GetDescription(this ReflectionElement item)
        {
            if (item.GetType() == typeof(AssemblyInfo))
            {
                var x = (AssemblyInfo)item;
                return x.Name;
            }
            else if (item.GetType() == typeof(NamespaceInfo))
            {
                var x = (NamespaceInfo)item;
                return x.Name;
            }
            else if (item.GetType() == typeof(TypeInfo))
            {
                var x = (TypeInfo)item;
                return x.TypeName;
            }
            else if (item.GetType() == typeof(VarModel))
            {
                var x = (VarModel)item;
                return $"{x.BaseType.TypeName} {x.Name}";
            }
            else if (item.GetType() == typeof(MethodModel))
            {
                var x = (MethodModel)item;
                StringBuilder output = new StringBuilder();

                foreach (string modifier in x.Modifiers)
                    output.Append(modifier + " ");

                output.Append(x.ReturnType.TypeName + " " + x.Name + "(");

                if (x.Parameters.Count > 0)
                {
                    foreach (VarModel parameter in x.Parameters)
                        output.Append(parameter.BaseType.TypeName + " " + parameter.Name + ", ");
                    output.Remove(output.Length - 2, 2);
                }

                output.Append(")");
                return output.ToString();
            }
            return "";
        }


        public static IEnumerable<ReflectionElement> GetChildren(this ReflectionElement item)
        {
            if (item.GetType() == typeof(AssemblyInfo))
            {
                var x = (AssemblyInfo)item;
                return x.Namespaces;
            }
            else if (item.GetType() == typeof(NamespaceInfo))
            {
                var x = (NamespaceInfo)item;
                return x.Classes;
            }
            else if (item.GetType() == typeof(TypeInfo))
            {
                var x = (TypeInfo)item;
                List<ReflectionElement> children = new List<ReflectionElement>();
                children.AddRange(x.Fields);
                children.AddRange(x.Properties);
                children.AddRange(x.Methods);
                children.AddRange(x.Attributes);
                children.AddRange(x.NestedTypes);
                return children;
            }
            else if (item.GetType() == typeof(VarModel))
            {
                var x = (VarModel)item;
                return x.BaseType.GetChildren();
            }
            else if (item.GetType() == typeof(MethodModel))
            {
                var x = (MethodModel)item;
                List<ReflectionElement> children = new List<ReflectionElement>();
                if (x.ReturnType.TypeName != "Void")
                    children.Add(x.ReturnType);
                children.AddRange(x.Parameters);
                return children;
            }
            return null; 
        }
    }
}
