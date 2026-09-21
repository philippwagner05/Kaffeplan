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
Für jeden Mitarbeiter in der Liste gibt es einen durchlauf in dem geprüft wird, ob der jetzt ausgewählte Mitarbeiter besser geeignet ist als der zuvor als bestes ausgewählt wurde.
Wer passenderer ist wird durch die Anzahl der Filtertausche, der Reinigungen und der Dauer, in der Sie nicht mehr dran waren, ermittelt.
Genauer funktioniert das so:
Wenn der ausgewählt Mitarbeiter weniger Filtertausche hat als der vorher am besten geeignet, wird er zum Besten geeigneten. Dies wird aber nur in der Filterwoche berücksichtigt.
Wenn der ausgewählt Mitarbeiter weniger Filtertausche und weniger Reinigungen hat als der vorher am besten geeignete, wird er zum Besten geeigneten.
Wenn der ausgewählt Mitarbeiter weniger Filtertausche, weniger Reinigungen und länger nicht dran war als der zuvor am besten geeignete, wird er zum Besten geeigneten.
Falls dies alles nicht zutrifft, bleibt der zuvor als bester ausgewählte einfach weiterhin der am besten geeignete. 
Nachdem dieser Vergleich für jeden Mitarbeiter in der Liste geschehen ist, wird die Person, die am besten für diese Woche geeignet ist in den Plan eingetragen. 

## Noch fehlende Funktionen und Einschränkungen des Programms
1. Der Planungsalgorithmus verteilt die Personen nur pro Kalenderjahr gleichmäßig und fair. Bei bestimmten Mitarbeiteranzahlen kommen einige Personen jedes Jahr aufs Neue 2-mal dran, was es über Jahre hinweg unfair macht.
2. Die CSV-Datei wird immer in den Ordner \\Kaffeeplan.Core\\Persistenz\\Data\\CSV gespeichert. Der Nutzer kann selbst nicht aussuchen, wo er diese speichern möchte.
