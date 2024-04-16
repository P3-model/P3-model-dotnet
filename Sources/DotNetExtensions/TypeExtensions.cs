namespace DotNetExtensions;

public static class TypeExtensions
{
    public static string GetFullTypeName(this Type type, char separator = '.')
    {
        var fullName = type.Name;
        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
            fullName = $"{type.Name}{separator}{fullName}";
        }
        return fullName;
    }
}