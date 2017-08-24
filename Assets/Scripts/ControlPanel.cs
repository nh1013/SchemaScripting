using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControlPanel : MonoBehaviour
{
    public FileManager fileManager;
    public MappingManager mapManager;

    // menus
    public Transform m_mainMenu;
    public Transform m_filesMenu;
    public Transform m_editMenu;

    // import/export menu objects
    public Dropdown m_sourceSchemaDropdown;
    public Dropdown m_targetSchemaDropdown;
    public Dropdown m_mappingDropdown;

    // edit menu objects
    public Text m_sourceFieldText;
    public Text m_targetFieldText;
    public Text m_beamConfidenceText;

    // selected items
    private Transform m_selectedSourceField;
    private Transform m_selectedTargetField;
    private Transform m_selectedMappingBeam;

    // Use this for initialization
    void Start() {
    }

    /// <summary>
    /// Show the selected menu, by deactivating all others
    /// </summary>
    /// <param name="menu">The menu to be shown.</param>
    public void ShowMenu (Transform menu) {
        if (menu.tag != "Menu") {
            Debug.Log("Error: object not a menu: " + menu.name);
            return;
        }
        m_mainMenu.gameObject.SetActive(false);
        m_filesMenu.gameObject.SetActive(false);
        m_editMenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        return;
    }

    /// <summary>
    /// Sort the selected item into its slot, 
    /// and add/remove others as suitable
    /// </summary>
    /// <param name="item">The object being selected.</param>
    public void Select(Transform item) {
        // if tagged with Sourcefield then
            // if m_selectedSourceField already assigned
                // unselect it and beam
            // assign to m_selectedSourceField
            // if targetfield assigned, then
                // try to select beam between them
        // similarly with targetField item
        // if tagged with beam, then
            // seek out its source and target field cells, and
            // assign them to correct variables
        // updateEditMenu()
    }
    
    /// <summary>
    /// Update the entries in edit menu
    /// </summary>
    public void UpdateEditMenu() {
        m_sourceFieldText.text = (m_selectedSourceField) ? m_selectedSourceField.GetComponent<FieldCell>().m_fullName : "not selected";
        m_targetFieldText.text = (m_selectedTargetField) ? m_selectedTargetField.GetComponent<FieldCell>().m_fullName : "not selected";
        m_beamConfidenceText.text = (m_selectedMappingBeam) ? m_selectedMappingBeam.GetComponent<MappingBeam>().m_confidence.ToString() : "none";
}

    /// <summary>
    /// Import to the source schema manager the schema selected by the control panel
    /// </summary>
    public void ImportSource() {
        string selection = m_sourceSchemaDropdown.captionText.text;
        if (selection == "Select schema") {
            Debug.Log("No database selected");
            return;
        }
        fileManager.ImportSchema(selection, true);
    }

    /// <summary>
    /// Import to the target schema manager the schema selected by the control panel
    /// </summary>
    public void ImportTarget() {
        string selection = m_targetSchemaDropdown.captionText.text;
        if (selection == "Select schema") {
            Debug.Log("No database selected");
            return;
        }
        fileManager.ImportSchema(selection, false);
    }

    /// <summary>
    /// Import the mapping selected by the control panel
    /// </summary>
    public void ImportMapping() {
        string selection = m_mappingDropdown.captionText.text;
        if (selection == "Select mapping") {
            Debug.Log("No database selected");
            return;
        }
        fileManager.ImportMapping(selection);
    }

    /*
    /// <summary>
    /// Select the table passed in
    /// </summary>
    public void SelectTable(RectTransform table)
    {
        selectedTable = table;
        int index = 0;
        foreach (var option in selectTableDropdown.options)
        {
            if (option.text == table.name)
            {
                break;
            }
            index++;
        }
        selectTableDropdown.value = index;
    }*/
}
