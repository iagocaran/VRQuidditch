using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject camera;
    public float sens = 20;
    float speed = 10.0f;

    public Vector2 rotationSpeed = new Vector2(0.1f, 0.1f);
    public bool reverse;

  
    private Vector2 lastMousePosition;
    private Vector2 newAngle = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //camera = GetComponentInChildren<GameObject>("Camera");
    }
	

    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            newAngle = camera.transform.localEulerAngles;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!reverse)
            {
                newAngle.y -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.y;
                newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.x;

                camera.transform.localEulerAngles = newAngle;
                lastMousePosition = Input.mousePosition;
            }
            else
            {
                newAngle.y -= (Input.mousePosition.x - lastMousePosition.x) * rotationSpeed.y;
                newAngle.x -= (lastMousePosition.y - Input.mousePosition.y) * rotationSpeed.x;

                camera.transform.localEulerAngles = newAngle;
                lastMousePosition = Input.mousePosition;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // rb.AddForce(new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")) * sens);
        if (Input.GetKey("up"))
        {
            transform.position += transform.forward * sens * Time.deltaTime;
        }
        if (Input.GetKey("down"))
        {
            transform.position -= transform.forward * sens * Time.deltaTime;
        }
        if (Input.GetKey("right"))
        {
            transform.position += transform.right * sens * Time.deltaTime;
        }
        if (Input.GetKey("left"))
        {
            transform.position -= transform.right * sens * Time.deltaTime;
        }
    }
}
