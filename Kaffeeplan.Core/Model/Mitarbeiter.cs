namespace Kaffeeplan.Core.Model;

public class Mitarbeiter
{
    public string Name { get; set; } = string.Empty;
    public bool IstAktiv { get; set; } = true;
    public override string ToString() => Name;
}
