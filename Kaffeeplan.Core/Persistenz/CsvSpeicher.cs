using Kaffeeplan.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaffeeplan.Core.Persistenz
{
    public class CsvSpeicher
    {
        public static void Exportiere(Jahresplan? plan, string pfad)
        {
            ArgumentNullException.ThrowIfNull(plan);
            var dir = Path.GetDirectoryName(pfad);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            var sb = new StringBuilder();
            sb.AppendLine("KW;Montag;Mitarbeiter;Reinigung;Filtertausch");

            foreach (var e in plan.Eintraege.OrderBy(x => x.Kalenderwoche))
            {
                sb.AppendLine($"{e.Kalenderwoche};{e.MontagDatum:dd.MM.yyyy};" +
                              $"{Maskiere(e.MitarbeiterName)};ja;{(e.HatFiltertausch ? "ja" : "nein")}");
            }
            File.WriteAllText(pfad, sb.ToString());
        }

        public static string Maskiere(string wert)
        {
            if (string.IsNullOrEmpty(wert))
                return string.Empty;
            // ; " \n \r
            // He said "HI" => "He said ""HI"""
            if (wert.Contains(';') || wert.Contains('"') || wert.Contains('\n') || wert.Contains('\r'))
            {
                return "\"" + wert.Replace("\"", "\"\"") + "\"";
            }
            return wert;
        }
    }
}
