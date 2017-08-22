using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Fit onto FieldCell prefab
public class FieldCell : MonoBehaviour
{
    public TextMesh m_fieldName;
    public TextMesh m_fieldType;

    // full name set by table manager on creation
    public string m_fullName;
    
    /// <summary>
    /// Get the full name of this cell
    /// </summary>
    public string GetFullName() {
        return m_fullName;
    }
}
