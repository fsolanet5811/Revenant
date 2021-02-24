using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserInputMotionProvider : IMotionProvider
{
    public string HorizontalAxis { get; set; }

    public string VerticalAxis { get; set; }

    public UserInputMotionProvider(string horizontalAxis, string verticalAxis)
    {
        HorizontalAxis = horizontalAxis;
        VerticalAxis = verticalAxis;
    }

    public Vector2 GetMotion()
    {
        return new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
    }
}
