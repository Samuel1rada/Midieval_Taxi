using UnityEngine;

public class test : MonoBehaviour
{
    private int owned_cash = 0;  
    private int salary = 5;

    public float timeRemaining; 
    public float amountRemaining = 3;
    public bool timerIsRunning = false;

    [SerializeField] private popup mypopup;  

  
    void Start()
    {

        OpenPopup();  
                      
        mypopup.completed.onClick.AddListener(OpenClicked);  
                                                             
        mypopup.close.onClick.AddListener(CloseClicked);
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Reduce the time remaining
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = amountRemaining;
                timerIsRunning = false;
                mypopup.gameObject.SetActive(false);
            }
        }
    }

    private void OpenPopup()
    {
        
        mypopup.textMeshPro.text = "Current Cash: " + owned_cash.ToString();  
    }

    private void CloseClicked()
    {
        mypopup.gameObject.SetActive(false);  
        Debug.Log("closed");
    }

    private void OpenClicked()
    {
        owned_cash += salary;  
        mypopup.textMeshPro.text = "Current Cash: " + owned_cash.ToString();  
        mypopup.gameObject.SetActive(true);  
        Debug.Log($"New owned cash: {owned_cash}");
        timerIsRunning = true;
    }

}
