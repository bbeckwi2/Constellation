using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System;

//Brandon Beckwith

public class CSVReader {

    private string file;
    private StreamReader reader;

    private char delimeter = ',';
    private string filter = "";

    public List<string> categories;
    public Dictionary<string, List<string>> data;

    public CSVReader(string file) {
        this.file = file;
        reader = new StreamReader(File.OpenRead(file));
        processData();
    }

    public CSVReader(string file, char delimeter, string filter) {
        this.delimeter = delimeter;
        this.file = file;
        this.filter = filter;
        reader = new StreamReader(File.OpenRead(file));
        processData();
    }

    //Processes the csv into a hash of a list of strings
    public void processData() {
        data = new Dictionary<string, List<string>>();
        categories = new List<string>();

        bool first = true;

        int count = 1;

        while (!reader.EndOfStream) {
            string line = reader.ReadLine();
            string[] vals = line.Split(delimeter);

            if (first) {
                first = false;
                foreach (string v in vals) {
                    string clean = Regex.Replace(v, filter, string.Empty);
                    data.Add(clean, new List<string>());
                    categories.Add(clean);
                }
                continue;
            }

            for (int i = 0; i < categories.Count; i++) {
                data[categories[i]].Add(Regex.Replace(vals[i], filter, string.Empty));
            }
            count++;
        }
    }

    public List<Tuple<string, int>> getCounts(string category) {
        if (!categories.Contains(category)) {
            Debug.Log("Category {" + category + "} doesn't exist!");
            throw new Exception("Category doesn't exist");
        }

        Dictionary<string, int> counts = new Dictionary<string, int>();

        foreach (string d in data[category]) {
            if (!counts.ContainsKey(d)) {
                counts.Add(d, 1);
            } else {
                counts[d] += 1;
            }
        }

        List<Tuple<string, int>> cOut = new List<Tuple<string, int>>();

        foreach (string k in counts.Keys) {
            cOut.Add(new Tuple<string, int>(k, counts[k]));
        }
        return cOut;
    }

    //Set's the filter
    public void setFilter(string newFilter) {
        this.filter = newFilter;
    }

    //Set's the delimeter
    public void setDelimeter(char newDelimeter) {
        this.delimeter = newDelimeter;
    }

    //Prints off the categories
    public void debugCategories() {
        foreach (string c in categories) {
            Debug.Log(c);
        }
    }

    //Category debug
    public void debug(string category) {
        if (!categories.Contains(category)) {
            Debug.Log("Category {" + category + "} doesn't exist!");
            return;
        }
        string sOut = "";
        foreach (string s in categories) {
            sOut += s + ", ";
        }
        Debug.Log(sOut);
    }
}