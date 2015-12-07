using System.Collections.Generic;

public static class Kombinatorik
{
    /// <summary>
    /// Generiert ein Array, aus Elementen die jeweils aus 'chars' unterschiedlichen Zeichen bestehen, mit jeweils 'places' Stellen.
    /// Das Array beinhaltet alle möglichen Verknüpfungsmöglichkeiten, die durch Permutation ermittelt werden.
    /// </summary>
    /// <param name="places">Anzahl der Stellen jedes Elements</param>
    /// <param name="chars">Array von Zeichen die benutzt werden dürfen</param>
    /// <returns>Ein string-Array mit allen unterschiedlichen Verknüpfungsmöglichkeiten</returns>
    public static string[] GetPermutation(int places, string[] chars)
    {
        // Eine neue, leere ArrayList generieren, an die alle Möglichkeiten angehängt werden
        List<string> output = new List<string>();
        GetPermutationPerRef(places, chars, output);

        // Das Ergebnis in einen string[] umwandeln und zurückgeben
        return output.ToArray();
    }

    /// <summary>
    /// Generiert ein Array, aus Elementen die jeweils aus 'chars' unterschiedlichen Zeichen bestehen, mit jeweils 'places' Stellen.
    /// Das Array beinhaltet alle möglichen Verknüpfungsmöglichkeiten, die durch Permutation ermittelt werden.
    /// Das Ergebnis wird in der als Referenz übergebenen ArrayList 'output' gespeichert.
    /// </summary>
    /// <param name="places">Anzahl der Stellen jedes Elements</param>
    /// <param name="chars">Array von Zeichen die benutzt werden dürfen</param>
    /// <param name="output">ArrayList in die alle Möglichkeiten hinzugefügt werden</param>
    /// <param name="outputPart">Optionaler interner Parameter, zur Weitergabe der Informationen während des rekursiven Vorgangs</param>
    private static void GetPermutationPerRef(int places, string[] chars, List<string> output, string outputPart = "")
    {
        if (places == 0)
        {
            // Wenn die Anzahl der Stellen durchgerechnet wurde,
            // wird der sich ergebende string (Element) an die Ausgabe angehängt.
            output.Add(outputPart);
        }
        else
        {
            // Für die Stelle rechts im Element, werden alle Zeichenmöglichkeiten durchlaufen
            foreach (string c in chars)
            {
                // Danach wird für jedes dieser Zeichen, basierend auf der Anzahl der Stellen, wieder ein neuer
                // foreach-Vorgang begonnen, der alle Zeichen der nächsten Stelle hinzufügt

                GetPermutationPerRef(places - 1,   // Die Stellen Anzahl wird verwindert, bis 0
                    chars,                         // Benötigte Variablen werden
                    output,                    // mitübergeben
                    outputPart + c);               // An diesen letzen string werden alle anderen Stellen angehängt
            }
        }
    }

}
