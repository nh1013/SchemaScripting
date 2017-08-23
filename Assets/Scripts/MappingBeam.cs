﻿using UnityEngine;
using System.Collections;

public class MappingBeam : MonoBehaviour 
{
    public Transform m_SourceField;
    public Transform m_TargetField;

    private Transform m_SourceBox;
    private Transform m_TargetBox;

    public bool debugMode = false;
    public float m_confidence = 1.0f;

    /// <summary>
    /// Set the nodes of the beam, and extract the node's box mesh
    /// </summary>
    /// <param name="sourceField">Transform of source side field.</param>
    /// <param name="targetField">Transform of target side field.</param>
    public void SetValues(Transform sourceField, Transform targetField, float confidence) {
        m_SourceField = sourceField;
        m_TargetField = targetField;
        m_confidence = confidence;

        m_SourceBox = sourceField.GetComponent<FieldCell>().m_boxMesh;
        m_TargetBox = targetField.GetComponent<FieldCell>().m_boxMesh;
    }

    // Update is called once per frame
    void Update() {
    Vector3 sourceNode = new Vector3(
        m_SourceBox.position.x + 0.5f * m_SourceBox.lossyScale.x,
        m_SourceBox.position.y,
        m_SourceBox.position.z);
    Vector3 targetNode = new Vector3(
        m_TargetBox.position.x - 0.5f * m_TargetBox.lossyScale.x,
        m_TargetBox.position.y,
        m_TargetBox.position.z);
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
