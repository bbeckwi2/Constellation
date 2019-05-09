using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public int CHARS_PER_LINE = 25;
    public int CHARS_TITLE = 12;
    public int MAX_LINES = 8;


    public GameObject titleDisplay;
    public GameObject mainDisplay;

    private TextMesh titleMesh;
    private TextMesh mainMesh;

    /* Sets the title text, returns false if the text is too big (sets anyways) */
    public bool setTitle(string title) {
        titleMesh.text = title;

        if (title.Length > CHARS_TITLE) {
            return false;
        }
        return true;
    }

    /* Sets the main text, returns false if the text is too big (sets anyways) */
    public bool setText(string text) {

        mainMesh.text = text;

        string[] lines = text.Split(
            new[] { System.Environment.NewLine },
            System.StringSplitOptions.None
        );

        if (lines.Length > MAX_LINES) {
            return false;
        }

        foreach (string l in lines) {
            if (l.Length > CHARS_PER_LINE) {
                return false;
            }
        }

        return true;
    }

    /* Attempts to format the text for the main display, clips extra lines */
    public string formatTextForMain(string text) {
        string outString = "";
        int charCount = 0;
        int lineCount = 0;
        foreach (string s in text.Split(' ')) {
            if (charCount + s.Length > CHARS_PER_LINE) {
                outString += System.Environment.NewLine + s;
                charCount = s.Length;
                lineCount++;
            } else if (lineCount == MAX_LINES){
                outString += " ...";
            } else {
                charCount += s.Length;
                outString += ' ' + s;
            }
        }
        return outString;
    }

    // Start is called before the first frame update
    void Start() {
        this.titleMesh = titleDisplay.GetComponent<TextMesh>();
        this.mainMesh = mainDisplay.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
