using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutManager : MonoBehaviour
{

    bool mouseDragging = false;
    Vector3 screenSpace;
    Vector3 offset;
    bool CutPiece = false;
    [SerializeField]
    public List<Log> logs;
    [SerializeField]
    int Goal = 0;
    
    List<Log> newLogs = new List<Log>();

    private float timeStamp = Mathf.Infinity;


    [SerializeField]
    int waitTime = 10;

    [SerializeField]
    int Gas = 100;
    [SerializeField]
    int gasDrain = 1;
    [SerializeField]
    TextMeshProUGUI GasLeft;
    [SerializeField]
    TextMeshProUGUI LogGoal;
    [SerializeField]
    TextMeshProUGUI CurrentLogs;
    [SerializeField]
    TextMeshProUGUI Victory;
    [SerializeField]
    TextMeshProUGUI Defeat;

    bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        LogGoal.text = Goal.ToString(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {

            print("r was pressed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //float depth = 10.0f;
        //float speed = 1.5f;
        //if (mouseDragging)
        //{
        //    var mousePos = Input.mousePosition;
        //    var wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        //    transform.position = Vector3.MoveTowards(transform.position, wantedPos, speed * Time.deltaTime);
        //}
        CurrentLogs.text = logs.Count.ToString();
        GasLeft.text = Gas.ToString();
        if (logs.Count == Goal)
        {
            if (Input.GetKeyDown("c"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (gamePaused)
            {
                return;
            }
            mouseDragging = false;
            Debug.Log("Victory!");
            Victory.gameObject.SetActive(true);
            gamePaused = true;

            return;
        }
        else if (Gas <= 0 || logs.Count > Goal)
        {
            gamePaused = true;
            Time.timeScale = 0;
            Defeat.gameObject.SetActive(true);
        }
        if (mouseDragging)
        {
            if (gamePaused)
            {
                return;
            }
            timeStamp += Time.time;
        }




    }

    void OnMouseDown()
    {
        newLogs = new List<Log>();
        mouseDragging = true;
        timeStamp = Time.time;
        //translate the cubes position from the world to Screen Point
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        foreach (Log log in logs)
        {
            log.pieceCutRecently = false;
        }
        //calculate any difference between the cubes world position and the mouses Screen position converted to a world point  
        //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

    }
    void OnMouseUp()
    {
        mouseDragging = false;
        logs.AddRange(newLogs);
        newLogs = new List<Log>();
        timeStamp = Mathf.Infinity;
}




    void OnMouseDrag()
    {
        if (gamePaused)
        {
            return;
        }
        //keep track of the mouse position
        var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        if (Gas <= 0)
        {
            return;
        }
        if (timeStamp > waitTime)
        {
            timeStamp = Time.time;
            Gas -= gasDrain;

        }
        //convert the screen mouse position to world point and adjust with offset
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);

        //check if mouse cuts any logs
        foreach(Log log in logs)
        {
            if (log.pieceCutRecently == true) { continue; }
            foreach (Box box in log.Parts)
            {
                Vector3 calcPos = new Vector3(curPosition.x, curPosition.y, box.transform.position.z);

                if (Vector3.Distance(calcPos, box.transform.position) < .5f)
                {

                    if (!log.pieceCutRecently)
                    {
                        Log newLog = log.CutPiece(box);
                        if(newLog != null)
                        {
                            newLogs.Add(newLog);
                        }
                        log.pieceCutRecently = true;
                        break;
                    }
                }
            }
        }
        
        ////update the position of the object in the world
        //transform.position = curPosition;
    }
}
