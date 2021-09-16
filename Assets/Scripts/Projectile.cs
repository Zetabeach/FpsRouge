using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 420.0f;
    public int predictionStepsPerFrame = 12;
    public Vector3 bulletVelocity;

    private int isBounce = 0;


    //Penetration
    private Vector3 endPoint;
    private Vector3? penetrationPoint;
    private Vector3? impactPoint;
    public float penetrationAmount = 1;

    public float lifeTime;
    private float currentLife;


    // Start is called before the first frame update
    void Start()
    {
        bulletVelocity = this.transform.forward * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (lifeTime < currentLife)
        {
            Destroy(gameObject);
        }
        else
        {
            currentLife++;
        }
        Vector3 point1 = this.transform.position;
        float stepSize = 1.0f / predictionStepsPerFrame;
        for (float step = 0; step < 1; step += stepSize)
        {

            bulletVelocity += Physics.gravity * stepSize * Time.deltaTime;
            Vector3 point2 = point1 + bulletVelocity * stepSize * Time.deltaTime;

            Ray ray = new Ray(point1, point2 - point1);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude))
            {

                //Ray raypEn = new Ray(this.transform.position, this.transform.forward);
                //RaycastHit hit1;
                //if (Physics.Raycast(raypEn, out hit1))
                //{
                //    impactPoint = hit1.point;
                //    Ray penRay = new Ray(hit1.point + raypEn.direction * penetrationAmount, -raypEn.direction);
                //    RaycastHit penHit;
                //    if (hit1.collider.Raycast(penRay, out penHit, penetrationAmount))
                //    {

                //        //Does exit wall

                //        point2 = bulletVelocity;
                //        Debug.Log("MENEE LÄPI");
                //    }


                //}

               

                    impactPoint = hit.point;
                    Ray penRay = new Ray(hit.point + ray.direction * penetrationAmount, -ray.direction);
                    RaycastHit penHit;
                    if (hit.collider.Raycast(penRay, out penHit, penetrationAmount))
                    {
                   //     Debug.Log("MENEE LÄPI");
                    point2 += Physics.gravity * stepSize * Time.deltaTime;

                    }

                

                else { 

                  //  Debug.Log(" EI MEEE MEEE MENEE LÄPI");

                    //check if object has ricoched before
                    if (isBounce == 1)
                {
                    Destroy(gameObject);
                }


                Vector3 oldVelocity =bulletVelocity;

                bulletVelocity = Vector3.Reflect(bulletVelocity, hit.normal);
             //   Debug.Log("hit");
                float angle = Vector3.Angle(oldVelocity, bulletVelocity);
               // Debug.Log(angle);

              

                

                if (angle <= 50)
                {
                    bulletVelocity *= Mathf.Lerp(0.1f, 1, 1 - (angle / 180));
                    isBounce = 1;
                }
                else if (angle >= 50 && angle <= 150 )
                {
                        float angleFactor = angle - 50;
                        // if(Random.Range(0,100) >= Random.Range(0,100))
                        if(Random.Range(0,100) > angleFactor)
                        {
                            bulletVelocity *= Mathf.Lerp(0.1f, 1, 1 - (angle / 180));
                        isBounce = 1;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
                

               
                float rng = Random.Range(0.1f,1);

                bulletVelocity = bulletVelocity * rng;

                point2 = point1 + bulletVelocity * stepSize * Time.deltaTime;

                }
            }

            point1 = point2;
            

        }
        this.transform.position = point1;
    }

    void UpdatePenetration()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            impactPoint = hit.point;
            Ray penRay = new Ray(hit.point + ray.direction * penetrationAmount, -ray.direction);
            RaycastHit penHit;
           if(hit.collider.Raycast(penRay, out penHit, penetrationAmount))
            {

                //Does exit wall
                penetrationPoint = penHit.point;
                bulletVelocity = penHit.point;
                
            }
            else
            {
                //does not exit wall
                endPoint = impactPoint.Value;
                penetrationPoint = impactPoint.Value;
                return;
            }

        }
        else
        {
            endPoint = this.transform.position + this.transform.forward * 100;
            penetrationPoint = null;
            impactPoint = null;
            return;

        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 point1 = this.transform.position;
        Vector3 predictedBulletVelocity = bulletVelocity;
        float stepSize = 0.01f;
        for(float step = 0; step < 1; step += 0.01f)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
            Gizmos.DrawLine(point1, point2);
            point1 = point2;

        }
    }



 
}
