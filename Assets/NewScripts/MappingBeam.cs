using UnityEngine;
using System.Collections;

public class MappingBeam : MonoBehaviour 
{
    public Transform m_SourceField;
    public Transform m_TargetField;
    public Transform m_beam;

    // Use this for initialization
    void Start() {
        m_beam = GetComponent<Transform>();
    }

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
        m_beam.position = Vector3.Lerp(sourceNode, targetNode, 0.5f);
        m_beam.LookAt(targetNode);
        m_beam.localScale = new Vector3(m_beam.localScale.x, m_beam.localScale.y,
            Vector3.Distance(sourceNode, targetNode));
    }
}
