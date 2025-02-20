using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    
    public static MoneyManager instance;
    public int money = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UIManager.instance.UpdateMoneyUI(money);
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UIManager.instance.UpdateMoneyUI(money);
            return true; // Purchase successful
        }
        return false; // Not enough money
    }

}
