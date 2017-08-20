using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MappingManager : MonoBehaviour
{
    public Transform BeamPrefab;

    public List<Transform> m_BeamList;

    // Use this for initialization
    void Start() {

    }

    // in-game addition of a beam, via selecting two fields
    bool AddBeam(Transform sourceField, Transform targetField) {
        // should check if beam already exists
        
        // add the beam
        Transform beam = Instantiate(BeamPrefab, transform);
        m_BeamList.Add(beam);
        beam.GetComponent<MappingBeam>().m_SourceField = sourceField;
        beam.GetComponent<MappingBeam>().m_TargetField = targetField;
        return true;
    }

    // in-game removal of a beam, via selecting the beam's end nodes
    bool RemoveBeam(Transform sourceField, Transform targetField) {
        string sourceName = sourceField.GetComponent<FieldCell>().GetFullName();
        string targetName = targetField.GetComponent<FieldCell>().GetFullName();
        foreach (Transform beam in m_BeamList) {
            string beamSourceName = beam.GetComponent<MappingBeam>().m_SourceField.GetComponent<FieldCell>().GetFullName();
            if (sourceName != beamSourceName) {
                continue;
            }
            string beamTargetName = beam.GetComponent<MappingBeam>().m_TargetField.GetComponent<FieldCell>().GetFullName();
            if (targetName != beamTargetName) {
                continue;
            }
            // beam matches target for removal
            m_BeamList.Remove(beam);
            Destroy(beam);
            return true;
        }
        // no matches
        Debug.Log("Error: could not locate beam between " + sourceName + " and " + targetName);
        return false;
    }

    // in-game removal of a beam, via selecting the beam directly
    bool RemoveBeam(Transform beam) {
        // check if item is a beam
        if (!m_BeamList.Contains(beam)) {
            Debug.Log("Error: target object for deletion is not a beam, name: " + beam.name);
            return false;
        }
        // remove this beam from the list
        m_BeamList.Remove(beam);
        Destroy(beam);
        return true;
    }


    public void ClearHeader() {
        for (int i = headerRow.childCount - 1; i >= 0; i--) {
            Destroy(headerRow.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
