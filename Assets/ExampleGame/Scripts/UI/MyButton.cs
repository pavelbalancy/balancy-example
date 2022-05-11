using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : Button
{
    private const float DOWN_SCALE = 0.95f;
    private Vector3 _defaultScale;

    protected override void Awake()
    {
        base.Awake();
        _defaultScale = transform.localScale;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) 
            return;

        base.OnPointerDown(eventData);

        transform.localScale = _defaultScale * DOWN_SCALE;
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        transform.localScale = _defaultScale;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        AudioManager.PlayButtonClick();
    }
}
