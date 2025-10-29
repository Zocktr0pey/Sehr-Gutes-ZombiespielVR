using NUnit;
using UnityEngine;

public class MenuGunScript : MonoBehaviour
{
    [SerializeField] private float range = 150f;
    [SerializeField] private ParticleSystem muzzleFlash;

    AudioManager audioManager;
    LineRenderer laserPointer;
    private Animator animator;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        laserPointer = GetComponent<LineRenderer>();
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
        animator.SetTrigger("ShootTrigger");
        audioManager.ShootPistol();
        muzzleFlash.Emit(10);
        RaycastHit hit;

        // Fur VR
        if (Physics.Raycast(muzzleFlash.transform.position, muzzleFlash.transform.forward, out hit, range))
        {
            ButtonTarget target = hit.transform.GetComponent<ButtonTarget>();
            if (target != null)
            {
                target.PushButton();
            }
        }
    }
}

