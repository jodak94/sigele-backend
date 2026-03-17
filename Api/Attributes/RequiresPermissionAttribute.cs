namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RequiresPermissionAttribute : Attribute
{
    public string Permission { get; }

    public RequiresPermissionAttribute(string permissions)
    {
        Permission = permissions;
    }
}