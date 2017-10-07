using UnityEngine;
using System.Collections;

public class DialogDetail {

    public string[] lines;
    private int currentLine = 0;
    public string source;

    /**
     * @NULLABLE
     */
    public string GetNextLine()
    {
        if (currentLine >= lines.Length) return null;
        return lines[currentLine++];
    }

    public string PeekNextLine()
    {
        if (currentLine >= lines.Length) return null;
        return lines[currentLine];
    }

    public void Reset()
    {
        currentLine = 0;
    }

}
