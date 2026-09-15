using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
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
        ArgumentNullException.ThrowIfNull(mitarbeiter);
        if (mitarbeiter.Count <= 1)
            throw new ArgumentOutOfRangeException(nameof(filterRhythmus));

        int wochen = _kalender.WochenImJahr(jahr);

        var reinigungen = new int[mitarbeiter.Count];
        var filter      = new int[mitarbeiter.Count];
        var letzteWoche  = new int[mitarbeiter.Count];
        Array.Fill(letzteWoche, int.MinValue);

        var plan = new Jahresplan { Jahr = jahr };

        for (int woche = 1; woche <= wochen; woche++)
        {
            bool istFilterwoche = (woche - 1) % filterRhythmus == 0;

            int gewaehlt = WaehleMitarbeiter(
                mitarbeiter.Count, istFilterwoche, reinigungen, filter, letzteWoche);
            
            var aufgaben = Aufgabenart.Reinigung;
            reinigungen[gewaehlt] += 1;
            if (istFilterwoche)
            {
                aufgaben |= Aufgabenart.Filtertausch;
                filter[gewaehlt] += 1;
            }
            var planeintrag = new Planeintrag
            {
                Kalenderwoche = woche,
                MitarbeiterName = mitarbeiter[gewaehlt].Name,
                MontagDatum = _kalender.MontageDerWoche(jahr, woche),
                Aufgabenart = aufgaben
            };

            plan.Eintraege.Add(planeintrag);

            letzteWoche[gewaehlt] = woche;
        } 

        return plan;
    }

    private static int WaehleMitarbeiter(
        int anzahl, bool istFilterwoche, int[] reinigungen, int[] filter, int [] letzteWoche)
    {
        int besterIndex = 0;

        for (int i = 1; i < anzahl; i++)
        {
            if (IstBesser(i, besterIndex, istFilterwoche, reinigungen, filter, letzteWoche))
                besterIndex = i;
        }
        
        return besterIndex;
    }

    private static bool IstBesser(
        int kandidat, int bisher, bool istFilterwoche,
        int[] reinigungen, int[] filter, int[] letzeWoche)
    {
        if (istFilterwoche)
        {
            if (filter[kandidat] < filter[bisher])
                return true;
        }
        if (reinigungen[kandidat] < reinigungen[bisher])
            return true;
        return false;
    }

}
