namespace Kaffeeplan.Core.Model;

public class Jahresplan
{
    public int Jahr { get; set; }
    public List<Planeintrag> Eintraege { get; set; } = [];
    public Dictionary<string, int> ReinigungProMitarbeiter()
    {
        Dictionary<string, int> reinigungProMitarbeiter = new Dictionary<string, int>();    
        foreach (var eintrag in Eintraege)
        {
           if(reinigungProMitarbeiter.ContainsKey(eintrag.MitarbeiterName))
            {
                reinigungProMitarbeiter[eintrag.MitarbeiterName] += 1;
            }
            else
            {
                reinigungProMitarbeiter.Add(eintrag.MitarbeiterName, 1);
            }
        }
        return reinigungProMitarbeiter;
    }

    public Dictionary<string, int> FiltertauschProMitarbeiter()
    {
        Dictionary<string, int> filtertauschProMitarbeiter = new Dictionary<string, int>();
        foreach (var eintrag in Eintraege)
        {
            if((eintrag.Aufgabenart & Aufgabenart.Filtertausch) == Aufgabenart.Filtertausch)
            {
                if(filtertauschProMitarbeiter.ContainsKey(eintrag.MitarbeiterName))
                {
                    filtertauschProMitarbeiter[eintrag.MitarbeiterName] += 1;
                }
                else
                {
                    filtertauschProMitarbeiter.Add(eintrag.MitarbeiterName, 1);
                }
            }
        }
        return filtertauschProMitarbeiter;
    }   
    
    
}
