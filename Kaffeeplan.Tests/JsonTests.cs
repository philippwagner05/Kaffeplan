using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Persistenz;
using Kaffeeplan.Core.Services;

namespace Kaffeeplan.Tests;

[TestClass]
public class JsonTests
{
    private readonly JsonSpeicher _jsonSpeichern = new();
    private int Jahr = 2027;

    [TestMethod]
    public void LädtGleicheDaten()
    {
        var plan = ErzeugeTestplan(Jahr);

        Console.WriteLine(Path.GetTempPath() + $"JsonTestPlan-{Jahr}.json");

        _jsonSpeichern.Speichern(plan, Path.GetTempPath() + $"JsonTestPlan-{Jahr}.json");

        var geladenerplan = _jsonSpeichern.Laden<Jahresplan>(Path.GetTempPath() + $"JsonTestPlan-{Jahr}.json");
        Assert.IsNotNull(geladenerplan);
        Assert.HasCount(plan.Eintraege.Count, geladenerplan.Eintraege);

        for (int i = 0; i < plan.Eintraege.Count; i++)
        {
            var planeintrag = plan.Eintraege[i];
            var geladenerplaneintrag = geladenerplan.Eintraege [i];

            Assert.AreEqual(planeintrag.MitarbeiterName, geladenerplaneintrag.MitarbeiterName);
            Assert.AreEqual(planeintrag.Kalenderwoche, geladenerplaneintrag.Kalenderwoche);
            Assert.AreEqual(planeintrag.Aufgabenart, geladenerplaneintrag.Aufgabenart);
            Assert.AreEqual(planeintrag.MontagDatum, geladenerplaneintrag.MontagDatum);
        }
    }

    [TestCleanup]
    public void LoescheTestDaten()
    {
        File.Delete(Path.GetTempPath() + $"JsonTestPlan-{Jahr}.json");
    }

    private static Jahresplan ErzeugeTestplan(int jahr)
    {
        var team = new List<Mitarbeiter>
        {
            new() { Name = "Ralf" }, new() { Name = "Jochen"},
            new() { Name = "Mario"}, new() { Name = "Gabriel"},
            new() { Name = "Ehsan"}, new() { Name = "Shariyar"},
            new() { Name = "Philipp"}, new() { Name = "Michi"},
            new() { Name = "Bernhard"}, new() { Name = "Wolfi"}
        };

        var service = new PlanungsService(new KalenderService());
        var plan = service.ErzeugePlan(jahr, team);

        return plan;
    }
}
