using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    private int choiceCount;
    // Start is called before the first frame update
    void Start()
    {
        choiceCount = 0;
    }

    public void OnChoice()
    {
        choiceCount++;
        if (choiceCount == 3)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
