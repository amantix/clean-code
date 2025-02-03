namespace Core.Models;

public class Role(string name)
{
    public string Name { get; set; } = name;
    public static Role Create(string name) => new Role(name);
    public static Role User => new Role("user");
    public static Role Admin => new Role("admin");
    public override string ToString() => Name;
}