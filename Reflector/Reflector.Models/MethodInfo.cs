//using MVVMProject.Model.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class MethodModel : IExpandable
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<string> Modifiers { get { return _modifiers; } private set { _modifiers = value; } }
        [DataMember]
        public List<VarModel> Parameters { get { return _parameters; } private set { _parameters = value; } }
        [DataMember]
        public TypeInfo ReturnType { get { return _returnType; } set { _returnType = value; } }

        internal void LoadItself(MethodInfo method, AssemblyInfo assembly)
        {
            Name = method.Name;
            LoadModifiers(method);

             //ReturnType = new TypeModel() { TypeName = method.ReturnType.Name }; 
            assembly.TryDefineTypeModel(method.ReturnType);
            ReturnType = assembly.Classes[method.ReturnType.Name];

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                AddParameter(assembly, parameter);
            }
        }

        private void AddParameter(AssemblyInfo assembly, ParameterInfo parameter)
        {
            string typeName = parameter.ParameterType.Name;
 
            /*if (!assembly.Classes.ContainsKey(typeName))
             {
                 TypeModel classModel = new TypeModel() { TypeName = typeName };
                 assembly.Classes.Add(typeName, classModel);
             }
             VarModel p = new VarModel() { Name = parameter.Name, BaseType = assembly.Classes[typeName] };*/

            assembly.TryDefineTypeModel(parameter.ParameterType);
            VarModel p = new VarModel() { Name = parameter.Name, BaseType = assembly.Classes[typeName] };

            Parameters.Add(p);
        }

        #region Object override
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            foreach (string modifier in Modifiers)
                output.Append(modifier + " ");

            output.Append(ReturnType.TypeName + " " + Name + "(");

            if (Parameters.Count > 0)
            {
                foreach (VarModel parameter in Parameters)
                    output.Append(parameter.BaseType.TypeName + " " + parameter.Name + ", ");
                output.Remove(output.Length - 2, 2);
            }

            output.Append(")");
            return output.ToString();
        }
        #endregion

        #region Privates
        private List<VarModel> _parameters = new List<VarModel>();
        private List<string> _modifiers = new List<string>();
        private TypeInfo _returnType;
        private void LoadModifiers(MethodInfo method)
        {
            if (method.IsAbstract) Modifiers.Add("abstract");
            if (method.IsFinal) Modifiers.Add("final");
            if (method.IsPrivate) Modifiers.Add("private");
            if (method.IsPublic) Modifiers.Add("public");
            if (method.IsStatic) Modifiers.Add("static");
            if (method.IsVirtual) Modifiers.Add("virtual");
        }
        #endregion

        #region IExpandable implementation
        public IEnumerable<IExpandable> Expand()
        {
            List<IExpandable> children = new List<IExpandable>();
            if (ReturnType.TypeName != "Void")
                children.Add(ReturnType);
            children.AddRange(Parameters);
            return children;
        } 
        #endregion
    }
}
