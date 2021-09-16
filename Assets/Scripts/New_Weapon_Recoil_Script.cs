using UnityEngine;

public class New_Weapon_Recoil_Script : MonoBehaviour
{
    [Header("Recoil_Transform")]
    public Transform RecoilPositionTranform;
    public Transform RecoilRotationTranform;
    [Space(10)]
    [Header("Recoil_Settings")]
    public float PositionDampTime;
    public float RotationDampTime;
    [Space(10)]
    public float Recoil1;
    public float Recoil2;
    public float Recoil3;
    public float Recoil4;
    [Space(10)]
    public Vector3 RecoilRotation;
    public Vector3 RecoilKickBack;

    public Vector3 RecoilRotation_Aim;
    public Vector3 RecoilKickBack_Aim;
    [Space(10)]
    [Header("ADS Recoil")]
    public Gun gun;
    public float adsPositionDampTime;
    public float adsRotationDampTime;
    [Space(10)]
    public float adsRecoil1;
    public float adsRecoil2;
    public float adsRecoil3;
    public float adsRecoil4;
    [Space(10)]
    public Vector3 adsRecoilRotation;
    public Vector3 adsRecoilKickBack;

    public Vector3 adsRecoilRotation_Aim;
    public Vector3 adsRecoilKickBack_Aim;

    [Space(10)]
    public Vector3 CurrentRecoil1;
    public Vector3 CurrentRecoil2;
    public Vector3 CurrentRecoil3;
    public Vector3 CurrentRecoil4;
    [Space(10)]
    public Vector3 RotationOutput;

    public bool aim;
    private bool ads;

  private  float oldPositionDampTime;
    private float oldRotationDampTime;


    private float oldRecoil1;
    private float oldRecoil2;
    private float oldRecoil3;
    private float oldRecoil4;


    private Vector3 oldRecoilRotation;
    private Vector3 oldRecoilKickBack;

    private Vector3 oldRecoilRotation_Aim;
    private Vector3 oldRecoilKickBack_Aim;


    public void Start()
    {
        oldPositionDampTime = PositionDampTime;
         oldRotationDampTime = RotationDampTime;


         oldRecoil1 = Recoil1;
      oldRecoil2 = Recoil2;
       oldRecoil3 = Recoil3;
        oldRecoil4 = Recoil4;


      oldRecoilRotation = RecoilRotation;
        oldRecoilKickBack = RecoilKickBack;

   oldRecoilRotation_Aim = RecoilRotation_Aim;
       oldRecoilKickBack_Aim = RecoilKickBack_Aim;

    }

    void FixedUpdate()
    {
        AimDownSight();
        CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, Recoil1 * Time.deltaTime);
        CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, Recoil2 * Time.deltaTime);
        CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, Recoil3 * Time.deltaTime);
        CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, Recoil4 * Time.deltaTime);

        RecoilPositionTranform.localPosition = Vector3.Slerp(RecoilPositionTranform.localPosition, CurrentRecoil3, PositionDampTime * Time.fixedDeltaTime);
        RotationOutput = Vector3.Slerp(RotationOutput, CurrentRecoil1, RotationDampTime * Time.fixedDeltaTime);
        RecoilRotationTranform.localRotation = Quaternion.Euler(RotationOutput);
    }
    public void Fire()
    {
        if (aim == true)
        {
            
            CurrentRecoil1 += new Vector3(RecoilRotation_Aim.x, Random.Range(-RecoilRotation_Aim.y, RecoilRotation_Aim.y), Random.Range(-RecoilRotation_Aim.z, RecoilRotation_Aim.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack_Aim.x, RecoilKickBack_Aim.x), Random.Range(-RecoilKickBack_Aim.y, RecoilKickBack_Aim.y), RecoilKickBack_Aim.z);
        }
        if (aim == false)
        {
          
            CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        }
    }
    private void AimDownSight()
    {
     


        if (Input.GetKey(KeyCode.Mouse1))
        {

            if (!ads)
            {
                ads = true;
            }  
           




          PositionDampTime = adsPositionDampTime;
          RotationDampTime = adsRotationDampTime;


           Recoil1 = adsRecoil1;
           Recoil2 = adsRecoil2;
           Recoil3 = adsRecoil3;
           Recoil4 = adsRecoil4;
   
        RecoilRotation = adsRecoilRotation;
        RecoilKickBack = adsRecoilKickBack;

        RecoilRotation_Aim = adsRecoilRotation_Aim;
        RecoilKickBack_Aim = adsRecoilKickBack_Aim;


        }
        else
        {

            PositionDampTime = oldPositionDampTime;
            RotationDampTime = oldRotationDampTime;


            Recoil1 = oldRecoil1;
            Recoil2 = oldRecoil2;
            Recoil3 = oldRecoil3;
            Recoil4 = oldRecoil4;

            RecoilRotation = oldRecoilRotation;
            RecoilKickBack = oldRecoilKickBack;

            RecoilRotation_Aim = oldRecoilRotation_Aim;
            RecoilKickBack_Aim = oldRecoilKickBack_Aim;

        }

        
    }
}