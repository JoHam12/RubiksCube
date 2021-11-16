using UnityEngine;
using System.Collections.Generic;

public class CenterManager : MonoBehaviour
{
    [SerializeField] public string tagString;
    [SerializeField] public Vector3 rotationAxis;
    public List<GameObject> objectsBeside =  new List<GameObject>();
    // Get All close pieces to form a side
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject != null){ objectsBeside.Add(other.transform.parent.gameObject); }
    }

    // Remove all pieces that are not close
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.parent.gameObject != null){ objectsBeside.Remove(other.transform.parent.gameObject);}   
    }

}
