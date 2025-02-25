using UnityEngine;
using UnityEngine.UI;

public class Ragdoll : MonoBehaviour
{
    [Header("NPC Ragdoll Settings")]
    public bool autoFindRagdoll = true;  // Option to auto-find ragdoll components
    public GameObject npc; // Reference to the NPC GameObject (set this if autoFindRagdoll is false)

    [Header("Trigger Collider Settings")]
    public Collider triggerCollider; // The specific trigger collider that will activate the ragdoll

    [Header("Ragdoll Deactivation Settings")]
    public bool deactivateAfterExit = true;  // Option to deactivate ragdoll after a delay
    public float deactivateDelay = 5f;  // Delay in seconds to deactivate ragdoll after exit

    private Rigidbody[] npcRigidbodies;
    private Collider[] npcColliders;
    private bool isRagdollActive = false;

    void Start()
    {
        // If auto-find is enabled, try to find the NPC's Rigidbody and Collider components automatically
        if (autoFindRagdoll)
        {
            if (npc == null)
            {
                npc = gameObject; // Use this GameObject if npc isn't assigned
            }

            // Automatically find all the Rigidbodies and Colliders in the NPC
            npcRigidbodies = npc.GetComponentsInChildren<Rigidbody>();
            npcColliders = npc.GetComponentsInChildren<Collider>();
        }
        else
        {
            // If auto-find is off, ensure npc is manually set in the inspector
            if (npc == null)
            {
                Debug.LogError("NPC GameObject is not assigned!");
                return;
            }
        }

        // Initially, ensure the ragdoll is disabled
        ToggleRagdoll(false);
    }

    // Only activate the ragdoll if the player enters the specified trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision happens in the assigned trigger collider
        if (other == triggerCollider && other.CompareTag("Player"))
        {
            ActivateRagdoll();
        }
    }

    // Optional: Deactivate the ragdoll when the player exits the trigger zone (after a delay)
    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider && other.CompareTag("Player"))
        {
            if (deactivateAfterExit)
            {
                // Delay deactivation after a specified time
                Invoke("DeactivateRagdoll", deactivateDelay);
            }
        }
    }

    void ActivateRagdoll()
    {
        // If ragdoll isn't active yet, enable it
        if (!isRagdollActive)
        {
            ToggleRagdoll(true);
        }
    }

    void DeactivateRagdoll()
    {
        // If ragdoll is active, disable it
        if (isRagdollActive)
        {
            ToggleRagdoll(false);
        }
    }

    void ToggleRagdoll(bool state)
    {
        isRagdollActive = state;

        // Enable/Disable the Rigidbody components of the NPC to switch between ragdoll state
        foreach (var rb in npcRigidbodies)
        {
            rb.isKinematic = !state;  // If ragdoll, make Rigidbody affected by physics
        }

        // Keep colliders enabled so the NPC can collide with the player
        foreach (var col in npcColliders)
        {
            col.enabled = true;  // Ensure colliders stay active for collision detection
        }
    }
}
