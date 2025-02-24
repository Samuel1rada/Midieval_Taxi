using UnityEngine;
using TMPro;
using MalbersAnimations.Controller;

public class SpeedometerUI : MonoBehaviour
{
    public MAnimal horse;  // Assign the horse in the Inspector
    public TextMeshProUGUI speedText; // Assign a UI TextMeshPro element

    void Update()
    {
        if (horse != null && speedText != null)
        {
            float speed = horse.MovementAxis.z; // Get forward movement speed
            speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
        }
    }
}
