using UnityEngine;
using UnityEngine.UI;

public class HandActionController : MonoBehaviour
{
    [SerializeField] private Button RButton, OButton, GButton, BButton, WButton, YButton;
    private RectTransform RTransform, OTransform, GTransform, BTransform, WTransform, YTransform; 
    private RectTransform handTransform;
    private bool select = false;
    [SerializeField] private Constants constants;
    [SerializeField] private ReadFile fileReader;
    void Start(){
        handTransform = GetComponent<RectTransform>();

        RTransform = RButton.GetComponent<RectTransform>();
        OTransform = OButton.GetComponent<RectTransform>();
        GTransform = GButton.GetComponent<RectTransform>();
        BTransform = BButton.GetComponent<RectTransform>();
        WTransform = WButton.GetComponent<RectTransform>();
        YTransform = YButton.GetComponent<RectTransform>();

    }

    void Update(){
        select = false;
        if(fileReader.values.Count >= constants.NUMINDICES && fileReader.values[constants.SELECT] != 0){ select = true; }
        if(select){
            if(isOnButton(RTransform)){ RButton.onClick.Invoke(); }
            if(isOnButton(OTransform)){ OButton.onClick.Invoke(); }
            if(isOnButton(GTransform)){ GButton.onClick.Invoke(); }
            if(isOnButton(BTransform)){ BButton.onClick.Invoke(); }
            if(isOnButton(WTransform)){ WButton.onClick.Invoke(); }
            if(isOnButton(YTransform)){ YButton.onClick.Invoke(); }
        }
        
    }
    
    // Check if hand is on a button
    bool isOnButton(RectTransform buttonTransform){
        if(handTransform.position.x >= buttonTransform.position.x - buttonTransform.rect.width/2 &&
         handTransform.position.x <= buttonTransform.position.x + buttonTransform.rect.width/2 && 
         handTransform.position.y >= buttonTransform.position.y - buttonTransform.rect.height/2 && 
         handTransform.position.y <= buttonTransform.position.y + buttonTransform.rect.height/2){
            return true;
        }
        return false;
    }
}
