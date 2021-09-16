
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject bullet;

    public GameObject gunLocator;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, spreadFactor,baseSpread,maxSpread,Recovery;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    public int bulletsLeft, bulletsShot;

   public bool shooting, readyToShoot, reloading, aiming;

    public Camera fpsCam;
    public float adsZoom;
    public float FOV;
    public Transform attackPoint;


    public GameObject muzzleFlash;

    public New_Weapon_Recoil_Script recoil;
    public New_Weapon_Recoil_Script CamRecoil;

    public bool allowInvoke = true;


    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public GameObject gunModel;
    public float adsSpeed = 8;
    public float adsSpread;
    private float oldSpead;
    private bool isADS;


    public GameObject adsDot;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        FOV = fpsCam.fieldOfView;
        originalPosition = gunModel.transform.localPosition;
    }

    private void FixedUpdate()
    {
        MyInput();
        AimDownSight();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);





        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();



        if( readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shooting();
        }
        if (readyToShoot && !shooting)
        {
          
           

            if (spread > baseSpread)
            {
                spread -= Recovery * Time.deltaTime * 1.5f;
            }
        
            

        }
       
    }

    private void AimDownSight()
    {
       oldSpead = spread;
        if (Input.GetKey(KeyCode.Mouse1) && !reloading && bulletsLeft > 0)
        {
            gunModel.transform.localPosition = Vector3.Lerp(gunModel.transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);

            spread = adsSpread;
            isADS = true;
            fpsCam.fieldOfView = adsZoom;

        }
        else 
        {
            gunModel.transform.localPosition = Vector3.Lerp(gunModel.transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
            spread = oldSpead;
            isADS = false;
            fpsCam.fieldOfView = FOV;
        }
    }
  

    private void Shooting()
    {
        readyToShoot = false;

        recoil.Fire();
        CamRecoil.Fire();

        
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        

        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        

        // Debug.Log("Without Spread" + directionWithoutSpread);




        //Create 1 meter circle multiply by spread
        Vector3 deviation3D = Random.insideUnitCircle * spread;

        //Looks at a point 100 meter ahead and adds deviation3D to the point
        Quaternion rot = Quaternion.LookRotation(Vector3.forward * 100 + deviation3D);

        //bullets shooting vector
        Vector3 directionWithSpread3 = attackPoint.rotation * rot * Vector3.forward;

        

      //  Debug.Log("With Spread" + directionWithSpread.normalized + "magnitude" + directionWithSpread.sqrMagnitude);
      //  Debug.Log("New Vector3" + new Vector3(x, y, 0));
        



        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithSpread3.normalized;




   

        bulletsLeft--;
        bulletsShot++;

        if (spread < maxSpread)
        {
            spread += spreadFactor * Time.deltaTime * 1.1f;
            
        }
        

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
  

    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        
    }
}
