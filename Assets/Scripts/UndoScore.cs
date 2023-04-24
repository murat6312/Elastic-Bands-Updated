using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoScore : MonoBehaviour
{

    [HideInInspector] public bool ropeIsCut = false;
    [HideInInspector] public List<RaycastHit> hitss1;
    [HideInInspector] public List<RaycastHit> hitss2;

    void FixedUpdate()
    {

        if (ropeIsCut && hitss1 != null && hitss2 !=null) // ropeIsCut roperemaining scriptinin içinde ayarlanýyo
        {
            hitss1.AddRange(hitss2);
            Debug.Log(hitss1.Count);
            foreach (RaycastHit hit in hitss1)
            {
                
                GameObject obj = hit.collider.gameObject;
                //Debug.Log(obj.name);
                obj.GetComponent<CollisionDetector>().collided = 0;
            }

            ropeIsCut = false;
        }
        if (ropeIsCut && hitss1 == null && hitss2 == null)
        {
            Debug.Log("NULLLLLLLL");
        }
        if (ropeIsCut && hitss1 != null && hitss2 == null)
        {
            Debug.Log("hitss2 NULLLLLLLL");
        }
        if (ropeIsCut && hitss1 == null && hitss2 != null)
        {
            Debug.Log("hitss1 NULLLLLLLL");
        }
    }
}
