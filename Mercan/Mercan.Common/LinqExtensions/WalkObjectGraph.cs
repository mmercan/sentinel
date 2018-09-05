using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MMercan.Common.LinqExtensions
{
    public static class WalkExtensions
    {

        // bool isDirty = false;
        //WalkObjectGraph<IDirtyCheck>(dirt,(d) => { if (d.IsDirty) { isDirty = true; return true; } else { return false; } }, null, null);
        public static void WalkObjectGraph<T>(T content, Func<T, bool> ObjectCommand, Action<IEnumerable> ListCommand, params string[] examptProperties) where T : class
        {
            HashSet<Type> myPrimetiveTypes = new HashSet<Type>();
            if (typeof(T) == typeof(object))
            {
                myPrimetiveTypes = new HashSet<Type>() { typeof(Boolean), typeof(Byte), typeof(SByte), typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64), typeof(IntPtr), typeof(UIntPtr), typeof(Char), typeof(Double), typeof(Single), typeof(string), typeof(DateTime), typeof(DateTime?), typeof(decimal), typeof(int?) };
            }


            List<string> exemptions = new List<string>();
            if (examptProperties != null) { exemptions = exemptions.ToList(); }

            List<T> visited = new List<T>();
            Action<T> walk = null;
            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);
                    bool exitwalk = false;
                    if (ObjectCommand != null) { exitwalk = ObjectCommand.Invoke(o); }
                    if (!exitwalk)
                    {
                        List<PropertyInfo> Properties = o.GetType().GetRuntimeProperties().ToList();
                        foreach (PropertyInfo property in Properties)
                        {
                            if (!exemptions.Contains(property.Name) && !myPrimetiveTypes.Contains(property.PropertyType))
                            {
                                if (typeof(T).IsAssignableFrom(property.PropertyType))
                                {
                                    T dt = property.GetValue(o, null) as T;
                                    walk(dt);
                                }
                                else
                                {
                                    IEnumerable lst = property.GetValue(o, null) as IEnumerable;
                                    //ICollection is common but not everythere HashSet<T> inherits from ICollection<T> but not ICollection, string has IEnumerable interface which is not one of our target collection type
                                    if (lst != null && lst.GetType() != typeof(string))
                                    {
                                        if (ListCommand != null) { ListCommand.Invoke(lst); }
                                        foreach (object item in lst)
                                        {
                                            if (typeof(T).IsAssignableFrom(item.GetType())) walk(item as T);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            walk(content);
        }
    }

}
