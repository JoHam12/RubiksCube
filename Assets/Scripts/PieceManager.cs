using UnityEngine;

public class PieceManager : MonoBehaviour
{

    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public bool restart = false;
    void Start(){
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    void Update(){
        if(restart){
            ResetPosition();
            restart = false;
        }
    }
    void ResetPosition(){
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
