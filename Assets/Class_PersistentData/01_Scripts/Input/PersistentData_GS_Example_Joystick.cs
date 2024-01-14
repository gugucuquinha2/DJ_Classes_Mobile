using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PersistentData_GS_Example_Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private RectTransform background;
	private RectTransform handle;
	private Vector2 originalPosition;
	private Vector2 output = Vector2.zero;

	public enum JoystickModeEnum 
    { 
        Fixed, 
        Dynamic 
    };
	public JoystickModeEnum joystickMode = JoystickModeEnum.Fixed;

	[Range(0f, 0.5f)]
	public float deadZone = 0.2f;

	private void Start()
	{
        // get references of all important components
		background = GetComponentsInChildren<RectTransform>()[1];
		handle = background.GetComponentsInChildren<RectTransform>()[1];

        // default values for the joystick
		output = Vector2.zero;
		background.anchoredPosition = Vector2.zero;
		handle.anchoredPosition = Vector2.zero;
		originalPosition = background.anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
        // position the joystick based on the touch position
		if (joystickMode == JoystickModeEnum.Dynamic)
		{
			background.position = eventData.position;
			handle.anchoredPosition = Vector2.zero;
        }

        CalculateOutput(eventData.position);	
	}

	public void OnDrag(PointerEventData eventData)
	{
        CalculateOutput(eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        // when we stop dragging the joystick, reset the position
		output = Vector2.zero;
		handle.anchoredPosition = Vector2.zero;
		background.anchoredPosition = originalPosition;
	}

    private void CalculateOutput (Vector2 _position)
    {
		// get the direction of the joystick movement by comparing the background position to the touch position
        Vector2 movementDirection = _position - (Vector2)background.position;

		// get the joystick radius since that's the valid range for the joystick
        float joystickRadius = background.sizeDelta.x * 0.5f;
		// calculate the "ignorable" area based on the user-defined deadzone
		float deadSize = deadZone * joystickRadius;

        // if we don't reach the minimum amount of input, don't move the handle nor consider the output
		if (movementDirection.magnitude < deadSize)
		{
			output = Vector2.zero;
			handle.anchoredPosition = Vector2.zero;
			return;
		}

        // only calculate the output until the end of the joystick limit (from 0 to 1)
		if (movementDirection.magnitude < joystickRadius)
        {
            output = movementDirection.normalized * ((movementDirection.magnitude - deadSize) / (joystickRadius - deadSize));
        }
        else
        {
            output = movementDirection.normalized;
        }

        // position the handle UI
		handle.anchoredPosition = output * joystickRadius;
    }

	// GET THE OUTPUT VALUE SO WE CAN USE IT IN OTHER CLASSES
	public Vector2 GetJoystickOutput()
    {
		return output;
    }
}