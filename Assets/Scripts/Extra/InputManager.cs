using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static Vector3 GetKeyboardMove()
    {
        var move = new Vector3
            (
            Input.GetAxis(ConstantStrings.AxisNameHorizontal),
            0f,
            Input.GetAxis(ConstantStrings.AxisNameVertical)
            );

        if (Input.GetButton(ConstantStrings.ButtonNameJump))
        {
            move.y = 1f;
        }

        return move;
    }

    public static float GetHorizontalMouseMove()
    {
        return Input.GetAxis(ConstantStrings.AxisNameMouseX);
    }

    public static float GetVerticalMouseMove()
    {
        return Input.GetAxis(ConstantStrings.AxisNameMouseY);
    }
}
