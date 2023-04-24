using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
    public int collided = 0; //(0 = bağlı lastik yok, 1 = yeni bağlandı, 2 = uzun süredir bağlı lastik var)


    //Gerek kalmadı ama lastiğin bağlı kalma süresi gerekirse ilerde diye comment halde bıraktım
    /*
    private bool collidedCheck = false;
    private float collisionStartTime = 0.0f;
    private float collisionDuration = 0.0f;
    // lastik yeni bağlandı ve uzun süredir bağlı diye ayırma sebebim lastiği yakın bırakınca takılmasını sağlayan bi bugı çözmek. Snapteki waitlerle ilgili 
    void FixedUpdate()
    {
        if (collided > 0)
        {
            collidedCheck = true;
        }
        else
        {
            collidedCheck = false;
        }

        if (GetComponent<CollisionDetector>().collidedCheck)
        {
            if (!collidedCheck)
            {
                collidedCheck = true;
                collisionStartTime = Time.time;
            }
            else
            {
                collisionDuration = Time.time - collisionStartTime;
                if (collisionDuration >= 1.0f) // Lastiğin ne kadar uzun süredir bağlı olduğunu burada belirle !!!
                {
                    collided = 2;
                }
            }
        }
        else
        {
            collidedCheck = false;
            collisionStartTime = 0.0f;
            collisionDuration = 0.0f;
        }
    }
    */

}
