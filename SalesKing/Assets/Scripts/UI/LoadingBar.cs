using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    RectTransform rect;
    public float speed;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.Rotate(0, 0, - (speed * Time.deltaTime));        
    }
}
