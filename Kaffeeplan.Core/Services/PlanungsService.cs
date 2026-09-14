using Kaffeeplan.Core.Model;

namespace Kaffeeplan.Core.Services;

public class PlanungsService
{
    private readonly KalenderService _kalender;

    public PlanungsService(KalenderService kalender)
    {
        _kalender = kalender;
    }
    
    public Jahresplan ErzeugePlan(
        int jahr,
        IReadOnlyList<Mitarbeiter> mitarbeiter,
        int filterRhythmus = 8)
    {
        if(mitarbeiter.Count == 0 || filterRhythmus < 1)
        {
            throw new ArgumentOutOfRangeException();
        }   

        var jahresplan = new Jahresplan();
        var wochen = _kalender.WochenImJahr(jahr);

        for (int woche = 1; woche <= wochen; woche++)
        {
            var person = mitarbeiter[(woche - 1) % mitarbeiter.Count];
            var aufgaben = Aufgabenart.Reinigung;
            if ((woche - 1) % filterRhythmus == 0)
            {
                aufgaben |= Aufgabenart.Filtertausch;
            }
            var planeintrag = new Planeintrag
            {
                Kalenderwoche = woche,
                MitarbeiterName = person.Name,
                MontagDatum = _kalender.MontageDerWoche(jahr, woche),
                Aufgabenart = aufgaben

            };

            jahresplan.Eintraege.Add(planeintrag); 
        }
        return jahresplan;
    }
}
