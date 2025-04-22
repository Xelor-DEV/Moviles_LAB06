using UnityEngine;
using UnityEngine.Events;

public class TouchInputHandler : MonoBehaviour
{
    public UnityEvent OnHoldStarted = new UnityEvent();
    public UnityEvent OnHoldEnded = new UnityEvent();

    [SerializeField] private bool isHolding;

    private void Update()
    {
        HandleMobileInput();
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && isHolding == false)
            {
                isHolding = true;
                OnHoldStarted?.Invoke();
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isHolding == true)
            {
                isHolding = false;
                OnHoldEnded?.Invoke();
            }
        }
    }
}