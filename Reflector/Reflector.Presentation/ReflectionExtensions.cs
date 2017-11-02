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
            StringBuilder output = new StringBuilder();
            foreach (var attribute in _type.Attributes)
            {
                output.Append($"[{attribute.Name}] ");
            }

            output.Append(_type.Name);

            if (_type.ImplementedInterfaces.Count > 0 || _type.BaseType != null)
            {
                output.Append(": ");
                if (_type.BaseType != null)
                {
                    output.Append($"{_type.BaseType.Name}, ");
                }
                foreach (TypeInfo impInterface in _type.ImplementedInterfaces)
                    output.Append($"{impInterface.Name}, ");
                output.Remove(output.Length - 2, 2);
            }
            return output.ToString();
        }

        public static string GetDescription(this VarModel _var)
        {
            StringBuilder output = new StringBuilder();

            foreach (var attribute in _var.Attributes)
            {
                output.Append($"[{attribute.Name}] ");
            }

            output.Append(_var.Type.Name);

            output.Append($" {_var.Name}");
            return output.ToString();
        }

        public static string GetDescription(this MethodModel _method)
        {
            StringBuilder output = new StringBuilder();
            foreach (var attribute in _method.Attributes)
            {
                output.Append($"[{attribute.Name}] ");
            }

            foreach (string modifier in _method.Modifiers)
                output.Append(modifier + " ");

            output.Append(_method.ReturnType.Name);
            output.Append($" {_method.Name}");

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

        public static string GetDescription(this IReflectionElement item)
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

        public static IEnumerable<IReflectionElement> GetChildren(this IReflectionElement item)
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
                List<IReflectionElement> children = new List<IReflectionElement>();
                children.AddRange(x.Fields);
                children.AddRange(x.Properties);
                children.AddRange(x.Methods);
                children.AddRange(x.Attributes);
                children.AddRange(x.NestedTypes);
                children.AddRange(x.ImplementedInterfaces);
                children.AddRange(x.GenericArguments);
                if (x.BaseType != null)
                {
                    children.Add(x.BaseType);
                }
                return children;
            }
            else if (item.GetType() == typeof(VarModel))
            {
                var x = (VarModel)item;
                List<IReflectionElement> children = new List<IReflectionElement>();
                children.Add(x.Type);
                children.AddRange(x.Attributes);
                return children;
            }
            else if (item.GetType() == typeof(MethodModel))
            {
                var x = (MethodModel)item;
                List<IReflectionElement> children = new List<IReflectionElement>();
                if (x.ReturnType.Name != "Void")
                    children.Add(x.ReturnType);
                children.AddRange(x.Parameters);
                children.AddRange(x.Attributes);
                return children;
            }
            return null;
        }
    }
}
