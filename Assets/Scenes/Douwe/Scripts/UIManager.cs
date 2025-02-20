using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text moneyText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateMoneyUI(int amount)
    {
        moneyText.text = "Money: $" + amount;
    }

}
