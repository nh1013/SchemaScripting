using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct StrPair {
    public string field;
    public string type;

    public StrPair (string _field, string _type) {
        field = _field;
        type = _type;
    }
}

public struct Table {
    public string name;
    public List<StrPair> fields;

    public Table(string _name, List<StrPair> _fields) {
        name = _name;
        fields = _fields;
    }
}

public class FileManager : MonoBehaviour {

    public SchemaManager SourceManager;
    public SchemaManager TargetManager;
    public MappingManager MapManager;

    // Use this for initialization
    void Start () {
        RefreshFiles("Schemas");
        RefreshFiles("Mappings");
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
        // put list of fileNames onto control panel
        return fileNames;
    }

    void ImportSchema(string fileName, bool SchemaType) {
        // read file
        // parse through file to get 
        // database name
        // tables: table name, and pairs of fields/field types
        // ignore constraints
        // recognise end of table
        // recognise end of file

        // depending on if SchemaType is SOURCE or TARGET,
        // load schema in correct schema manager
    }
    
    void ImportMapping() {
        // line 1: "MatchResult", unknown brackets
        // line 2 and 3: file path of source and target
        // line 4: filler, 56 "-"s
        // use string.StartsWith(" - ") or " + " to determine end
        //  - [table].[field] <-> [table].[field]: [confidence]
        //  + Total: [number of] correspondences
        // end filler, 56 "-"s

        // send each mapping to mapping manager, call link(table, field, table2, field2)
        // on fail, clear existing mapping, log error
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
