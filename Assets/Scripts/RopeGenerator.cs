using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RopeGenerator : MonoBehaviour
{
    public GameObject rope; 
    public int startingRopeCount = 0;
    public int remainedCount;
    public TMP_Text remainedRopeCount;
    public Rect touchArea; //Gereksiz

    private GameObject instantiatedRope;
    private GameObject sphere;

    void Start() 
    { 
        remainedCount = startingRopeCount;
        instantiateFunction();
    }
    void FixedUpdate()
    {
        string z = "" + (remainedCount);
        remainedRopeCount.text = z;
        if (sphere != null )
        {
            if(sphere.GetComponent<Snap>().snapped == true && remainedCount != 0)
            {
                GetComponentInParent<RopeRemaining>().snappedRopes.Push(sphere.GetComponent<Snap>().rope);
                instantiateFunction();
                remainedCount--;
                // sonrada snape yeni eklediðim ropeIsCut ile uðraþ
            }

        }

    }

    public void instantiateFunction()
    {
        if (remainedCount > 0 ) 
        {

            instantiatedRope = Instantiate(rope, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                    Quaternion.identity);

            sphere = instantiatedRope.transform.GetChild(1).gameObject;
            
        }
    }

}
