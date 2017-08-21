using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TableManager : MonoBehaviour
{
    public Transform FieldCellPrefab;      // used to store cells
    
    public List<Transform> m_fields;    // list of field objects
    public TextMesh m_titleCell;                // Text of object storing the title
    private Transform table;
    private int fieldCount = 0;
    
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
        m_titleCell.text = name;
    }

    /// <summary>
    /// For every field, create a field object, set its contents and local position
    /// </summary>
    /// <param name="fields">Pairs of strings, indicating each field's name and type.</param>
    public void SetFields(List<StrPair> fields) {
        foreach (StrPair pair in fields) {
            Transform cell = Instantiate(FieldCellPrefab, table);
            m_fields.Add(cell);
            FieldCell cellManager = cell.GetComponent<FieldCell>();
            cellManager.m_fieldName.text = pair.field;
            cellManager.m_fieldType.text = pair.type;
            cell.localPosition = new Vector3(0.0f, -fieldCount * FieldCellPrefab.localScale.y, 0.0f);
            fieldCount++;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
