using UnityEngine;
using UnityEngine.InputSystem;

public class DenialAbility : MonoBehaviour
{
    public GameObject DenialPlatform;
    public Transform spawnPoint;
    public float platLife = 5f;

    private InputAction denialAction;

    void Awake()
    {
        denialAction = InputSystem.actions.FindAction("Denial");

        if (denialAction == null)
        {
            Debug.LogError("Denial action not found in Input System!");
        }
    }

    void OnEnable()
    {
        if (denialAction != null)
        {
            denialAction.performed += SpawnPlatform;
            denialAction.Enable(); // Make sure the action is enabled
        }
    }

    void OnDisable()
    {
        if (denialAction != null)
        {
            denialAction.performed -= SpawnPlatform;
            denialAction.Disable(); // Disable the action when not needed
        }
    }

    void SpawnPlatform(InputAction.CallbackContext context)
    {
        if (DenialPlatform != null && spawnPoint != null)
        {
            GameObject newPlat = Instantiate(DenialPlatform, spawnPoint.position, Quaternion.identity);
            Destroy(newPlat, platLife);
        }
        else
        {
            Debug.LogError("DenialPlatform or spawnPoint is missing!");
        }
    }
}
