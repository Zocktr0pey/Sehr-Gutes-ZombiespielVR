using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private PlayerControls playerControls;

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Sorgt dafür das immer nur eine Instanz dieses Managers existiert
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool GetContinuousFire()
    {
        return playerControls.Player.Attack.IsPressed();
    }

    public bool GetSingleFire()
    {
        return playerControls.Player.Attack.WasPerformedThisFrame();
    }
}
