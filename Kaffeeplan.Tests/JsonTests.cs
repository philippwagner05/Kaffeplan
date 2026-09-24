using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Persistenz;
using Kaffeeplan.Core.Services;
using System.Xml.Serialization;

namespace Kaffeeplan.Tests;

[TestClass]
public class JsonTests
{
    private readonly JsonSpeicher _jsonSpeichern = new();

    [TestMethod]
    [DataRow(2026)]
    [DataRow(2027)]
    public void LädtGleicheDaten(int jahr)
    {
        var plan = ErzeugeTestplan(jahr);

        _jsonSpeichern.Speichern(plan, Path.GetTempPath() + $"JsonTestPlan-{jahr}.json");

        var geladenerplan = _jsonSpeichern.Laden<Jahresplan>(Path.GetTempPath() + $"JsonTestPlan-{jahr}.json");
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

    [TestMethod]
    public void NichtVorhandeneDateiLaden()
    {
        Assert.ThrowsExactly<FileNotFoundException>(
            () => _jsonSpeichern.Laden<Jahresplan>(Path.GetTempPath() + "JsonTestPlan-GIBTESNICHT.json"));
    }

    [TestMethod]
    public void FehlerHafteDateiLaden()
    {
        string korrupterinhalt = "AUF gar keinen Fall typtische JSON Systax .,,.., .::: .,. .,. ,., . :,. , ich ,.,.du 4990309434324";

        _jsonSpeichern.Speichern(korrupterinhalt, Path.GetTempPath() + "korruptionstest.json");
        Assert.Throws<Exception>(
            () => _jsonSpeichern.Laden<Jahresplan>(Path.GetTempPath() + "korruptionstest.json"));
    }

    [TestCleanup]
    public void LoescheTestDaten()
    {
        var temp = Path.GetTempPath();

        foreach (var file in Directory.EnumerateFiles(temp, "JsonTestPlan-*.json"))
        {
            try { File.Delete(file); } catch { }
        }

        var korruptFile = Path.Combine(temp, "korruptionstest.json");
        if (File.Exists(korruptFile))
        {
            try { File.Delete(korruptFile); } catch { }
        }
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
