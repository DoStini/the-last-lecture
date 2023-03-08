using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    private float _time;
    private Label _minutes;
    private Label _seconds;
    
    // Start is called before the first frame update
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        _minutes = root.Q<Label>("Minutes");
        _seconds = root.Q<Label>("Seconds");
        _time = 0;
        
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _minutes.text = $"{Mathf.FloorToInt(_time / 60):00}";
        _seconds.text = $"{Mathf.FloorToInt(_time % 60):00}";
    }

    public void ResetTimer()
    {
        _time = 0;
        UpdateTimer();
    }

    public float GetTime()
    {
        return _time;
    }

    // Update is called once per frame
    private void Update()
    {
        _time += Time.deltaTime;
        
        UpdateTimer();
    }
}
