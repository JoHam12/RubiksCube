using UnityEngine;
using UnityEngine.UI;

public class HandPictureManager : MonoBehaviour
{
    [SerializeField] private ReadFile fileReader;
    [SerializeField] private Constants constants;
    [SerializeField] private Texture hand, closedHand, clickHand;
    [SerializeField] private RawImage handImage;
    void Update(){
        if(fileReader.values.Count < constants.NUMINDICES){
            return ;
        }
        if(fileReader.values[constants.SELECT] == 1){
            handImage.texture = clickHand;
            return ;
        }
        if(fileReader.values[constants.OPENHANDINDEX] == 0){
            handImage.texture = closedHand;
            return ;
        }

        handImage.texture = hand;

    }
}
