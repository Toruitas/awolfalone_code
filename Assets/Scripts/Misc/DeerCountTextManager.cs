using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeerCountTextManager : MonoBehaviour
{

    public TextMeshProUGUI deerCountTM;
    public int deerCount;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateDeerCount", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDeerCount(){
        deerCount = GameObject.FindGameObjectsWithTag("Deer").Length;

        deerCountTM.text = "X " + deerCount;
    }
}
