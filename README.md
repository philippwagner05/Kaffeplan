# Kaffeemaschinen-Dienst-Planer

## Inhaltsverzeichnis
1. Beschreibung des Programms.
2. Inbetriebnahme des Programms.
3. Ausführung der programmeigenen Tests.
4. Funktionsweise des Planungsalgorithmus.
5. Noch fehlenden Funktionen und Einschränkungen des Programms.

## Beschreibung des Programms
Das Programm Kaffeemaschinen-Dienst-Planer ist dafür da, einen Fair verteilten Dienstplan für die Reinigung und Filtertausche einer Kaffeemaschine zu erstellen. Man muss die Namen der Mitarbeiter hinzufügen bzw. entfernen und ein gültiges Jahr angeben, um einen Plan zu erzeugen.
Diesen Plan kann man dann speichern und ihn später wieder in Programm laden oder ihn als CSV-Datei exportieren um ihn z.B. in Excel öffnen, bearbeiten oder drucken zu können.

## Inbetriebnahme des Programms
1. Die Datei Kaffeeplan.slnx in MS VisualStudio 2026 öffnen.
2. In der oberen Leiste Kaffeplan.App neben dem grünen Dreieck zum Starten drücken oder F5 auf der Tastatur betätigen.
Das Programm sollte ohne Fehler gebaut werden und es sollte sich ein Fenster öffnen.

## Ausführung der programmeigenen Tests
1. Die Datei Kaffeeplan.slnx in MS VisualStudio 2026 öffnen.
2. Im oberen Ribbon unter Test -> TestExplorer, den TestExplorer öffnen
3. Oben rechts in der Ecke auf "Alle Tests in der Ansicht Ausführen" klicken
Alle Tests sollen ausgeführt werden und mit grün markiert werden und somit bestanden sein.

## Funktionsweise des Planungsalgorithmus
Der Algorithmus in der Klasse PlanungsService.cs funktioniert wie folgt:
Die Mitarbeiter für die Filterwochen, werden ersteinmal Reihum verteilt.
Danach wird für jede freie Woche entschieden wer dran kommt. Wer in der Filterwoche dran ist scheidet in der Woche davor und danach sofort aus. Unter den noch übrig gebliebenen, gewinnt der, der am wenigsten Reinigungen hat Rückfall: Wenn dabei niemand übrig bleibt, wird nur die Vorwoche gesperrt, damit das Programm nicht abstürzt.

## Noch fehlende Funktionen und Einschränkungen des Programms
1. Der Planungsalgorithmus verteilt die Personen nur pro Kalenderjahr gleichmäßig und fair. Bei bestimmten Mitarbeiteranzahlen kommen einige Personen jedes Jahr aufs Neue 2-mal dran, was es über Jahre hinweg unfair macht.
2. Die CSV-Datei wird immer in den Ordner %AppData%\\Kaffeplan\\Data\\CSV gespeichert. Der Nutzer kann selbst nicht aussuchen, wo er diese speichern möchte.
