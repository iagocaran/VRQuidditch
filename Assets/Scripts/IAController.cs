using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public GameObject snitch;

    public GameObject bludger1, bludger2;

    public GameObject quaffle;

    [Tooltip("Self broom")]
    public GameObject broom;

    public enum Roles { Seeker, Chaser, Keeper, Beater};

    public Roles role;

    private Rigidbody rb;
    private Animator anim;

    [Range(0, 1)]
    [Tooltip("How much baited by balls")]
    public float lured = .5f;

    [Tooltip("How much afraid by bludgers")]
    public float fear = 10f;

    [Min(0)]
    [Tooltip("The maximum distance to see objects")]
    public float visibility = 30;

    [Min(0)]
    public float speed = 5;
    public float rotSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void SeekerBehavior()
    {
        Vector3 target = snitch.transform.position - transform.position;
        if (target.magnitude < visibility || true)
        {
            rb.AddForce(target.normalized * speed * Time.deltaTime);
            Debug.DrawRay(transform.position, target);
            //Quaternion rot = Quaternion.LookRotation(target, Vector3.up);
            //transform.LookAt(target);
            //rb.AddTorque(Quaternion.Slerp(transform.rotation, rot, lured).eulerAngles * rotSpeed);
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
            // On l'écarte des bludgers


        } else
        {
            rb.AddForce(target.normalized * speed * lured * Time.deltaTime);
            Debug.DrawRay(transform.position, target);
            //rb.AddTorque(new Vector3(Random.value, 0, Random.value) * rotSpeed);
        }

        if (target.magnitude < 1)
        {
            anim.SetBool("CanGrabBall", true);
        } else
        {
            anim.SetBool("CanGrabBall", false);
        }

        Vector3 bludger1vec = bludger1.transform.position - transform.position;
        if (bludger1vec.magnitude < 10)
            rb.AddForce(bludger1vec.normalized * speed * Time.deltaTime * fear, ForceMode.Acceleration);
        Vector3 bludger2vec = bludger2.transform.position - transform.position;
        if (bludger2vec.magnitude < 10)
            rb.AddForce(bludger2vec.normalized * speed * Time.deltaTime * fear, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (role)
        {
            case Roles.Seeker:
                SeekerBehavior();
                break;
            case Roles.Chaser:
                break;
            case Roles.Beater:
                break;
        }
    }
}
