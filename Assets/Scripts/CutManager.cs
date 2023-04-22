using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutManager : MonoBehaviour
{

    bool mouseDragging = false;
    Vector3 screenSpace;
    Vector3 offset;
    bool CutPiece = false;
    [SerializeField]
    Log[] logs;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //float depth = 10.0f;
        //float speed = 1.5f;
        //if (mouseDragging)
        //{
        //    var mousePos = Input.mousePosition;
        //    var wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        //    transform.position = Vector3.MoveTowards(transform.position, wantedPos, speed * Time.deltaTime);
        //}
    }

    void OnMouseDown()
    {
        mouseDragging = true;
        Debug.Log("CutManager MouseDown");
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
    }




    void OnMouseDrag()
    {
        
        //keep track of the mouse position
        var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

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

                    if (!CutPiece)
                    {
                        log.CutPiece(box);
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
