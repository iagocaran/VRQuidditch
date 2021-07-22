using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRHand : MonoBehaviour
{
    public enum Hand
    {
        RIGHT,
        LEFT
    }

    public Hand hand;

    protected List<InputDevice> devices;

    private void Awake()
    {
        devices = new List<InputDevice>();
    }

    // Update is called once per frame
    void Update()
    {
        devices.Clear();

        InputDeviceCharacteristics x = hand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

        foreach (InputDevice d in devices)
        {
            if (d.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
            {
                if (d.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
                {
                    transform.localPosition = pos;
                    transform.localRotation = rot;
                }
            }
        }
    }
}
