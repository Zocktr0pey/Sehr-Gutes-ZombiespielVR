using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class MenuPlayerScript : MonoBehaviour
{
    [SerializeField] private float rotationInDegrees = 30;
    [SerializeField] private GameObject currentGun;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject vrRig;

    private InputManager inputManager;
    private AudioManager audioManager;
    private Gun gun;

    // Für rotation mit linken Stick
    private bool hasSnapped = false;
    private float snapThreshold = 0.8f;
    private float snapReset = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        gun = currentGun.GetComponent<Gun>();

        Rotation();

        // Schiessen!
        // Einzelschuss
        if (gun != null)
        {
            Gun();
        }
    }

    void Rotation()
    {
        //Vector2 input = inputManager.GetRightStick();

        //if (!hasSnapped && Mathf.Abs(input.x) > snapThreshold)
        //{
        //    Debug.Log("Get past this if " + input);
        //    if (input.x < 0)
        //    {
        //        vrRig.transform.eulerAngles -= new Vector3(0, rotationInDegrees, 0);
        //    }
        //    else
        //    {
        //        vrRig.transform.eulerAngles += new Vector3(0, rotationInDegrees, 0);
        //    }

        //    hasSnapped = true;
        //    return;
        //}

        //if (hasSnapped && input.magnitude < snapReset)
        //{
        //    hasSnapped = false;
        //}

    }

    void Gun()
    {
        if (inputManager.GetSingleFire() && !gun.isFullAuto)
        {
            gun.Shoot();
        }

        // Dauerfeuer (wird bei JEDEM Frame getriggert)
        if (inputManager.GetContinuousFire() && gun.isFullAuto)
        {
            gun.Shoot();
        }

        if (inputManager.GetReload())
        {
            gun.Reload();
        }
    }
}
