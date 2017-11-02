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
        public static string GetDescription(this AssemblyInfo _assembly)
        {
            return _assembly.Name;
        }

        public static string GetDescription(this NamespaceInfo _namespace)
        {
            return _namespace.Name;
        }

        public static string GetDescription(this TypeInfo _type)
        {
            return _type.Name;
        }

        public static string GetDescription(this VarModel _var)
        {
            return $"{_var.Type.Name} {_var.Name}";
        }

        public static string GetDescription(this MethodModel _method)
        {
            StringBuilder output = new StringBuilder();

            foreach (string modifier in _method.Modifiers)
                output.Append(modifier + " ");

            output.Append($"{_method.ReturnType.Name} {_method.Name}");

            output.Append(" (");
            if (_method.Parameters.Count > 0)
            {
                foreach (VarModel parameter in _method.Parameters)
                    output.Append(parameter.Type.Name + " " + parameter.Name + ", ");
                output.Remove(output.Length - 2, 2);
            }
            output.Append(")");

            return output.ToString();
        }

        public static string GetDescription(this ReflectionElement item)
        {
            if (item.GetType() == typeof(AssemblyInfo))
            {
                return (item as AssemblyInfo).GetDescription();
            }
            else if (item.GetType() == typeof(NamespaceInfo))
            {
                return (item as NamespaceInfo).GetDescription();
            }
            else if (item.GetType() == typeof(TypeInfo))
            {
                return (item as TypeInfo).GetDescription();
            }
            else if (item.GetType() == typeof(VarModel))
            {
                return (item as VarModel).GetDescription();
            }
            else if (item.GetType() == typeof(MethodModel))
            {
                return (item as MethodModel).GetDescription();
            }
            else throw new NotSupportedException("Extension method does not support external implementations of ReflectionElement");
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
                return x.Type.GetChildren();
            }
            else if (item.GetType() == typeof(MethodModel))
            {
                var x = (MethodModel)item;
                List<ReflectionElement> children = new List<ReflectionElement>();
                if (x.ReturnType.Name != "Void")
                    children.Add(x.ReturnType);
                children.AddRange(x.Parameters);
                return children;
            }
            return null; 
        }
    }
}
