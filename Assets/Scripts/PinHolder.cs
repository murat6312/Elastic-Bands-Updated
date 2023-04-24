using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHolder : MonoBehaviour
{
    public static PinHolder instance{ get; private set; } //Singleton
    [HideInInspector]public List<GameObject> pins = new List<GameObject>();
    [HideInInspector]public List<Transform> pinPivots = new List<Transform>();
    public GameObject middleFloor;
    private void Awake() 
    { 
    // If there is an instance, and it's not me, delete myself.
    
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
       
        
        for(int i = 0 ; i < this.transform.childCount ; i++){
            if(pinPivots.Contains(this.transform.GetChild(i))){
                continue;
            }
            
            if(this.transform.GetChild(i).gameObject.activeInHierarchy){
                pinPivots.Add(this.transform.GetChild(i));
                pins.Add(this.transform.GetChild(i).GetChild(0).gameObject);
            }

        }

    }

}
