using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class QuaffleController : MonoBehaviour
{
    public float maxSpeed;
    private Rigidbody rb;
    private float timeCount;
    public GameObject player;
    protected List<InputDevice> devices;
    private GameObject attachedHand = null;

    private float grabCooldown = 0f;

    public GameObject head;

    enum ColState
    {
        Front,
        Back,
        None
    };
    private ColState colstate = ColState.None;

    public enum Hand
    {
        RIGHT,
        LEFT
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        devices = new List<InputDevice>();
    }

    private void FixedUpdate()
    {
        if (attachedHand != null) {
            Hand hand = Hand.LEFT;

            devices.Clear();

            InputDeviceCharacteristics x = hand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

            foreach (InputDevice d in devices)
            {
                if (d.TryGetFeatureValue(CommonUsages.trigger, out float t) && t > 0.3)
                {
                    grabCooldown = 0.1f;

                    //transform.parent = null;
                    attachedHand = null;
                    transform.localScale *= 5f;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    Vector3 dir = (attachedHand.transform.position - (head.transform.position - Vector3.up*0.2f)).normalized;
                    rb.detectCollisions = false;
                    rb.AddForce(dir * 200f, ForceMode.Acceleration);
                    /*                    rb.useGravity = true;
                                        rb.isKinematic = true;
                                        rb.detectCollisions = true;
                                        rb.freezeRotation = false;*/

                    
                }
            }
        } else {
            rb.AddForce(-0.9f * Physics.gravity * rb.mass);
        }
    }

    private void LateUpdate()
    {

        if (attachedHand != null)
        {
            transform.position = attachedHand.transform.position;
        }
        
        timeCount += Time.deltaTime;
        grabCooldown -= Time.deltaTime;
        if (grabCooldown < 0)
        {
            rb.detectCollisions = true;
        }

        // Check if close to the player hand


        // Check if through a ring and score a point


        // Check if out of field and throw it back


    }

    private void OnTriggerExit(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.name == "GoalCollider")
        {
            colstate = ColState.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Debug.Log(colstate);
        // goal
        GameObject o = other.gameObject;
        if (o.name == "FrontBox" && colstate == ColState.None)
        {
            colstate = ColState.Front;
        }
        if (o.name == "BackBox" && colstate == ColState.None)
        {
            colstate = ColState.Back;
        }
        if (o.name == "BackBox" && colstate == ColState.Front)
        {
            GameObject.Find("dix points").GetComponent<AudioSource>().Play();
            GameObject.Find("ding").GetComponent<AudioSource>().Play();
            GameObject.Find("barca").GetComponent<AudioSource>().Play();
            colstate = ColState.None;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject o = collision.gameObject;

        // grab hand
        if (attachedHand != null) return;

        
        if (o.name == "Left hand" && grabCooldown <= 0)
        {
            Hand hand = Hand.LEFT;
/*            if (collision.gameObject.name == "Right hand")
            {
                hand = Hand.RIGHT;
            }
            else
            {
                hand = Hand.LEFT;
            }*/

            // Attach the ball to the player hand
            attachedHand = o;
/*            rb.useGravity = false;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.detectCollisions = false;
            rb.freezeRotation = true;*/
            transform.localScale /= 5;
            //rb.detectCollisions = false;
            //transform.parent = attachedHand.transform;

            devices.Clear();

            InputDeviceCharacteristics x = hand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

            foreach (InputDevice d in devices)
            {
                d.SendHapticImpulse(0, 1.0f, 0.5f);
            }
        }
    }
}
