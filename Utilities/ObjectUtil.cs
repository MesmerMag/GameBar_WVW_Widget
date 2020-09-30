namespace GameBarWidget.Utilities
{
    public class ObjectUtil
    {
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T) obj.GetType().GetProperty(propName)?.GetValue(obj, null);
        }
    }
}