using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private JoystickController joystick;
    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.transform.position = eventData.position;
        OnDrag(eventData);

    }
    public void OnDrag(PointerEventData eventData)
    {
        joystick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.OnPointerUp(eventData);
    }
}
