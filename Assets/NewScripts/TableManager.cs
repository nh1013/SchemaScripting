using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TableManager : MonoBehaviour
{
    public Transform FieldCellPrefab;      // used to store cells

    public List<Transform> m_fields;    // list of field objects
    public Transform m_titleCell;       // Transform of field cell with the title

    public bool debugMode = false;
    private string m_tableName;
    private int m_fieldCount = 1;         // start at 1 because of title cell
    
    // Use this for initialization
    void Start() {

    }

    /// <summary>
    /// 
    /// </summary>
    public void Select() {
        // turn on selection in Model Manipulator
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnSelect() {
        // turn off selection in Model Manipulator
    }

    /// <summary>
    /// Set the title text of the table
    /// </summary>
    /// <param name="name">Name of the table.</param>
    public void SetName(string name) {
        m_tableName = name;
        m_titleCell.GetComponent<FieldCell>().m_fieldName.text = name;
        m_titleCell.GetComponent<FieldCell>().m_fullName = name;
    }

    /// <summary>
    /// For every field, create a field object, set its contents and local position
    /// </summary>
    /// <param name="fields">Pairs of strings, indicating each field's name and type.</param>
    public void SetFields(List<StrPair> fields) {
        foreach (StrPair pair in fields) {
            // ignore empty inputs
            if (pair.field.Length == 0 || pair.type.Length == 0) {
                continue;
            }
            Transform cell = Instantiate(FieldCellPrefab, transform);
            m_fields.Add(cell);
            FieldCell cellManager = cell.GetComponent<FieldCell>();
            cellManager.m_fullName = m_tableName + "." + pair.field;
            cellManager.m_fieldName.text = pair.field;
            cellManager.m_fieldType.text = pair.type;
            cell.name = pair.field;
            cell.localPosition = new Vector3(0.0f, -m_fieldCount * FieldCellPrefab.localScale.y, 0.0f);
            m_fieldCount++;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
