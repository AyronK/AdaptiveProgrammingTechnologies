using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Reflector.Models
{
    [DataContract(IsReference = true)]
    public class NamespaceInfo : IReflectionElement
    {
        #region Constructors
        public NamespaceInfo(string name, IEnumerable<Type> types, AssemblyInfo assembly)
        {
            Name = name;
            Classes = from type in types orderby type.Name select new TypeInfo(type, this);
        }
        #endregion

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<TypeInfo> Classes
        {
            get; private set;
        }

        internal void LoadClasses(System.Reflection.Assembly assembly, AssemblyInfo assemblyModel)
        {
            foreach (Type type in assembly.GetTypes())
            {
                AddClass(assemblyModel, type);
            }
        }

        private void AddClass(AssemblyInfo assemblyModel, Type type)
        {
            if (type.Namespace == Name)
            {
                TryDefineTypeModel(type);
                _classes.Add(TypesAlreadyDefined.Find(t => t.Name == type.Name));
            }
        }

        [DataMember]
        internal List<TypeInfo> TypesAlreadyDefined = new List<TypeInfo>();

        internal void TryDefineTypeModel(Type type)
        {
            //if (type.IsGenericType)
            //{
            //    try
            //    {

            //        var x = type.gene;
            //    }
            //    catch (Exception e)
            //    {
            //        ;
            //        throw;
            //    }
            //    bool areSame = true;
            //    var sameTypes = TypesAlreadyDefined.FindAll(t => t.Name == type.Name);
            //    if (sameTypes.Count == 0)
            //    {
            //        areSame = false;
            //    }
            //    foreach (var sameType in sameTypes)
            //    {
            //        var gens = type.GetGenericArguments();
            //        if (gens == null)
            //        {
            //            areSame = true;
            //        }
            //        else
            //        if (sameType.GenericArguments.Count == gens.Length && sameType.GenericArguments.Count != 0)
            //        {
            //            for (int i = 0; i < sameType.GenericArguments.Count; i++)
            //            {
            //                if (sameType.GenericArguments[i].Name != gens[i].Name)
            //                    areSame = false;
            //            }
            //        }
            //        else
            //        {
            //            areSame = false;
            //        }
            //    }
            //    if (!areSame)
            //    {
            //        TypeInfo classModel = new TypeInfo() { Name = type.Name };
            //        TypesAlreadyDefined.Add(classModel);
            //        classModel.LoadItself(type, this);
            //    }
            //}
            //else
            //{
            if (TypesAlreadyDefined.Find(t => t.Name == type.Name) == null)
            {
                TypeInfo classModel = new TypeInfo() { Name = type.Name };
                TypesAlreadyDefined.Add(classModel);
                classModel.LoadItself(type, this);
            }
            //}
        }

        #region Privates
        private List<TypeInfo> _classes = new List<TypeInfo>();
        #endregion
    }
}
