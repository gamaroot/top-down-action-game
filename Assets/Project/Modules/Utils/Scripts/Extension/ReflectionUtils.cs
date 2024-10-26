using System.Reflection;

namespace Utils
{
    public static class ReflectionUtils
    {
        public static void CallNonPublicMethod<T>(this T objectRef, string name, object[] parameters = null)
        {
            objectRef.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(objectRef, parameters);
        }
    }
}