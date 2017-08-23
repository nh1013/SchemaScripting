﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using UnityEngine;

public struct StrPair {
    public string field;
    public string type;

    public StrPair (string _field, string _type) {
        field = _field;
        type = _type;
    }
}

/*
public struct Table {
    public string name;
    public List<StrPair> fields;

    public Table(string _name, List<StrPair> _fields) {
        name = _name;
        fields = _fields;
    }
}
*/

public class FileManager : MonoBehaviour {

    public SchemaManager SourceManager;
    public SchemaManager TargetManager;
    public MappingManager MapManager;

    public bool debugMode = false;
    private string m_sourceAddress;
    private string m_targetAddress;

    // Use this for initialization
    void Start () {
        RefreshFiles("Schemas");
        RefreshFiles("Mappings");
        // for testing
        ImportSchema("personCarCity-source", true);
        ImportSchema("personCarCity-target", false);
        ImportMapping("MatchResult1");
    }

    List<string> RefreshFiles (string folder) {
        DirectoryInfo dir = new DirectoryInfo(folder + "/");
        List<string> fileNames = new List<string> { };
        FileInfo[] info;
        if (folder == "Schemas") {
            info = dir.GetFiles("*.sql");
        }
        else if (folder == "Mappings") {
            info = dir.GetFiles("*.txt");
        }
        else {
            Debug.Log("RefreshFiles: folder not recognised: " + folder);
            return null;
        }

        foreach (FileInfo file in info)
        {
            fileNames.Add(Path.GetFileNameWithoutExtension(file.ToString()));
        }
        if (debugMode) {
            foreach (string fileName in fileNames) {
                Debug.Log(fileName);
            }
        }
        // put list of fileNames onto control panel
        return fileNames;
    }

    void ImportSchema(string fileName, bool isSourceType) {
        string path = "Schemas/" + fileName + ".sql";
        if (!File.Exists(path)) {
            Debug.Log("file not found: " + path);
            return;
        }
        
        SchemaManager schemaManager = TargetManager;
        if (isSourceType) {
            schemaManager = SourceManager;
        }
        
        schemaManager.ClearSchema();
        schemaManager.m_schemaName = path;
        StreamReader sr = new StreamReader(path);
        Regex fieldRX = new Regex(@"\A\(?\s*(\w+)\s*(\w+\(?\d*\)?).*");
        CultureInfo invarCult = System.Globalization.CultureInfo.InvariantCulture;

        while (sr.Peek() > -1) {
            string line = sr.ReadLine();
            if (line.StartsWith(@"CREATE DATABASE", true, invarCult)) {
                char[] seperators = { ' ', ';' };
                string[] words = line.Split(seperators);
                // check if database name is already set
                if (schemaManager.m_databaseName.Length != 0) {
                    Debug.Log("Error: second database name detected: " + words[2]);
                    Debug.Log("Aborting schema import...");
                    schemaManager.ClearSchema();
                    sr.Close();
                    return;
                }
                schemaManager.m_databaseName = words[2];
                if (debugMode) {
                    Debug.Log("database name: " + words[2]);
                }
                continue;
            }

            // skip ahead until creating a table, then capture table info
            if (!line.StartsWith(@"CREATE TABLE", true, invarCult)) {
                continue;
            }
            
            // get table name
            string tableName = Regex.Match(line, @"(?i)CREATE TABLE (\w*)").Groups[1].Value;
            if (debugMode) {
                Debug.Log("table name: " + tableName);
            }
            List<StrPair> fieldPairs = new List<StrPair>();
            // iterate reader to get list of field names and types
            while (sr.Peek() > -1) {
                string fieldLine = sr.ReadLine();
                // skip CONSTRAINT lines
                if (fieldLine.Contains("CONSTRAINT") || fieldLine.Contains("PRIMARY")) {
                    break;
                }
                Match match = fieldRX.Match(fieldLine);
                if (debugMode) {
                    for (int i = 1; i <= 2; i++) {
                        Debug.Log(match.Groups[i].Value);
                    }
                }
                fieldPairs.Add(new StrPair(match.Groups[1].Value, match.Groups[2].Value));
                // end of table creation
                if (fieldLine.EndsWith(";")) {
                    break;
                }
            }
            schemaManager.CreateTable(tableName, fieldPairs);
        }
        if (schemaManager.m_databaseName.Length == 0) {
            Debug.Log("Note: Database name not found for: " + path);
        }
        sr.Close();
    }

    /// <summary>
    /// Import the mapping and load it into Map Manager
    /// </summary>
    /// <param name="fileName">Name of the mapping text file.</param>
    void ImportMapping(string fileName) {
        string path = "Mappings/" + fileName + ".txt";
        if (!File.Exists(path)) {
            Debug.Log("file not found: " + path);
            return;
        }

        MapManager.ClearBeams();
        StreamReader sr = new StreamReader(path);
        sr.ReadLine(); // discard first line
        m_sourceAddress = sr.ReadLine();
        m_targetAddress = sr.ReadLine();
        sr.ReadLine(); // discard divider

        while (sr.Peek() > -1) {
            string line = sr.ReadLine();
            if (debugMode) {
                Debug.Log(line);
            }
            if (line.StartsWith(@" + ")) {
                break;
            }
            // input format: - [table].[field] <-> [table].[field]: [confidence]
            // 2nd field may not exist - interpreted as empty strings
            Regex mapRX = new Regex(@"\s\-\s(\w*)\.?(\w*)\s.{3}\s(\w*)\.?(\w*)\:\s(\d*\.\d*)");
            Match match = mapRX.Match(line);
            if (debugMode) {
                for (int i = 1; i <= 5; i++) {
                    Debug.Log(match.Groups[i].Value);
                }
            }
            // send each mapping to mapping manager, call AddBeam(table, field, table2, field2, confidence)
            bool addSuccess = MapManager.AddBeam(
                match.Groups[1].Value, 
                match.Groups[2].Value, 
                match.Groups[3].Value, 
                match.Groups[4].Value,
                float.Parse(match.Groups[5].Value, System.Globalization.CultureInfo.InvariantCulture)
            );
            // on fail, clear existing mapping, log error
            if (!addSuccess) {
                Debug.Log("Import failed, clearing existing mapping");
                MapManager.ClearBeams();
                break;
            }
        }
        sr.Close();
    }

    /// <summary>
    /// Export the mapping into an unused file in folder Mappings/
    /// </summary>
    void ExportMapping() {
        // find unused filename
        int count = 0;
        string path = "Mappings/mathResult" + count + ".txt";
        while (File.Exists(path)) {
            count++;
            path = "Mappings/mathResult" + count + ".txt";
        }
        StreamWriter sw = new StreamWriter(path);

        // export in format as COMA requires
        sw.WriteLine("MatchResult [16,18]");
        sw.WriteLine(SourceManager.m_schemaName);
        sw.WriteLine(TargetManager.m_schemaName);
        sw.WriteLine("--------------------------------------------------------");
        // for each connection in MappingManager,
        // output beam.source.getName() + " <-> " + beam.target.getName() + ": " + beam.confidence;
        foreach (Transform beam in MapManager.m_BeamList) {
            string beamSourceName = beam.GetComponent<MappingBeam>().m_SourceField.GetComponent<FieldCell>().GetFullName();
            string beamTargetName = beam.GetComponent<MappingBeam>().m_TargetField.GetComponent<FieldCell>().GetFullName();
            float beamConfidence = beam.GetComponent<MappingBeam>().m_confidence;
            sw.WriteLine(" - " + beamSourceName + " <-> " + beamTargetName + ": " + beamConfidence);
        }
        sw.WriteLine(" + Total: " + MapManager.m_BeamList.Count + " correspondences");
        sw.WriteLine("--------------------------------------------------------");
        sw.Close();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
