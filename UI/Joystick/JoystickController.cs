using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform outerCircleRectTransform;
    [SerializeField] private RectTransform innerCircleRectTransform;
    [SerializeField] private RectTransform dashIndicatorTransform;
    [SerializeField] private RectTransform knobRectTransform;
    [SerializeField] private Color resetColor; // Color to change to when resetDashIndicator = false

    private Color originalColor;

    private Vector2 inputVector;
    private bool knobInOuterCircle = false;
    private Vector3 initialDashIndicatorScale = Vector3.one;
    private bool resetDashIndicator = false;
    private float scaleResetPerSecond = 0.0f;

    private void OnEnable()
    {
        LevelEventManager.OnPlayerDashed.AddListener(ResetDashIndicator);
        originalColor = outerCircleRectTransform.GetComponent<Image>().color; 
    }
    private void Start()
    {
        initialDashIndicatorScale = dashIndicatorTransform.localScale;
    }
    private void Update()
    {
        if (resetDashIndicator)
        {
            float addScale = scaleResetPerSecond * Time.deltaTime;
            dashIndicatorTransform.localScale += new Vector3(addScale, addScale, addScale);

            outerCircleRectTransform.GetComponent<Image>().color = originalColor;
        }
        else
        {
            outerCircleRectTransform.GetComponent<Image>().color = resetColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        // Convert the screen point to local point relative to the outer circle's RectTransform.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerCircleRectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            localPoint.x = (localPoint.x / outerCircleRectTransform.sizeDelta.x) * 2;
            localPoint.y = (localPoint.y / outerCircleRectTransform.sizeDelta.y) * 2;
            inputVector = localPoint;
            if (inputVector.magnitude > 1)
            {
                inputVector = inputVector.normalized;
            }
            knobRectTransform.anchoredPosition = new Vector2(inputVector.x * (outerCircleRectTransform.sizeDelta.x / 2), inputVector.y * (outerCircleRectTransform.sizeDelta.y / 2));

            float distance = Vector2.Distance(knobRectTransform.anchoredPosition, Vector2.zero);
            if (distance < innerCircleRectTransform.sizeDelta.x / 2)
            {
                knobInOuterCircle = false;
            }
            else
            {
                knobInOuterCircle = true;
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        knobRectTransform.anchoredPosition = Vector2.zero;
        knobInOuterCircle = false;
    }
    public Vector2 GetInputDirection()
    {
        return inputVector;
    }
    public bool IsKnobInOuterCircle()
    {
        return knobInOuterCircle;
    }
    private void ResetDashIndicator(float time)
    {
        StartCoroutine(ResetDashIndicatorCoroutine(time));
    }
    private IEnumerator ResetDashIndicatorCoroutine(float time)
    {
        dashIndicatorTransform.localScale = Vector3.zero;
        scaleResetPerSecond = initialDashIndicatorScale.x / time;
        resetDashIndicator = true;
        yield return new WaitForSeconds(time);
        resetDashIndicator = false;
    }
}
