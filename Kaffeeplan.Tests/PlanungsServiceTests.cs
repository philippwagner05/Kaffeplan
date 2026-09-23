using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class PlanungsServiceTests
{
    private int testjahr = 2027;
    private int groese = 0;
    [TestMethod]
    public void Sichtpruefung_PlanAusgeben()
    {
        for (int groese = 0; groese < 3; groese++)
        {
            var plan = ErzeugeTestplan(testjahr, groese);
            foreach (var e in plan.Eintraege)
            {
                Console.WriteLine($"KW {e.Kalenderwoche:D2} | {e.MontagDatum:dd.MM.yyyy} | " +
                                  $"{e.MitarbeiterName,-10} | {e.Aufgabenart}");
            }
        }

    }
    [TestMethod]
    public void ErzeugePlan_2027_ReinigungenSindAusgeglichen()
    {
        for (int groese = 0; groese < 3; groese++)
        {
            var plan = ErzeugeTestplan(testjahr, groese);

            var proPerson = plan.ReinigungProMitarbeiter();
            int max = proPerson.Values.Max();
            int min = proPerson.Values.Min();

            Assert.IsLessThanOrEqualTo(1, max - min, $"Reinigungen sind ungleich verteilt: min={min}, max={max}");

            Console.WriteLine($"Max {max} Min {min}");
        }

    }

    [TestMethod]
    public void ErzeugePlan_2027_FiltertauscheSindAusgeglichen()
    {
        for (int groese = 0; groese < 3; groese++)
        {
            var plan = ErzeugeTestplan(testjahr, groese);
            var proPerson = plan.FiltertauschProMitarbeiter();
            int max = proPerson.Values.Max();
            int min = proPerson.Values.Min();

            Console.WriteLine($"Max={max} Min={min}");

            Assert.IsLessThanOrEqualTo(1, max - min, $"Filtertausche sind ungleich verteilt: min={min}, max={max}. " +
            string.Join(", ", proPerson.Select(p => $"{p.Key}={p.Value}")));
        }


    }

    [TestMethod]
    public void ErzeugePlan_2027_NiemandZweimalHintereinandner()
    {
        for (int groese = 0; groese < 3; groese++)
        {
            var plan = ErzeugeTestplan(testjahr, groese);
            var e = plan.Eintraege.OrderBy(x => x.Kalenderwoche).ToList();

            for (int i = 1; i < e.Count; i++)
            {
                Assert.AreNotEqual(e[i - 1].MitarbeiterName, e[i].MitarbeiterName,
                    $"KW {e[i].Kalenderwoche}: {e[i].MitarbeiterName} ist zweimal hinterinander dran.");
            }
        }
    }
    private static Jahresplan ErzeugeTestplan(int jahr, int groese)
    {
        List<Mitarbeiter> team = [];

        switch (groese)
        {
            case 0:
                team = [
                    new() { Name = "Ralf" }, new() { Name = "Jochen" },
                    new() { Name = "Mario" }, new() { Name = "Gabriel" },
                    new() { Name = "Ehsan" }, new() { Name = "Shariyar" },
                    new() { Name = "Philipp" }, new() { Name = "Michi" },
                    new() { Name = "Bernhard" }, new() { Name = "Wolfi" }
                ];
                break;
            case 1:
                team = [
                    new() { Name = "Ralf" }, new() { Name = "Jochen" },
                    new() { Name = "Mario" }, new() { Name = "Gabriel" },
                ];
                break;
            case 2:
                team = [
                    new() { Name = "Ralf" }, new() { Name = "Jochen" },
                    new() { Name = "Mario" }, new() { Name = "Gabriel" },
                    new() { Name = "Ehsan" }, new() { Name = "Shariyar" },
                ];
                break;
        }

        var service = new PlanungsService(new KalenderService());
        var plan = service.ErzeugePlan(jahr, team);

        return plan;

    }
}
