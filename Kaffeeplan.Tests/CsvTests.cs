using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Persistenz;
using Kaffeeplan.Core.Services;

namespace Kaffeeplan.Tests;

[TestClass]
public class CsvTests
{
    private string? _tempFilePath;

    [TestMethod]
    public void CSVMaskierung()
    {
        string test = "Cristiano; Ronaldo";
        string erwartet = "\"Cristiano; Ronaldo\"";
        var maskiert = CsvSpeicher.Maskiere(test);
        Assert.AreEqual(erwartet, maskiert);
    }

    [TestMethod]
    [DataRow(2026)]
    [DataRow(2027)]
    public void istUTFBOM(int jahr)
    {
        var plan = ErzeugeTestplan(jahr);
        var fullPath = Path.GetTempPath() + $"CsvTestPlan-{jahr}.csv";
        _tempFilePath = fullPath;
        CsvSpeicher.Exportiere(plan, fullPath);
        var bytes = File.ReadAllBytes(fullPath);
        Assert.IsGreaterThanOrEqualTo(3, bytes.Length, "Datei ist zu kurz");
        Assert.AreEqual(0xEF, bytes[0]);
        Assert.AreEqual(0xBB, bytes[1]);
        Assert.AreEqual(0xBF, bytes[2]);
    }

    [TestCleanup]
    public void LoescheTestDaten()
    {
        if (!string.IsNullOrEmpty(_tempFilePath) && File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
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
            new() { Name = "Bernhard"}, new() { Name = "Wolfi" }
        };

        var service = new PlanungsService(new KalenderService());
        var plan = service.ErzeugePlan(jahr, team);

        return plan;
    }
}
