using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MoveSnitch : MonoBehaviour
{
    //vitesse quand il s'elogine
    public float speed = 10f;
    //vitesse quand il bouge aleatoirement
    public float vitesse = 3f;
    [Tooltip("Tous les joueurs")]
    public GameObject[] players;

    public GameObject joueurProche;
    private Rigidbody rb;

    private float chargeTime = 1f;
    private float timeCount;

    private float seuilemin = 10;
    private float seuilemax = 200;

    private float collisionconst = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        players = GameObject.FindGameObjectsWithTag("Player");
        ReloadClosestPlayer();
    }

    // Trouve le joueur le plus proche
    void ReloadClosestPlayer()
    {
        float min = Mathf.Infinity;
        int index = 0;
        for (int i = 0; i < players.Length; i++)
            if ((transform.position - players[i].transform.position).magnitude < min)
                index = i;
        joueurProche = players[index];
    }

    // Update is called once per frame
    void LateUpdate()
    {

        timeCount += Time.deltaTime;
      

        Vector3 dis = ( transform.position - joueurProche.transform.position).normalized;
        float mag = (transform.position - joueurProche.transform.position).magnitude;

        if (mag < seuilemin)
        {
            rb.AddForce(dis * speed);
        }else if(mag > seuilemax)
        {
            rb.AddForce(-dis * speed);
        }else
        {
            rb.velocity = Vector3.zero;
            Vector3 now = rb.position;            
            now += transform.forward * Time.deltaTime * vitesse;
            rb.AddForce(transform.forward, ForceMode.VelocityChange);
            rb.position = now;

            //transform.position += transform.forward * Time.deltaTime * vitesse;

            if (timeCount > chargeTime)
            {
                ReloadClosestPlayer();
                Vector3 course = new Vector3(Random.Range(-180, 180), Random.Range(0, 180), Random.Range(0, 180));
                transform.localRotation = Quaternion.Euler(course);
                timeCount = 0;
            }
        }
        
        //Debug.Log(this.transform.position - joueur.transform.position);
        //Debug.DrawRay(transform.position, transform.forward);

    }

    public GameObject grabHand;

    public enum Hand
    {
        RIGHT,
        LEFT
    }

    protected List<InputDevice> devices;

    private void Awake()
    {
        devices = new List<InputDevice>();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject o = collision.gameObject;
        if (o.name.EndsWith("hand"))
        {
            Hand hand;
            if (collision.gameObject.name == "Right hand")
            {
                hand = Hand.RIGHT;
            }
            else
            {
                hand = Hand.LEFT;
            }

            transform.parent = o.transform;

            devices.Clear();

            InputDeviceCharacteristics x = hand == Hand.RIGHT ? InputDeviceCharacteristics.Right : InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | x, devices);

            foreach (InputDevice d in devices)
            {
                d.SendHapticImpulse(0, 1.0f, 0.5f);
            }

            SceneManager.LoadScene("fin");
        }
        /*
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == "Terrain")
        {
            rb.velocity = Vector3.zero;
            Vector3 dis = ( transform.position - joueur.transform.position).normalized;
            dis = Quaternion.Euler(70f, -90f, -90f) * (-dis);
            rb.AddForce( dis* collisionconst);
        }
        */
    }

}
