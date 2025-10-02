using NUnit;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private int currentAmmo;
    [SerializeField] private float reloadtime = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 150f;
    [SerializeField] private ParticleSystem muzzleFlash;
    public bool isFullAuto = false;

    // Nur für 3D
    //[SerializeField] private Camera cam;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        if (currentAmmo == 0)
        {
            //audioManager.EmptyPistol()
            return;
        }

        audioManager.ShootPistol();
        muzzleFlash.Emit(10);
        RaycastHit hit;

        // Nur für 3D, Bei VR muss origin und direction von der Waffe statt Kamera kommen
        /*
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.Hit(damage, cam.transform.forward, hit.point);
            }
        }
        */

        // Für VR
        if (Physics.Raycast(muzzleFlash.transform.position, muzzleFlash.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.Hit(damage, muzzleFlash.transform.forward, hit.point);
            }
        }

        currentAmmo--;
    }
    
    public void Reload()
    {
        // provisorisch
        currentAmmo = maxAmmo;
        //audioManager.ReloadPistol
    }
}

