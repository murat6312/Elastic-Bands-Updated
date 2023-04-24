using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Obi;


public class Snap : MonoBehaviour
{
    public float snapDistance = 0.5f;
    //public float lerpTime = 5; //lerpte kullanılır
    public GameObject otherSphere;
    public GameObject thisSphere;
    public GameObject obiRod;
    public GameObject obiRodStart;
    //public GameObject obiRodEnd; //içinden çıkamadığım için saldım
    public AudioSource audioSource;
 
    [HideInInspector] public string fingerState;
    [HideInInspector] public bool snapped;
    [HideInInspector] public List<RaycastHit> hits; // lastiğin bağladığı çiviler, skor için
    [HideInInspector] public List<RaycastHit> hits2;
    [HideInInspector] public GameObject rope;
    private List<Transform> pinPivots = new List<Transform>();
    private BoxCollider middleFloorCollider;
    private GameObject otherSpherePivot;
    private Vector3 otherSpherePivotPos;
    private GameObject thisSpherePivot;
    private Vector3 thisSpherePivotPos;
    private Snap otherSnap;
    private MeshRenderer rodStartMesh;
    //private MeshRenderer rodEndMesh;
    private MeshRenderer rodMesh;
    private bool isWaiting = false; // gri gözükmesine bakma,kullanmassak coroutine bitince tekrar başlıyo
    private bool isWaiting2 = false;
    private bool rayFlag = false; // buraya yazmakla starta yazmak arasındaki fark ne???
    private bool ropeDropped = false; // sepetteki ip sayısının yere düşünce 1 den fazla artmasını engellemek için
    private bool ropeMoved = false; // nadir bug çözümü

    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        otherSnap = otherSphere.GetComponent<Snap>();
        otherSpherePivot = otherSphere.transform.GetChild(0).gameObject;
        thisSpherePivot = thisSphere.transform.GetChild(0).gameObject;
        rope = this.transform.parent.gameObject;

        middleFloorCollider = PinHolder.instance.middleFloor.GetComponent<BoxCollider>();
        middleFloorCollider.enabled = true;

        rodStartMesh = obiRodStart.GetComponent<MeshRenderer>();
        //rodEndMesh = obiRodEnd.GetComponent<MeshRenderer>();
        rodStartMesh.enabled = true;
        //rodEndMesh.enabled = false;
        rodMesh = obiRod.GetComponent<MeshRenderer>();
        rodMesh.enabled = false;

        pinPivots = PinHolder.instance.pinPivots;
        snapped = false;


    }

    void FixedUpdate() 
    {

        if(fingerState == "down")
        {
            thisSphere.SetActive(true);
            otherSphere.SetActive(true);
            obiRod.SetActive(true);
            obiRodStart.SetActive(true);
            //obiRodEnd.SetActive(true);
        }

        meshToggle();

        float smallestDistance = snapDistance;

        thisSpherePivotPos = thisSpherePivot.transform.position;
        otherSpherePivotPos = otherSpherePivot.transform.position;

        scoreRay();

        if (snapped == false)
        {
            
            this.transform.LookAt(otherSphere.transform, Vector3.up); //Startsphere y = 90 endsphere y = -90 
            bool closeToPin = false;

            foreach (Transform pinPivot in pinPivots)
            {
                
                if (Vector3.Distance(pinPivot.position, thisSpherePivotPos) < smallestDistance)
                {

                    closeToPin = true;
                                                    
                    if( Input.touchCount == 0 && rodMesh.enabled == true ) 
                    {
                        //Debug.Log("SNAPPED!!");

                        snapped = true;

                        otherSpherePivot.transform.LookAt(pinPivot.transform, Vector3.up);

                        thisSpherePivot.transform.position = pinPivot.position;


                        //Lerp//thisSpherePivot.transform.position = Vector3.Lerp(thisSpherePivot.transform.position, pin.position, Time.deltaTime/snapTime);
                        //3.parametre ne kadar gerekli şüpheli
                        //Mathf.SmoothStep() yada direk 0.5f te mümkün
                        smallestDistance = Vector3.Distance(pinPivot.position, thisSpherePivotPos); //neye yarıyo????????

                        /*
                        if (audioSource != null){
                            audioSource.Play();
                        }
                        */

                        rayFlag = true;

                        isWaiting2 = true; //flag bişeye yaramıyo

                        StartCoroutine(DisablePinColliderBeforeSnapCoroutine(0.01f));

                        StartCoroutine(EnablePinColliderAfterSnapCoroutine(0.5f));

                        StartCoroutine(DecreaseSubstepsAfterSnapCoroutine(0.9f));

                        Destroy(thisSphere, 1f); 
                        Destroy(otherSphere, 1f);
                        Destroy(obiRodStart, 1f);
                        //Destroy(obiRodEnd, 1f);


                        //thisSphere.SetActive(false);
                        //otherSphere.SetActive(false);

                    }
                    
                }
                
            }
            
            if (fingerState == "up" && otherSnap.fingerState == "up" && closeToPin == false && ropeDropped == false) // snap olmadan bırakılan ipleri sepete geri yolla
            {//Input.touchCount == 0 && nedense bunu kullanınca olmuyo

                reInstantiate();

            }

            if(thisSphere.name == "StartSphere" && fingerState == "up" && otherSnap.fingerState == "" && ropeDropped == false && ropeMoved == true) // nadir bug
            {//lastik İLK alındığında açılmadan bırakınca yok olmadan duruyodu
                reInstantiate();
            }

        }
        
    }
    
    private IEnumerator DisablePinColliderBeforeSnapCoroutine(float time) // lastiğin hizada olmayan çivilere bağlanmasını engellemek için  
    {
        yield return new WaitForSeconds(time);
        if(thisSphere.name == "StartSphere")
        {
            foreach (RaycastHit hit in hits) // nadiren object bulunamadı hatası veriyo consolda ama herşey düzgün çalışıyo yinede garip!
            {
                GameObject obj = hit.collider.gameObject;
                obj.GetComponent<ObiCollider>().enabled = false;
            }
        }else if(thisSphere.name == "EndSphere")
        {
            foreach (RaycastHit hit in hits2) // same
            {
                GameObject obj = hit.collider.gameObject;
                obj.GetComponent<ObiCollider>().enabled = false;
            }
        }
        
        
        middleFloorCollider.enabled = false;
        isWaiting2 = false;
        isWaiting = true;
    }
    
    
    private IEnumerator EnablePinColliderAfterSnapCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (Transform pinpivot in pinPivots)
        {
            pinpivot.GetComponentInChildren<ObiCollider>().enabled = true;
        }

        middleFloorCollider.enabled = true;
        isWaiting = false;

    }

    private IEnumerator DecreaseSubstepsAfterSnapCoroutine(float time) //Performans
    {
        yield return new WaitForSeconds(time);

        this.transform.parent.GetComponent<ObiFixedUpdater>().substeps = 4;

    }

    private void scoreRay()//raycastle pin collider alırken hep 1 eksik aldığı için böyle dolaylı bi yolla çözdüm.
     // ilerde sorun olmaya devam ederse startsphere daki pin pivotun arkasına child olarak 2.pinpivot ekleyip score tutma rayini oradan başlatmayı dene
    {

        if (rayFlag && thisSphere.name == "StartSphere" )
        {

            hits = new List<RaycastHit>(Physics.RaycastAll(thisSpherePivotPos, otherSpherePivotPos - thisSpherePivotPos, Vector3.Distance(thisSpherePivotPos, otherSpherePivotPos)));
            
            foreach (RaycastHit hit in hits)
            {
                GameObject obj = hit.collider.gameObject;
                obj.GetComponent<CollisionDetector>().collided = 1;        
            }

            GetComponentInParent<UndoScore>().hitss1 = hits;

            rayFlag = false;
        }

        else if(rayFlag && thisSphere.name == "EndSphere") 
        {
            hits2 = new List<RaycastHit>(Physics.RaycastAll(thisSpherePivotPos, otherSpherePivotPos - thisSpherePivotPos, Vector3.Distance(thisSpherePivotPos, otherSpherePivotPos)));

            foreach (RaycastHit hit in hits2)
            {
                GameObject obj = hit.collider.gameObject;
                obj.GetComponent<CollisionDetector>().collided = 1;
            }

            GetComponentInParent<UndoScore>().hitss2 = hits2;
            rayFlag = false;
        }
    }

    private void reInstantiate()
    {
        
        if (rope.name == "Yellow Rope(Clone)")
        {
            reInstantiateExtension(0);
        }
        else if (rope.name == "Green Rope(Clone)")
        {
            reInstantiateExtension(1);
        }
        else if (rope.name == "Red Rope(Clone)")
        {
            reInstantiateExtension(2);
        }
        else if (rope.name == "White Rope(Clone)")
        {
            reInstantiateExtension(3);
        }

        if (ropeDropped)
        {
            Destroy(rope);
        }
    }

    private void reInstantiateExtension(int childNum)
    { 

        RopeGenerator ropegenerator = FindObjectOfType<RopeGenerator>(); //muhtemelen daha direkt bi erişim varda bu çalıştı neyse
        GameObject baskets = ropegenerator.transform.parent.gameObject;
        baskets.transform.GetChild(childNum).GetComponent<RopeGenerator>().instantiateFunction();
        ropeDropped = true;

    }
    
    private void meshToggle(){

        if( fingerState == "down" && otherSnap.fingerState == "down" ){
            rodStartMesh.enabled = false;
            //rodEndMesh.enabled = false;
            rodMesh.enabled = true;
        }
        if( fingerState == "up" && otherSnap.fingerState == "down" ){ //thisSphere.name == "StartSphere" && 
            rodStartMesh.enabled = true;
            //rodEndMesh.enabled = true;
            rodMesh.enabled = false; 
        }
        if( fingerState == "down" && otherSnap.fingerState == "up" ){ //thisSphere.name == "StartSphere" && 
            rodStartMesh.enabled = true;
            //rodEndMesh.enabled = false;
            rodMesh.enabled = false; 
        }
        if( fingerState == "up" && otherSnap.fingerState == "up" ){
            rodStartMesh.enabled = false;
            //rodEndMesh.enabled = false;
            rodMesh.enabled = true; 
        }
    }
    public void fingerDown()
    {
        fingerState = "down";
        if(thisSphere.name == "StartSphere")
        {
            ropeMoved = true;
        }
    }

    public void fingerUp()
    {fingerState = "up";}


}