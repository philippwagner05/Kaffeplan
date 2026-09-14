using System.Globalization;
using System.IO.IsolatedStorage;

namespace Kaffeeplan.Core.Services;

public class KalenderService
{
    public int WochenImJahr(int jahr)
    {
        if(jahr < 1 || jahr > 9999)
        {
            throw new ArgumentOutOfRangeException();
        }
        return ISOWeek.GetWeeksInYear(jahr);
    }

    public DateOnly MontageDerWoche(int jahr, int kalenderwoche)
    {
        var check = WochenImJahr(jahr);
        if(kalenderwoche > check || kalenderwoche < 1)
        {
            throw new ArgumentOutOfRangeException();
        }
        var mo = ISOWeek.ToDateOnly(jahr, kalenderwoche, DayOfWeek.Monday);
        return mo;
    }
}
