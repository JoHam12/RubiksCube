using UnityEngine;
using UnityEngine.UI;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Constants constants; 
    [SerializeField] private float distance = 7;
    [SerializeField] private Transform cubeCenter; 
    private float speed = 2f;
    [SerializeField] public ReadFile fileReader;
    [SerializeField] private Toggle useJoystickToggle;
    public RawImage moveableHand;
    [SerializeField] private float height, width; 
    private float handPosX, handPosY;
    private bool canRotate = true;
    private bool handTrackerMode = false;
    void Awake(){
        useJoystickToggle.isOn = false;
        moveableHand.gameObject.SetActive(false);
        RectTransform cnvTransform = moveableHand.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        height = cnvTransform.rect.height;
        width = cnvTransform.rect.width;
        handPosX = width/2; handPosY = height/2;
    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape)){
            fileReader.QuitHandTracker();
            Application.Quit();
        }

        //Get the center of the cube
        cubeCenter = GameObject.Find("Middle").transform;

        // Activate HandTracker
        if(useJoystickToggle.isOn && !handTrackerMode){
            handTrackerMode = true;
            fileReader.StartHandTracker();
            moveableHand.gameObject.SetActive(true);
        }

        //Deactivate HandTracker
        if(handTrackerMode && !useJoystickToggle.isOn){
            handTrackerMode = false;
            moveableHand.gameObject.SetActive(false);
            fileReader.QuitHandTracker();
            
        }
        
        float horizontal = 0;
        float vertical = 0;
        
        float isOpen;

        // Get Values from file 
        if(handTrackerMode && fileReader.values.Count >= constants.NUMINDICES){
            isOpen = fileReader.values[constants.OPENHANDINDEX];
            if(isOpen == 0){ canRotate = true; }
            else{ canRotate = false; }
            horizontal = fileReader.values[constants.XVALUEINDEX];
            vertical = fileReader.values[constants.YVALUEINDEX];
            handPosX = 1 - fileReader.values[constants.XHANDINDEX];
            handPosY = 1 - fileReader.values[constants.YHANDINDEX];
            moveableHand.transform.position = new Vector3(handPosX * width, handPosY * height);
        }

        // Get Values from mouse Input
        if(!handTrackerMode && Input.GetMouseButton(1)){
            canRotate = true;
            horizontal = Input.GetAxisRaw("Mouse X");
            vertical = Input.GetAxisRaw("Mouse Y");
        }

        // Rotate Cube
        if(canRotate){
            transform.position = cubeCenter.position - (transform.forward * distance);
            transform.RotateAround(cubeCenter.position, Vector3.up, horizontal * speed);
            transform.RotateAround(cubeCenter.position, transform.right, vertical * speed);
        }
        
    }
        
}