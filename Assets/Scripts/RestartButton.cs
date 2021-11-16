using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{

    [SerializeField] private GameObject cube;
    private PieceManager[] allPieces;
    private Button button;
    [SerializeField] private ReadFile fileReader;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        button.onClick.AddListener(ResetCube);
    }
    void ResetCube(){
        allPieces = cube.GetComponentsInChildren<PieceManager>();
        foreach (PieceManager piece in allPieces)
        {
            piece.restart = true;
        }
    }
}
