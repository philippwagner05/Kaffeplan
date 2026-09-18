namespace Kaffeeplan.Core.Model;

public class Planeintrag
{
    public int Kalenderwoche { get; set; }
    public DateOnly MontagDatum { get; set; }
    public string MitarbeiterName { get; set; } = string.Empty;
    public Aufgabenart Aufgabenart { get; set; }
    public bool HatFiltertausch => Aufgabenart.HasFlag(Aufgabenart.Filtertausch);
    public override string ToString()
    {
        return $"KW: {Kalenderwoche} - Montag: {MontagDatum} - Mitarbeiter: {MitarbeiterName} - Aufgabenart: {Aufgabenart}";
    }
}
