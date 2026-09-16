using Kaffeeplan.Core.Model;
using Kaffeeplan.Core.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Kaffeeplan.App.ViewModels
{
    public class MainViewModel : ViewModelBasis
    {
        public int Jahr { get; set; } = 2026;
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
        
        //public ICommand SpeichernCommand { get; }
        //public ICommand LadenCommand { get; }
        //public ICommand CsvExportCommand { get; }
        private readonly PlanungsService _planungsService = new(new KalenderService());

        public MainViewModel()
        {

            foreach (var name in new string[] { "Ralf", "Jochen", "Mario", "Gabriel", "Ehsan", "Shariyar", "Philipp", "Michi", "Bernhard", "Wolfi" })
                Mitarbeiter.Add(new Mitarbeiter { Name = name });
            GewaehlterMitarbeiter = Mitarbeiter[0];
            PlanErzeugenCommand = new RelayCommand(GenerierePlan, () => Mitarbeiter.Count > 1);
            MitarbeiterHinzufuegenCommand = new RelayCommand(fuegeMitarbeiterhinzu, () => Mitarbeiter.Count < 12);
            MitarbeiterEntfernenCommand = new RelayCommand(entferneMitarbeiter, () => Mitarbeiter.Count > 0);
        }

        private void GenerierePlan()
        {
            try
            {
                Statusmeldung = string.Empty;
                var plan = _planungsService.ErzeugePlan(Jahr, Mitarbeiter);
                Planeintraege.Clear();
                foreach (var item in plan.Eintraege)
                {
                    Planeintraege.Add(item);
                }
                GeneriereStatistik();
                Statusmeldung = $"Plan für Jahr: {Jahr} wurde erfolgreich generiert.";
            }
            catch (Exception ex)
            {
                Statusmeldung = ex.Message;
            }
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
    }

    public record StatisticsRow(string Name, int Reinigungen, int FilterTausche);
}