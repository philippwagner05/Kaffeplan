using Kaffeeplan.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class KalenderServiceTests
{
    [TestMethod]
    public void WochenImJahr_2026_Liefert53()
    {
        var service = new KalenderService();

        int ergebnis = service.WochenImJahr(2027);

        Assert.AreEqual(52, ergebnis);
    }

    [TestMethod]
    public void MontagDerWoche_2027KW9_Liefert1Mearz()
    {
        var service = new KalenderService();

        DateOnly ergebnis = service.MontageDerWoche(2027, 9);

        Assert.AreEqual(new DateOnly(2027, 3, 1), ergebnis);
    }
}
