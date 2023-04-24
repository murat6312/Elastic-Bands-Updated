using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RopeRemaining : MonoBehaviour
{
    public int ropeRemained;
    public List<GameObject> baskets = new List<GameObject>();
    public GameObject pinHolder;
    public TMP_Text resultText;
    public TMP_Text pinsLeft;
    public GameObject leftCanvas;

    [HideInInspector] public Stack<GameObject> snappedRopes = new Stack<GameObject>();
    private List<GameObject> pins = new List<GameObject>();
    private int startingPinCount;
    private int connectedPinCount = 0;

    void Awake(){

        //pins = pinHolder.GetComponent<PinHolder>.pins; // pinleri oto almak için
        for(int i = 0 ; i < pinHolder.transform.childCount ; i++){
            if(pinHolder.transform.GetChild(i).GetChild(0).gameObject.activeInHierarchy){
                pins.Add(pinHolder.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        startingPinCount = pins.Count;
        leftCanvas.SetActive(false);
        
    }
    void FixedUpdate()
    {
        endingScreen();
        undoLastMove();
    }
    private void endingScreen(){

        foreach (GameObject pin in pins){

            if(pin.GetComponent<CollisionDetector>().collided != 0)
            {
                connectedPinCount++;
            }
    
        }
        int x = startingPinCount - connectedPinCount;
        string y = "Pins Left: " + x;
        pinsLeft.text = y;

        if(connectedPinCount == startingPinCount){
            resultText.text = "YOU WIN!";
            StartCoroutine(WaitBeforeEndingScreenCoroutine(1f));
        }
        else if(this.transform.GetChild(0).GetComponent<RopeGenerator>().remainedRopeCount.text == "0" &&
            this.transform.GetChild(1).GetComponent<RopeGenerator>().remainedRopeCount.text == "0" &&
            this.transform.GetChild(2).GetComponent<RopeGenerator>().remainedRopeCount.text == "0" &&
            this.transform.GetChild(3).GetComponent<RopeGenerator>().remainedRopeCount.text == "0")
        {
            resultText.text = "GAME OVER!!";
            StartCoroutine(WaitBeforeEndingScreenCoroutine(1f));
        }
        
        connectedPinCount = 0;
        
    }

    private IEnumerator WaitBeforeEndingScreenCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        leftCanvas.SetActive(true);

    }
    private void undoLastMove()
    {
        UndoButtonScript undoButtonScript = FindObjectOfType<UndoButtonScript>();
        
        if (undoButtonScript.undo == true && snappedRopes.Count > 0)
        {

            GameObject lastRope = snappedRopes.Peek();

            if(lastRope != null)
            {
                if (lastRope.name == "Yellow Rope(Clone)")
                { 
                    transform.GetChild(0).GetComponent<RopeGenerator>().remainedCount += 1;
                }
                else if (lastRope.name == "Green Rope(Clone)")
                {
                    transform.GetChild(1).GetComponent<RopeGenerator>().remainedCount += 1;
                }
                else if (lastRope.name == "Red Rope(Clone)")
                {
                    transform.GetChild(2).GetComponent<RopeGenerator>().remainedCount += 1;
                }
                else if (lastRope.name == "White Rope(Clone)")
                {
                    transform.GetChild(3).GetComponent<RopeGenerator>().remainedCount += 1;
                }
            }

            GameObject poppedRope = snappedRopes.Pop();
            if (poppedRope != null)
            {
                poppedRope.GetComponent<UndoScore>().ropeIsCut = true;
                Destroy(poppedRope, 0.2f);
            }
            undoButtonScript.undo = false;
        }
    }
}
