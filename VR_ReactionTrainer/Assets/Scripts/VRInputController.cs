using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR;

public class VRInputController : MonoBehaviour
{
    [Header("Reference")]
    public ReactionManager reactionManager;

    [Header("VR Controller")]
    public XRNode controllerNode = XRNode.RightHand;   // เลือกมือขวาเป็นค่าเริ่มต้น

    private InputDevice device;
    private bool lastPressed = false;

    void Start()
    {
        // หาอุปกรณ์ตามมือที่เลือก (ขวา/ซ้าย)
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    void Update()
    {
        if (reactionManager == null)
            return;

        // ถ้า device หลุด ให้ลองหาใหม่
        if (!device.isValid)
        {
            device = InputDevices.GetDeviceAtXRNode(controllerNode);
            if (!device.isValid)
                return;
        }

        bool isPressed = false;

        // อ่านปุ่ม triggerButton จาก controller
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed))
        {
            // กดลงครั้งแรกเท่านั้น (กันกดย้ำตอนถือค้าง)
            if (isPressed && !lastPressed)
            {
                reactionManager.OnButtonPressed();
            }

            lastPressed = isPressed;
        }
    }
}
