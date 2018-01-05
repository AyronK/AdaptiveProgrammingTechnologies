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
        public static string GetDescription(this AssemblyMetadata _assembly)
        {
            return _assembly.Name;
        }

        public static string GetDescription(this NamespaceMetadata _namespace)
        {
            return _namespace.Name;
        }

        public static string GetDescription(this TypeMetadata _type)
        {
            StringBuilder output = new StringBuilder();
            //foreach (var attribute in _type.Attributes)
            //{
            //    output.Append($"[{attribute.Name}] ");
            //}

            output.Append(_type.Name);

            //if (_type.ImplementedInterfaces.Count > 0 || _type.BaseType != null)
            //{
            //    output.Append(": ");
            //    if (_type.BaseType != null)
            //    {
            //        output.Append($"{_type.BaseType.Name}, ");
            //    }
            //    foreach (TypeMetadata impInterface in _type.ImplementedInterfaces)
            //        output.Append($"{impInterface.Name}, ");
            //    output.Remove(output.Length - 2, 2);
            //}
            return output.ToString();
        }

        public static string GetDescription(this VarMetadata _var)
        {
            StringBuilder output = new StringBuilder();

            foreach (var attribute in _var.Attributes)
            {
                output.Append($"[{attribute.Name}] ");
            }

            //output.Append(_var.Type.Name);

            output.Append($" {_var.Name}");
            return output.ToString();
        }

        public static string GetDescription(this MethodMetadata _method)
        {
            StringBuilder output = new StringBuilder();
            //foreach (var attribute in _method.Attributes)
            //{
            //    output.Append($"[{attribute.Name}] ");
            //}

            //foreach (string modifier in _method.Modifiers)
            //    output.Append(modifier + " ");

            //output.Append(_method.ReturnType.Name);
            //output.Append($" {_method.Name}");

            //output.Append(" (");
            //if (_method.Parameters.Count > 0)
            //{
            //    foreach (VarMetadata parameter in _method.Parameters)
            //        //output.Append(parameter.Type.Name + " " + parameter.Name + ", ");
            //    output.Remove(output.Length - 2, 2);
            //}
            output.Append(")");

            return output.ToString();
        }

        public static string GetDescription(this IReflectionElement item)
        {
            if (item.GetType() == typeof(AssemblyMetadata))
            {
                return (item as AssemblyMetadata).GetDescription();
            }
            else if (item.GetType() == typeof(NamespaceMetadata))
            {
                return (item as NamespaceMetadata).GetDescription();
            }
            else if (item.GetType() == typeof(TypeMetadata))
            {
                return (item as TypeMetadata).GetDescription();
            }
            else if (item.GetType() == typeof(VarMetadata))
            {
                return (item as VarMetadata).GetDescription();
            }
            else if (item.GetType() == typeof(MethodMetadata))
            {
                return (item as MethodMetadata).GetDescription();
            }
            else throw new NotSupportedException("Extension method does not support external implementations of ReflectionElement");
        }

        public static IEnumerable<IReflectionElement> GetChildren(this IReflectionElement item)
        {
            if (item.GetType() == typeof(AssemblyMetadata))
            {
                var x = (AssemblyMetadata)item;
                return x.NamespaceMetadatas;
            }
            else if (item.GetType() == typeof(NamespaceMetadata))
            {
                var x = (NamespaceMetadata)item;
                return x.Classes;
            }
            else if (item.GetType() == typeof(TypeMetadata))
            {
                var x = (TypeMetadata)item;
                List<IReflectionElement> children = new List<IReflectionElement>();
                //children.AddRange(x.Fields);
                //children.AddRange(x.Properties);
                //children.AddRange(x.Methods);
                //children.AddRange(x.Attributes);
                //children.AddRange(x.NestedTypes);
                //children.AddRange(x.ImplementedInterfaces);
                //children.AddRange(x.GenericArguments);
                //if (x.BaseType != null)
                //{
                //    children.Add(x.BaseType);
                //}
                return children;
            }
            else if (item.GetType() == typeof(VarMetadata))
            {
                var x = (VarMetadata)item;
                List<IReflectionElement> children = new List<IReflectionElement>();
                //children.Add(x.Type);
                children.AddRange(x.Attributes);
                return children;
            }
            else if (item.GetType() == typeof(MethodMetadata))
            {
                var x = (MethodMetadata)item;
                List<IReflectionElement> children = new List<IReflectionElement>();
                //if (x.ReturnType.Name != "Void")
                //    children.Add(x.ReturnType);
                //children.AddRange(x.Parameters);
                //children.AddRange(x.Attributes);
                return children;
            }
            return null;
        }
    }
}
