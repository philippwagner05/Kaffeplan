using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class PlanungsServiceTests
{
    [TestMethod]
    public void Sichtpruefung_PlanAusgeben()
    {
        var team = new List<Mitarbeiter>
        {
            new() { Name = "Ralf" }, new() { Name = "Jochen"},
            new() { Name = "Mario"}, new() { Name = "Gabriel"},
            new() { Name = "Ehsan"}, new() { Name = "Shariyar"}
        };

        var service = new PlanungsService(new KalenderService());
        var plan = service.ErzeugePlan(2027, team);

        foreach (var e in plan.Eintraege)
        {
            Console.WriteLine($"KW {e.Kalenderwoche:D2} | {e.MontagDatum:dd.MM.yyyy} | " +
                              $"{e.MitarbeiterName,-10} | {e.Aufgabenart}");
        }
    }
}
