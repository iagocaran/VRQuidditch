using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;

public class VRSupermanMovement : MonoBehaviour
{
    public enum Hand
    {
        RIGHT,
        LEFT
    }

    public float movementSpeed = 10.0f;

    protected List<InputDevice> devices;

    private bool grabbing = false;
    public Hand grabHand;

    private void Awake()
    {
        devices = new List<InputDevice>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject broom = GameObject.Find("Broom");
        broom.GetComponent<LookAtConstraint>().enabled = false;
        //if (grabbing)
        //{
            devices.Clear();

            InputDeviceCharacteristics x = grabHand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

            foreach (InputDevice d in devices)
            {
                float t;
                if (d.TryGetFeatureValue(CommonUsages.trigger, out t) && t > 0.1)
                {
                broom.GetComponent<LookAtConstraint>().enabled = true;
                //    grabbing = false;
                //}
                //if (grabbing)
                //{
                GameObject hand;
                    if (grabHand == Hand.RIGHT)
                        hand = GameObject.Find("Right hand");
                    else
                        hand = GameObject.Find("Left hand");
                    GameObject br = GameObject.Find("Broom");
                    Vector3 dir = (hand.transform.position - br.transform.position).normalized;
                    GetComponent<Rigidbody>().MoveRotation(Quaternion.FromToRotation(Vector3.forward, dir));
                    GetComponent<Rigidbody>().AddForce(dir * t * movementSpeed);
                }

            }
        //}
    }

/*    private void OnCollisionStay(Collision collision)
    {
        if (!grabbing && collision.gameObject.name.EndsWith("hand"))
        {
            devices.Clear();

            InputDeviceCharacteristics x = grabHand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

            foreach (InputDevice d in devices)
            {
                float t;
                if (d.TryGetFeatureValue(CommonUsages.trigger, out t) && t > 0.1 && !grabbing)
                {
                    grabbing = true;
                    if (collision.gameObject.name == "Right hand")
                    {
                        grabHand = Hand.RIGHT;
                    } else
                    {
                        grabHand = Hand.LEFT;
                    }
                }
            }
        }
    }*/
}
