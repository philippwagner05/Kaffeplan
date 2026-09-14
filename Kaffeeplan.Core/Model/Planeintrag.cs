namespace Kaffeeplan.Core.Model;

public class Planeintrag
{
    public int Kalenderwoche { get; set; }
    public DateOnly MontagDatum { get; set; }
    public string MitarbeiterName { get; set; } = string.Empty;
    public Aufgabenart Aufgabenart { get; set; }
    public Aufgabenart HatFiltertausch { get; set; } = Aufgabenart.Keine;
    
}
