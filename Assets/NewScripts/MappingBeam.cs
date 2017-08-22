using UnityEngine;
using System.Collections;

public class MappingBeam : MonoBehaviour 
{
    public Transform m_SourceField;
    public Transform m_TargetField;

    public bool debugMode = false;
    public float m_confidence = 1.0f;

    // Update is called once per frame
    void Update() {
        Vector3 sourceNode = new Vector3(
            m_SourceField.position.x + 0.5f * m_SourceField.localScale.x, 
            m_SourceField.position.y, 
            m_SourceField.position.z);
        Vector3 targetNode = new Vector3(
            m_TargetField.position.x - 0.5f * m_TargetField.localScale.x,
            m_TargetField.position.y, 
            m_TargetField.position.z);
        // update beam position, orientation, and size (scale)
        transform.position = Vector3.Lerp(sourceNode, targetNode, 0.5f);
        transform.LookAt(targetNode);
        transform.localScale = new Vector3(
            transform.localScale.x, 
            transform.localScale.y,
            Vector3.Distance(sourceNode, targetNode)
        );
    }
}
