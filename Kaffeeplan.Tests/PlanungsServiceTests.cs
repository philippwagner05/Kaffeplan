using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class PlanungsServiceTests
{
    private static readonly List<string> mitarbeiter = [
        "Ralf", "Jochen", "Mario", "Gabriel",
        "Ehsan", "Shariyar", "Philipp", "Michi",
        "Bernhard", "Wolfi"
    ];

    [TestMethod]
    [DataRow(2026, 4)]
    [DataRow(2026, 6)]
    [DataRow(2026, 8)]
    [DataRow(2027, 4)]
    [DataRow(2027, 6)]
    [DataRow(2027, 8)]
    public void Sichtpruefung_PlanAusgeben(int testjahr, int groese)
    {
        var plan = ErzeugeTestplan(testjahr, groese);
        foreach (var e in plan.Eintraege)
        {
            Console.WriteLine($"KW {e.Kalenderwoche:D2} | {e.MontagDatum:dd.MM.yyyy} | " +
                              $"{e.MitarbeiterName,-10} | {e.Aufgabenart}");
        }

    }
    [TestMethod]
    [DataRow(2026, 4)]
    [DataRow(2026, 6)]
    [DataRow(2026, 8)]
    [DataRow(2027, 4)]
    [DataRow(2027, 6)]
    [DataRow(2027, 8)]
    public void ErzeugePlan_ReinigungenSindAusgeglichen(int testjahr, int groese)
    {
        var plan = ErzeugeTestplan(testjahr, groese);

        var proPerson = plan.ReinigungProMitarbeiter();
        int max = proPerson.Values.Max();
        int min = proPerson.Values.Min();

        Assert.IsLessThanOrEqualTo(1, max - min, $"Reinigungen sind ungleich verteilt: min={min}, max={max}");

        Console.WriteLine($"Max {max} Min {min}");
    }

    [TestMethod]
    [DataRow(2026, 4)]
    [DataRow(2026, 6)]
    [DataRow(2026, 8)]
    [DataRow(2027, 4)]
    [DataRow(2027, 6)]
    [DataRow(2027, 8)]
    public void ErzeugePlan_FiltertauscheSindAusgeglichen(int testjahr, int groese)
    {
        var plan = ErzeugeTestplan(testjahr, groese);
        var proPerson = plan.FiltertauschProMitarbeiter();
        int max = proPerson.Values.Max();
        int min = proPerson.Values.Min();

        Console.WriteLine($"Max={max} Min={min}");

        Assert.IsLessThanOrEqualTo(1, max - min, $"Filtertausche sind ungleich verteilt: min={min}, max={max}. " +
        string.Join(", ", proPerson.Select(p => $"{p.Key}={p.Value}")));
    }

    [TestMethod]
    [DataRow(2026, 4)]
    [DataRow(2026, 6)]
    [DataRow(2026, 8)]
    [DataRow(2027, 4)]
    [DataRow(2027, 6)]
    [DataRow(2027, 8)]
    public void ErzeugePlan_NiemandZweimalHintereinandner(int testjahr, int groese)
    {
        var plan = ErzeugeTestplan(testjahr, groese);
        var e = plan.Eintraege.OrderBy(x => x.Kalenderwoche).ToList();

        for (int i = 1; i < e.Count; i++)
        {
            Assert.AreNotEqual(e[i - 1].MitarbeiterName, e[i].MitarbeiterName,
                $"KW {e[i].Kalenderwoche}: {e[i].MitarbeiterName} ist zweimal hinterinander dran.");
        }
    }

    private static Jahresplan ErzeugeTestplan(int jahr, int groese)
    {
        List<Mitarbeiter> team = [.. mitarbeiter.Take(groese).Select(n => new Mitarbeiter { Name = n })];

        var service = new PlanungsService(new KalenderService());
        var plan = service.ErzeugePlan(jahr, team);

        return plan;

    }
}
