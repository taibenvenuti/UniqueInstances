using System;
using System.Reflection;
using UnityEngine;

namespace UniqueInstances.Extensions
{
    public static partial class Extensions
    {
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type;
            if (other.GetType().IsInstanceOfType(comp))
                type = other.GetType();
            else if (comp.GetType().IsInstanceOfType(other))
                type = comp.GetType();
            else return null;

            PropertyInfo[] pinfos = type.GetProperties(Flags);
            foreach (var pinfo in pinfos)
                if (pinfo.CanWrite)
                {
                    try { pinfo.SetValue(comp, pinfo.GetValue(other, null), null); }
                    catch { }
                }

            FieldInfo[] finfos = type.GetFields(Flags);
            foreach (var finfo in finfos)
                finfo.SetValue(comp, finfo.GetValue(other));
            return comp as T;
        }
    }
}
