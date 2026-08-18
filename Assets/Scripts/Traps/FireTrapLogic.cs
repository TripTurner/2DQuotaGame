using UnityEngine;

public class FireTrapLogic : MonoBehaviour
{
    public GameObject fire;
    [SerializeField] private float offTime;
    [SerializeField] private float onTime;
    private float timer;
    private string state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire.SetActive(false);
        state = "off";
        timer = offTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0 && state=="off") {
            fire.SetActive(true);
            timer = onTime;
            state = "on";
        } else if (timer<=0 && state=="on") {
            fire.SetActive(false);
            timer = offTime;
            state = "off";
        }
    }
}
