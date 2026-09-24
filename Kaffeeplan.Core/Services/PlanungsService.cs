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

        int anzahl = mitarbeiter.Count;
        int wochen = _kalender.WochenImJahr(jahr);
        
        var wahl = new int [wochen + 1];

        for (int i = 1; i <= wochen; i++)
        {
            wahl[i] = -1;
        }

        var naechste = 0;

        for (int i = 1; i <= wochen; i++)
        {
            if ((i - 1) % filterRhythmus == 0)
            {
                wahl[i] = naechste % anzahl;
                naechste++;
            }
        }

        var reinigungen = new int [anzahl];

        for (int i = 1; i <= wochen; i++)
        {
            if (wahl[i] != -1)
                reinigungen[wahl[i]]++;
        }

        // 0 0 -1 -1 -1 -1 ... 1
        for (int i = 1; i < wochen; i++)
        {
            if (wahl[i] != -1)
                continue;

            var davor = i > 1 ? wahl[i - 1] : -1;
            var danach = i < wochen ? wahl[i + 1] : -1; 

            var gewaehlt = WaehleMitarbeiter(reinigungen, davor, danach);
            wahl [i] = gewaehlt;
            reinigungen[gewaehlt]++;
        }

        var plan = new Jahresplan
        {
            Jahr = jahr
        };

        for (int i = 1; i < wochen; i++)
        {
            bool istFilterwoche = (i - 1) % filterRhythmus == 0;

            var aufgaben = istFilterwoche ? Aufgabenart.Reinigung | Aufgabenart.Filtertausch : Aufgabenart.Reinigung;

            var planeintrag = new Planeintrag
            {
                Aufgabenart = aufgaben,
                MitarbeiterName = mitarbeiter[wahl[i]].Name,
                Kalenderwoche = i,
                MontagDatum = _kalender.MontageDerWoche(jahr, i)
            };

            plan.Eintraege.Add(planeintrag);
        }

        return plan;
    }

    private static int WaehleMitarbeiter(
        int[] reinigungen, int davor, int danach) // 0 , 1
    {
        int besterIndex = -1; // 2

        for (int i = 0; i < reinigungen.Length; i++)
        {
            if (i == davor || i == danach)
                continue;

            if (besterIndex == -1 || reinigungen[i] < reinigungen[besterIndex])
                besterIndex = i;
        }

        if (besterIndex == -1)
        {
            for (int i = 0; i < reinigungen.Length; i++)
            {
                if (i == davor)
                    continue;

                if (besterIndex == -1 || reinigungen[i] < reinigungen[besterIndex])
                    besterIndex = i;
            }
        }
        return besterIndex;
    }

    private static bool IstBesser(
        int kandidat, int bisher, bool istFilterwoche,
        int[] reinigungen, int[] filter, int[] letzteWoche)
    {
        if (istFilterwoche && filter[kandidat] != filter[bisher])
            return filter[kandidat] < filter[bisher];

        if (reinigungen[kandidat] != reinigungen[bisher])
            return reinigungen[kandidat] < reinigungen[bisher];

        if (letzteWoche[kandidat] != letzteWoche[bisher])
            return letzteWoche[kandidat] < letzteWoche[bisher];

        return false;
    }
}
