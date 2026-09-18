using Kaffeeplan.Core;
using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Persistenz;
using Kaffeeplan.Core.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Kaffeeplan.App.ViewModels
{
    public class MainViewModel : ViewModelBasis
    {
        public int Jahr { get; set; } = 2026;
        private Jahresplan? plan;
        public Mitarbeiter GewaehlterMitarbeiter
        {
            get;
            set => SetzeWert(ref field, value);
        }
        public ObservableCollection<Mitarbeiter> Mitarbeiter { get; set; } = [];
        public ObservableCollection<Planeintrag> Planeintraege { get; set; } = [];
        public ObservableCollection<StatisticsRow> Statistik { get; set; } = [];
        public string Statusmeldung
        {
            get;
            set => SetzeWert(ref field, value);
        } = string.Empty;
        public ICommand PlanErzeugenCommand { get; }
        public ICommand MitarbeiterHinzufuegenCommand { get; }
        public ICommand MitarbeiterEntfernenCommand { get; }

        public ICommand SpeichernCommand { get; }
        public ICommand LadenCommand { get; }
        public ICommand PlanLoeschenCommand { get; }
        public ICommand CsvExportCommand { get; }
        private readonly PlanungsService _planungsService = new(new KalenderService());
        private readonly JsonSpeicher _jsonSpeichern = new();
         //private readonly CsvSpeicher _csvSpeicher = new();
        public MainViewModel()
        {
            //var plan = _jsonSpeichern.Laden<Jahresplan>($"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\JsonPlan-{Jahr}.json");
            foreach (var name in new string[] { "Ralf", "Jochen", "Mario", "Gabriel", "Ehsan", "Shariyar", "Philipp", "Michi", "Bernhard", "Wolfi" })
                Mitarbeiter.Add(new Mitarbeiter { Name = name });
            GewaehlterMitarbeiter = Mitarbeiter[0];
            PlanErzeugenCommand = new RelayCommand(GenerierePlan, () => Mitarbeiter.Count > 1);
            MitarbeiterHinzufuegenCommand = new RelayCommand(fuegeMitarbeiterhinzu, () => Mitarbeiter.Count < 12);
            MitarbeiterEntfernenCommand = new RelayCommand(entferneMitarbeiter, () => Mitarbeiter.Count > 0);
            SpeichernCommand = new RelayCommand(SpeicherePlan, () => plan != null);
            LadenCommand = new RelayCommand(LadePlan, () => true);
            PlanLoeschenCommand = new RelayCommand(LöschePlan, () => true);
            CsvExportCommand = new RelayCommand(CSVExport, () => plan != null);
        }

        private void LadePlan()
        {

            try
            {
                plan = _jsonSpeichern.Laden<Jahresplan>($"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\JSON\\JsonPlan-{Jahr}.json");
                if (plan == null)
                    Statusmeldung = $"Plan für das Jahr {Jahr} konnte nicht geladen werden.";
                else
                {
                    PlanAnzeigen(plan);
                    Statusmeldung = $"Plan für das Jahr {Jahr} wurde erfolgreich geladen.";
                    AktualiesiereMitarbeiterListe(plan);
                }
            }
            catch (Exception ex)
            {
                Statusmeldung = ex.Message;
            }
        }

        private void AktualiesiereMitarbeiterListe(Jahresplan plan)
        {
            Mitarbeiter.Clear();
            foreach (var item in plan.Eintraege)
            {
                var name = item.MitarbeiterName;
                //if (!Mitarbeiter.Any(m => m.Name == name))
                //{
                //    Mitarbeiter.Add(new Mitarbeiter { Name = name });
                //}
                bool hinzufuegen = true;
                foreach (var check in Mitarbeiter)
                {
                    if (check.Name == name)
                        hinzufuegen = false; ;
                }
                if (hinzufuegen)
                    Mitarbeiter.Add(new Mitarbeiter { Name = name });
            }
            GeneriereStatistik();
        }

        private void SpeicherePlan()
        {
            try
            {
                _jsonSpeichern.Speichern(plan, $"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\JSON\\JsonPlan-{Jahr}.json");
                Statusmeldung = $"Plan für Jahr {Jahr} wurde erfolgreich gespeichert.";
            }
            catch (Exception ex)
            {
                Statusmeldung = ex.Message;
            }
        }

        private void LöschePlan()
        {
            if (File.Exists($"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\JSON\\JsonPlan-{Jahr}.json"))
            {
                File.Delete($"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\JSON\\JsonPlan-{Jahr}.json");
                Statusmeldung = $"Die Datei JsonPlan-{Jahr}.json wurde erfolgreich gelöscht.";
            }
            else
                Statusmeldung = $"Die Datei JsonPlan-{Jahr} konnte nicht gefunden werden.";
        }

        private void GenerierePlan()
        {
            try
            {
                Statusmeldung = string.Empty;
                plan = _planungsService.ErzeugePlan(Jahr, Mitarbeiter);
                PlanAnzeigen(plan);
                Statusmeldung = $"Plan für Jahr {Jahr} wurde erfolgreich generiert.";
            }
            catch (Exception ex)
            {
                Statusmeldung = ex.Message;
            }
        }

        private void PlanAnzeigen(Jahresplan jahresplan)
        {
            Planeintraege.Clear();
            foreach (var item in jahresplan.Eintraege)
            {
                Planeintraege.Add(item);
            }
            GeneriereStatistik();
        }

        private void GeneriereStatistik()
        {
            var plan = _planungsService.ErzeugePlan(Jahr, Mitarbeiter);
            var reinungenpromitarbeiter = plan.ReinigungProMitarbeiter();
            var filtertauschpromitarbeiter = plan.FiltertauschProMitarbeiter();

            Statistik.Clear();

            foreach (var item in Mitarbeiter)
            {
                Statistik.Add(new StatisticsRow(item.Name, reinungenpromitarbeiter[item.Name], filtertauschpromitarbeiter[item.Name]));
            }
        }

        private void fuegeMitarbeiterhinzu()
        {
            string neuermitarbeiter = Interaction.InputBox("Geben Sie den Names des zu hinzufügenden Mitarbeiters ein:", "Mitarbeiter hinzufügen", "Name des Mitarbeiters");
            foreach (var item in Mitarbeiter)
            {
                if (item.Name == neuermitarbeiter)
                {
                    Statusmeldung = "Mitarbeiter exestiert schon!";
                    return;
                }
            }
            Statusmeldung = "Mitarbeiter erfolgreich hinzugefügt.";
            Mitarbeiter.Add(new Mitarbeiter { Name = neuermitarbeiter });
        }

        private void entferneMitarbeiter()
        {
            Mitarbeiter.Remove(GewaehlterMitarbeiter);
        }

        public void CSVExport()
        {
            try
            {
                CsvSpeicher.Exportiere(plan, $"C:\\Users\\wagner_p\\Documents\\Philipp Wagner\\Kaffeplan\\03_Code\\Stage1-Classic\\Kaffeeplan.Core\\Persistenz\\Data\\CSV\\CSVExport-{Jahr}.csv");
                Statusmeldung = $"Plan für Jahr {Jahr} wurde erfolgreich als CSV-Datei exportiert.";
            }
            catch (Exception ex)
            {
                Statusmeldung = ex.Message;
            }
        }
    }

    public record StatisticsRow(string Name, int Reinigungen, int FilterTausche);
}