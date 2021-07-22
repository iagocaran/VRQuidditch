using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuldger : MonoBehaviour
{
    //vitesse quand il s'elogine
    public float speed = 100f;
    //vitesse quand il bouge aleatoirement
    public float vitesse = 500f;
    public GameObject joueur;
    private Rigidbody rb;

    private float chargeTime = 0.5f;
    private float timeCount;

    private float seuilemin = 80f;
    private float seuilemax = 400f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        timeCount += Time.deltaTime;


        Vector3 dis = (transform.position - joueur.transform.position).normalized;
        float mag = (transform.position - joueur.transform.position).magnitude;

        if (mag < seuilemin || mag >seuilemax)
        {
            rb.AddForce(-dis * speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
            Vector3 now = rb.position;
            now += transform.forward * Time.deltaTime * vitesse;
            rb.AddForce(transform.forward, ForceMode.VelocityChange);
            rb.position = now;

            //transform.position += transform.forward * Time.deltaTime * vitesse;

            if (timeCount > chargeTime)
            {
                Vector3 course = new Vector3(Random.Range(-180, 180), Random.Range(0, 180), Random.Range(0, 180));
                transform.localRotation = Quaternion.Euler(course);
                timeCount = 0;
            }
        }

        //Debug.Log(this.transform.position - joueur.transform.position);
        //Debug.DrawRay(transform.position, transform.forward);

    }
}
