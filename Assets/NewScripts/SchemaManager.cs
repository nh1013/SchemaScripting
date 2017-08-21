using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemaManager : MonoBehaviour
{
    public Transform TablePrefab;

    public List<Transform> m_tableList;
    // true for source manager, false for target manager
    public bool m_isSource = true;
    public string m_schemaName;
    private int m_tableCount = 0;
    private float m_bottomSpace = 0;

	// Use this for initialization
	void Start () {
		
	}

    /// <summary>
    /// Load a schema and create tables
    /// </summary>
    /// <param name="name">Name of the schema.</param>
    /// <param name="fields">List of tables to be created and managed.</param>
    public void LoadSchema(string name, List<Table> table_list ) {
        ClearSchema();
        m_schemaName = name;
        foreach (Table table in table_list) {
            m_tableList.Add(CreateTable(table.name, table.fields));
            m_tableCount++;
        }
    }

    /// <summary>
    /// Clean up Schema
    /// </summary>
    private void ClearSchema() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        m_tableList.Clear();
        m_schemaName = "";
        m_tableCount = 0;
        m_bottomSpace = 0;
    }

    /// <summary>
    /// Create a new table
    /// </summary>
    /// <param name="name">Name of the table.</param>
    /// <param name="fields">Pairs of strings, indicating each field's name and type.</param>
    Transform CreateTable(string name, List<StrPair> fields) {
        Transform table = Instantiate(TablePrefab, transform);
        table.position = new Vector3(0.0f, m_bottomSpace, 0.0f);
        table.GetComponent<TableManager>().SetName(name);
        table.GetComponent<TableManager>().SetFields(fields);
        //m_bottomSpace -=
        return table;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
