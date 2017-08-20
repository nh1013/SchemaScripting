using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemaManager : MonoBehaviour
{
    public Transform TablePrefab;

    public List<Transform> m_tableList;
    // true for source manager, false for target manager
    public bool m_isSource = true;
    private int m_tableCount = 0;
    private float m_bottomSpace = 0;

	// Use this for initialization
	void Start () {
		
	}

    // Load a schema and create tables
    public void LoadSchema(string name, List<Table> table_list ) {

    }

    /// <summary>
    /// Create a new table
    /// </summary>
    /// <param name="name">Name of the table.</param>
    /// <param name="fields">Pairs of strings, indicating each field's name and type.</param>
    void CreateTable(string name, List<StrPair> fields) {
        Transform table = Instantiate(TablePrefab, transform);
        table.position = new Vector3(0.0f, m_bottomSpace, 0.0f);
        TableManager tabMan = table.GetComponent<TableManager>();
        tabMan.SetName(name);
        tabMan.SetFields(fields);
        //m_bottomSpace -=
    }

    // Update is called once per frame
    void Update () {
		
	}
}
