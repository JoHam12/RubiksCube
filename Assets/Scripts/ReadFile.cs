using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class ReadFile : MonoBehaviour
{
    [System.Serializable]

    //Sensitivity values 
    public class Sensitivity{
        [SerializeField] public float sensitivityNormalizedMin = .2f;
        [SerializeField] public float sensitivityNormalizedMax = 3f;
        [SerializeField] public float sensitivityNonNormalizedMin = .0001f;
        [SerializeField] public float sensitivityNonNormalizedMax = .2f;
    }
    [SerializeField] private Constants constants;

    int handTrackerId;
    // Values list
    public List<float> values;

    //Function to extract x, y, close, fullX, fullY, select values from string 
    static List<float> ExtractData(string text){
        
        int j = 0;
        List<float> res = new List<float>();
        int x, y, button, select;
        string xStr = "", yStr = "";
        float xp, yp;
        int k = 0;
        while(k < text.Length){
            if(text[k] == ':'){
                if(j == 0){ 
                    if(text[k+1] == '-'){ x = -1;}
                    else if (text[k+1] == '0'){ x = 0; }
                    else{ x = 1; }
                    res.Add(x);
                }
                else if(j == 1){
                    if(text[k+1] == '-'){ y = -1;}
                    else if (text[k+1] == '0'){ y = 0; }
                    else{ y = 1; }
                    res.Add(y);
                }
                else if (j == 2){
                    if(text[k+1] == '0'){ button = 0; }
                    else{ button = 1; }
                    res.Add(button);
                }
                else if(j == 3){
                    k += 1;
                    while(text[k] != ';'){
                        xStr += text[k];
                        k+=1;
                    }
                    xp = float.Parse(xStr);
                    res.Add(xp);
                }
                else if(j == 4){
                    k += 1;
                    while(text[k] != ';'){
                        yStr += text[k];
                        k+=1;
                    }
                    yp = float.Parse(yStr);
                    res.Add(yp);
                }
                else if (j == 5){
                    if(text[k+1] == '0'){ select = 0; }
                    else{ select = 1; }
                    res.Add(select);
                }
                j += 1;
            }
            k += 1;
        }
        return res;
    }
    private int listLenghtLimit = 1000;
    void Awake(){
        File.WriteAllText("python_scripts\\breakFile.txt", string.Empty);
    }
    // Update is called once per frame
    void Update()
    {
        // Open text file
        using (var stream = File.Open("python_scripts\\test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        
        // Read text file
        using (var reader = new StreamReader(stream))
        {
            // Get list containing every line of text file
            string[] list = reader.ReadToEnd().Split('\n');
            if(list.Length > 2){ 
                // Get second last line of text file
                string text = list[list.Length - 2]; 
                values = ExtractData(text);
            }
            if(list.Length >= listLenghtLimit){
                File.WriteAllText("python_scripts\\test.txt", string.Empty);
            }
        }
    }
    void OnApplicationQuit()
    {
        QuitHandTracker();
    }
    public void StartHandTracker(){
        using (Process myProcess = new Process())
        {
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "python_scripts\\pyHandTrackerLauncher.bat";
            myProcess.StartInfo.CreateNoWindow = true;
                
            myProcess.Start();

        }
        File.WriteAllText("python_scripts\\breakFile.txt", string.Empty);
        
        
    }

    public void QuitHandTracker(){
        File.WriteAllText("python_scripts\\breakFile.txt", "break");
    }
    

}
