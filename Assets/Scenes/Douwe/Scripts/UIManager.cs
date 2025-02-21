using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;   
    public Text moneyText;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void UpdateMoneyUI(int amount)
    {
        moneyText.text = "Money: $" + amount;
    }

}
