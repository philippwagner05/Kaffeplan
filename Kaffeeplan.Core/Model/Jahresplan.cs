namespace Kaffeeplan.Core.Model;

public class Jahresplan
{
    public int Jahr { get; set; }
    public List<Planeintrag> Eintraege { get; set; } = [];
    public Dictionary<string, int> ReinigungProMitarbeiter()
    {
        Dictionary<string, int> reinigungProMitarbeiter = [];    
        foreach (var eintrag in Eintraege)
        {
           if(!reinigungProMitarbeiter.TryAdd(eintrag.MitarbeiterName, 1))
            {
                reinigungProMitarbeiter[eintrag.MitarbeiterName] += 1;
            }
        }
        return reinigungProMitarbeiter;
    }

    public Dictionary<string, int> FiltertauschProMitarbeiter()
    {
        Dictionary<string, int> filtertauschProMitarbeiter = [];
        foreach (var eintrag in Eintraege)
        {
            filtertauschProMitarbeiter.TryAdd(eintrag.MitarbeiterName, 0);
            if ((eintrag.Aufgabenart & Aufgabenart.Filtertausch) == Aufgabenart.Filtertausch)
            {
                filtertauschProMitarbeiter[eintrag.MitarbeiterName] += 1;
            }
        }
        return filtertauschProMitarbeiter;
    }   
    
    
}
