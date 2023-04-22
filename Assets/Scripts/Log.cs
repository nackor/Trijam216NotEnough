using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{

    [SerializeField]
    CutManager cutManager;

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
                if (Parts.Contains(child.GetComponent<Box>())) { continue; }
                Parts.Add(child.GetComponent<Box>());
            }
        }
     cutManager = FindAnyObjectByType<CutManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Log CutPiece(Box partToCut)
    {
        if(pieceCutRecently == true)
        {
            return null;
        }
        if(Parts.Count <= baseNumber)
        {
            return null;
        }
        pieceCutRecently = true;
        //delete cut piece from parts
        //parts needs to be sorted
        Debug.Log(partToCut.name);
        List<Box> oldLog = new List<Box>();
        List<Box> newLog = new List<Box>();
        bool found = false;
        foreach(Box part in Parts)
        {
            if(part == partToCut)
            {

                found = true;
                continue;
                //Parts.Remove(partToCut);
                //partToCut.gameObject.SetActive(false);
            }
            if (!found)
            {

                oldLog.Add(part);
                continue;
            }
            else
            {
                if (newLog.Contains(part)) { continue; }
                newLog.Add(part);
            }

        }
        Parts = oldLog;
        partToCut.gameObject.SetActive(false);
        //spawn new log that has pieces not in my part
        GameObject newObj = new GameObject();
        newObj.AddComponent<Log>();
        newObj.GetComponent<Log>().Parts = newLog;
        foreach(Box part in newLog)
        {
            part.transform.parent = newObj.transform;
        }
        return newObj.GetComponent<Log>();
        //Instantiate(newObj);
        //handle end edge case
    }
}
