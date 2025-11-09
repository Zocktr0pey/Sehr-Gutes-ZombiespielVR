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

    public GameObject BloodHit;

    // Nur f�r 3D
    //[SerializeField] private Camera cam;

    AudioManager audioManager;
    LineRenderer laserPointer;
    private Animator animator;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        laserPointer = GetComponent<LineRenderer>();

        currentAmmo = maxAmmo;
        laserPointer.enabled = true;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = muzzleFlash.transform.position;
        laserPointer.SetPosition(0, origin);
        laserPointer.SetPosition(1, origin + muzzleFlash.transform.forward * range);
    }

    public void Shoot()
    {
        if (currentAmmo == 0)
        {
            //audioManager.EmptyPistol()
            return;
        }

        animator.SetTrigger("ShootTrigger");
        audioManager.ShootPistol();
        muzzleFlash.Emit(10);
        RaycastHit hit;

        // Nur f�r 3D, Bei VR muss origin und direction von der Waffe statt Kamera kommen
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

        // F�r VR
        if (Physics.Raycast(muzzleFlash.transform.position, muzzleFlash.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                Instantiate(BloodHit, hit.point, Quaternion.LookRotation(hit.normal));
                target.Hit(damage, muzzleFlash.transform.forward, hit.point);
                // Destroy(effectInstance, 1f);
            }
        }

        currentAmmo--;
    }

    public bool AmmoIsFull()
    {
        return currentAmmo == maxAmmo;
    }
    
    public void Reload()
    {
        // provisorisch
        currentAmmo = maxAmmo;
        audioManager.ReloadPistol();
        Debug.Log("Waffe nachgeladen!");
    }
}

