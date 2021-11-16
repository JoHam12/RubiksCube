using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeController : MonoBehaviour
{
    public class Algorithm{
        public List<string> movesList;
        public string name;
    } 
    [SerializeField] private GameObject Rface, Oface, Yface, Wface, Bface, Gface;
    [SerializeField] private KeyCode Ractivate, Oactivate, Yactivate, Wactivate, Bactivate, Gactivate;
    [SerializeField] private CenterManager Rcenter, Ocenter, Ycenter, Wcenter, Bcenter, Gcenter;
    private bool isR, isO, isY, isW, isB, isG;
    private float speed = 2f;
    float angleR, angleL;
    private bool isReverse;
    private List<string> movesList;
    float timer;
    bool timerReached;
    float delayTime = .5f;
    bool activateScramble = false, startAlg = false;
    int numOfMoves, loopVar;
    bool goToNext = false;
    [SerializeField] private Elements elements;
    [SerializeField] private Button scrambleButton, algButton;
    [SerializeField] private Button RButton, OButton, GButton, BButton, WButton, YButton;
    [SerializeField] private List<string> algoMoves;
    private bool Rbool, Obool, Gbool, Bbool, Wbool, Ybool;
    void Awake(){
        
        elements = GameObject.Find("/Elements").GetComponent<Elements>();
        // Get UI elements from elements gameObject
        if(elements != null){
            scrambleButton = elements.scrambleButton;
            algButton = elements.algButton;
            RButton = elements.RButton;
            BButton = elements.BButton;
            OButton = elements.OButton;
            GButton = elements.GButton;
            BButton = elements.BButton;
            WButton = elements.WButton;
            YButton = elements.YButton;
        }
    }
    void Start(){
        
        isR = isO = isY = isW = isB = isG = false;
        Rbool = Obool = Gbool = Bbool = Wbool = Ybool = false;
        // Set rotation angles to 0
        angleL = angleR = 0f;

        // Add all possible moves to moves list
        movesList = new List<string>()
        {
            "R", "O", "B", "G", "Y", "W", 
            "R'", "O'", "B'", "G'", "Y'", "W'"
        };
        isReverse = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(elements == null){
            return ;
        }


        algButton.onClick.AddListener(TaskOnClickAlgoButton);
        PerformAlgorithm(algoMoves);
    
        scrambleButton.onClick.AddListener(TaskOnClickScrambleButton); 
        Scramble();
        

        isReverse = false;
        if(Input.GetKeyDown(KeyCode.LeftShift)){ isReverse = true; }


        if (isR) { R(isReverse, true); return; }
        if (isY) { Y(isReverse, true); return; }
        if (isB) { B(isReverse, true); return; }
        if (isG) { G(isReverse, true); return; }
        if (isW) { W(isReverse, true); return; }
        if (isO) { O(isReverse, true); return; }

        // Keyboard Input
        if (Input.GetKeyDown(Oactivate)) { TaskOnClickO(); }
        if (Input.GetKeyDown(Wactivate)) { TaskOnClickW(); }
        if (Input.GetKeyDown(Gactivate)) { TaskOnClickG(); }
        if (Input.GetKeyDown(Bactivate)) { TaskOnClickB(); }
        if (Input.GetKeyDown(Yactivate)) { TaskOnClickY(); }
        if (Input.GetKeyDown(Ractivate)) { TaskOnClickR(); }

        // Button Input
        OButton.onClick.AddListener(TaskOnClickO);
        WButton.onClick.AddListener(TaskOnClickW);
        GButton.onClick.AddListener(TaskOnClickG);
        BButton.onClick.AddListener(TaskOnClickB);
        YButton.onClick.AddListener(TaskOnClickY);
        RButton.onClick.AddListener(TaskOnClickR);

        if(Obool){ isO = true; isR = isY = isW = isB = isG = false; return ;}
        if(Wbool){ isW = true; isO = isY = isR = isB = isG = false; return ;}
        if(Gbool){ isG = true; isO = isY = isW = isB = isR = false; return ;}
        if(Bbool){ isB = true; isO = isY = isW = isR = isG = false; return ;}
        if(Ybool){ isY = true; isO = isR = isW = isB = isG = false; return ;}
        if(Rbool){ isR = true; isO = isY = isW = isB = isG = false; return ;}

        
    }
    void Scramble()
    {

        if (activateScramble)
        {
            if (loopVar < numOfMoves && goToNext)
            {
                int moveIndex = Random.Range(minInclusive: 0, maxExclusive: movesList.Count);
                PerformMove(movesList[moveIndex]);
                ResetTimer();

                loopVar += 1;
            }
        }
        WaitForTime(delayTime);
        if (loopVar >= numOfMoves)
        {
            activateScramble = false;
        }
    }
    //Change parent of gameObject
    static private void ChangeParent(GameObject _object, GameObject newParent){
        _object.transform.SetParent(newParent.transform);
    }
    private void RevertParent(GameObject _object){
        _object.transform.SetParent(this.transform);
    }
    private void SelectWholeSide(CenterManager _center, GameObject _face){
        for(int i = 0; i < _center.objectsBeside.Count; i++){
            ChangeParent(_center.objectsBeside[i], _face);
        }
    }
    private void DeSelectSide(CenterManager _center){
        for(int i = 0; i < _center.objectsBeside.Count; i++){
            RevertParent(_center.objectsBeside[i]);
        }
    }

    // All Moves Functions
    private void R(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Rcenter, Rface);
        if(_normalSpeed){
            Rface.transform.Rotate(Rcenter.rotationAxis, coeff * speed, Space.World);
            angleR += coeff * speed;
            isO = isY = isW = isB = isG = false;
            if((angleR >= 90f && coeff == 1) || (angleR <= -90f && coeff == -1)){ DeSelectSide(Rcenter);isR = false; Rbool = false; angleR = 0f; }
            return ;
        }
        Rface.transform.Rotate(Rcenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Rcenter);isR = false; Rbool = false;
    }
    private void Y(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Ycenter, Yface);
        if(_normalSpeed){
            Yface.transform.Rotate(Ycenter.rotationAxis, coeff * speed, Space.World);
            angleR += coeff * speed;
            isR = isO = isW = isB = isG = false;
            if((angleR >= 90f && coeff == 1) || (angleR <= -90f && coeff == -1)){ DeSelectSide(Ycenter);isY = false; Ybool = false; angleR = 0f; }
            return ;
        }
        Yface.transform.Rotate(Ycenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Ycenter); isY = false; Ybool = false;
    }
    private void B(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Bcenter, Bface);
        if(_normalSpeed){
            Bface.transform.Rotate(Bcenter.rotationAxis, coeff * speed, Space.World);
            angleR += coeff * speed;
            isR = isO = isY = isW = isG = false;
            if((angleR >= 90f && coeff == 1) || (angleR <= -90f && coeff == -1)){ DeSelectSide(Bcenter);isB = false; Bbool = false; angleR = 0f; }
            return ;
        }
        Bface.transform.Rotate(Bcenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Bcenter);isB = false; Bbool = false;
    }
    private void G(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Gcenter, Gface);
        if(_normalSpeed){
            Gface.transform.Rotate(Gcenter.rotationAxis, coeff * speed, Space.World);
            angleL += coeff * speed;
            isR = isO = isY = isW = isB = false;
            if((angleL >= 90f && coeff == 1) || (angleL <= -90f && coeff == -1)){ DeSelectSide(Gcenter);isG = false; Gbool = false; angleL = 0f; }
            return ;
        }
        Gface.transform.Rotate(Gcenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Gcenter);isG = false;Gbool = false;
    }
    private void W(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Wcenter, Wface);
        if(_normalSpeed){
            Wface.transform.Rotate(Wcenter.rotationAxis, coeff * speed, Space.World);
            angleL += coeff * speed;
            isR = isO = isY = isB = isG = false;
            if((angleL >= 90f && coeff == 1) || (angleL <= -90f && coeff == -1)){ DeSelectSide(Wcenter);isW = false; Wbool = false;angleL = 0f; }
            return ;
        }
        Wface.transform.Rotate(Wcenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Wcenter);isW = false;Wbool = false;
    }
    private void O(bool _isReverse, bool _normalSpeed){
        int coeff = 1;
        if(_isReverse){ coeff = -1; }
        SelectWholeSide(Ocenter, Oface);
        if(_normalSpeed){
            Oface.transform.Rotate(Ocenter.rotationAxis, coeff * speed, Space.World);
            angleL += coeff * speed;
            isR = isY = isW = isB = isG = false;
            if((angleL >= 90f && coeff == 1) || (angleL <= -90f && coeff == -1)){ DeSelectSide(Ocenter);isO = false; Obool = false; angleL = 0f; }
            return ;
        }
        Oface.transform.Rotate(Ocenter.rotationAxis, coeff * 90, Space.World);
        DeSelectSide(Ocenter); isO = false;Obool = false;
    }
    private void ResetTimer(){
        timer = 0;
        goToNext = false;
        timerReached = false;
    }
    private void WaitForTime(float t){
        if(!timerReached){ timer += Time.deltaTime;}

        if (!timerReached && timer > t)
        {
            goToNext = true;
            timerReached = true;
        }
    }
    void PerformMove(string _move){
        switch (movesList.IndexOf(_move))
        {
            case 0:
                R(false, false);
                break;
            case 1:
                O(false, false);
                break;
            case 2:
                B(false, false);
                break;
            case 3:
                G(false, false);
                break;
            case 4:
                Y(false, false);
                break;
            case 5:
                W(false, false);
                break;
            case 6:
                R(true, false);
                break;
            case 7:
                O(true, false);
                break;
            case 8:
                B(true, false);
                break;
            case 9:
                G(true, false);
                break;
            case 10:
                Y(true, false);
                break;
            case 11:
                W(true, false);
                break;
            default:
                break;
        }
    }

    void TaskOnClickR(){Rbool = true; Obool = Ybool = Wbool = Bbool = Gbool = false; }
    void TaskOnClickO(){Obool = true; Rbool = Ybool = Wbool = Bbool = Gbool = false; }
    void TaskOnClickG(){Gbool = true; Rbool = Obool = Ybool = Wbool = Bbool = false; }
    void TaskOnClickB(){Bbool = true; Rbool = Obool = Ybool = Wbool = Gbool = false; }
    void TaskOnClickY(){Ybool = true; Rbool = Obool = Wbool = Bbool = Gbool = false; }
    void TaskOnClickW(){Wbool = true; Rbool = Obool = Ybool = Bbool = Gbool = false; }
    void TaskOnClickScrambleButton(){
        if(!activateScramble){ 
            activateScramble = true;
            numOfMoves = Random.Range(minInclusive: 10, maxExclusive: 20); 
            loopVar = 0; goToNext = true;
        } 
    }
    void TaskOnClickAlgoButton(){
        if(!startAlg){
            startAlg = true;
            loopVar = 0; goToNext = true;
        }
        
    }
    void PerformAlgorithm(List<string> moves){
        if (startAlg)
        {
            if (loopVar < moves.Count && goToNext)
            {
                PerformMove(moves[loopVar]);
                ResetTimer();

                loopVar += 1;
            }
        }
        WaitForTime(delayTime);
        if (loopVar >= moves.Count)
        {
            startAlg = false;
        }
    }
}
