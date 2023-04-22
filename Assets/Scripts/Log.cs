using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    public List<Box> Parts;

    public bool pieceCutRecently = false;

    int baseNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
     foreach(Transform child in this.transform)
        {
            if(child.GetComponent<Box>() != null)
            {
                Parts.Add(child.GetComponent<Box>());
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CutPiece(Box partToCut)
    {
        pieceCutRecently = true;
        //delete cut piece from parts
        //parts needs to be sorted
        Debug.Log(partToCut.name);
        Parts.Remove(partToCut);
        partToCut.gameObject.SetActive(false);
        //spawn new log that has pieces not in my part
        //handle end edge case
    }
}
