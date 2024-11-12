using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public bool isPlayerControlled = true;
    public Rigidbody rb;
    public GameObject[] up_thrusters;
    public GameObject[] down_thrusters;
    public GameObject[] left_thrusters;
    public GameObject[] right_thrusters;
    public GameObject[] forward_thrusters;
    public GameObject[] backward_thrusters;
    public GameObject[] pitch_up_thrusters;
    public GameObject[] pitch_down_thrusters;
    public GameObject[] yaw_left_thrusters;
    public GameObject[] yaw_right_thrusters;
    public GameObject[] roll_left_thrusters;
    public GameObject[] roll_right_thrusters;
    public float force_factor;
    public float torque_factor;
    public Transform cannon1;
    public Transform cannon2;
    public GameObject CannonFlash;
    public Rigidbody cannonProjectile;
    public bool isMouseToCamera;
    public int cannonSpeed;
    private HashSet<GameObject> all_thrusters = new HashSet<GameObject>();
    private HashSet<GameObject> thruster_particles_on = new HashSet<GameObject>();
    private Dictionary<GameObject, ParticleSystem> thrusterParticles = new Dictionary<GameObject, ParticleSystem>();
    
    private Vector3 thrustDirection;
    private Vector3 rotationDirection;
    public float cannonRate = 0.2F;
    private float nextFire = 0.0F;
    private bool cannon_alt = true;


    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false; 
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        //combine thruster groups to all_thruster
        all_thrusters.UnionWith(up_thrusters);
        all_thrusters.UnionWith(down_thrusters);
        all_thrusters.UnionWith(left_thrusters);
        all_thrusters.UnionWith(right_thrusters);
        all_thrusters.UnionWith(forward_thrusters);
        all_thrusters.UnionWith(backward_thrusters);
        all_thrusters.UnionWith(pitch_up_thrusters);
        all_thrusters.UnionWith(pitch_down_thrusters);
        all_thrusters.UnionWith(yaw_left_thrusters);
        all_thrusters.UnionWith(yaw_right_thrusters);
        all_thrusters.UnionWith(roll_left_thrusters);
        all_thrusters.UnionWith(roll_right_thrusters);

        //turn off all thrusters to start
        foreach (GameObject thruster in all_thrusters)
        {
            thrusterParticles[thruster] = thruster.GetComponent<ParticleSystem>();
            thrusterParticles[thruster].Stop();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerControlled)
        {
            getPlayerFlightInput();
            // fire cannon
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + cannonRate;
                if (cannon_alt)
                {
                    FireCannon(cannon1);
                }
                else
                {
                    FireCannon(cannon2);
                }
            }
        }
        FireThusters();
        addForces();
    }

    private void getPlayerFlightInput()
    {
        if (Input.GetKey("space"))
        {
            // all stop
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            thrustDirection = new Vector3(-localVelocity.x, -localVelocity.y, -localVelocity.z).normalized * 1;
            Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
            rotationDirection = new Vector3(-localAngularVelocity.x, -localAngularVelocity.y, -localAngularVelocity.z).normalized * 1;
        }

        else
        {
            if (isMouseToCamera)
            {
                rotationDirection = new Vector3(0, 0, Input.GetAxis("Roll") * 10);
            }
            else
            {
                rotationDirection = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), Input.GetAxis("Roll") * 10);
            }
            thrustDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Lift"), Input.GetAxis("Vertical"));
        }
    }
    private void ActivateThrusters(GameObject[] thrusters, float thrustValue)
    {
        if (thrustValue > 0)
        {
            thruster_particles_on.UnionWith(thrusters);
        }
    }
    private void FireThusters()
    {
        thruster_particles_on.Clear();

        ActivateThrusters(forward_thrusters, thrustDirection.z);
        ActivateThrusters(backward_thrusters, -thrustDirection.z);
        ActivateThrusters(right_thrusters, thrustDirection.x);
        ActivateThrusters(left_thrusters, -thrustDirection.x);
        ActivateThrusters(up_thrusters, thrustDirection.y);
        ActivateThrusters(down_thrusters, -thrustDirection.y);
        ActivateThrusters(pitch_up_thrusters, -rotationDirection.x);
        ActivateThrusters(pitch_down_thrusters, rotationDirection.x);
        ActivateThrusters(yaw_right_thrusters, rotationDirection.y);
        ActivateThrusters(yaw_left_thrusters, -rotationDirection.y);
        ActivateThrusters(roll_left_thrusters, rotationDirection.z);
        ActivateThrusters(roll_right_thrusters, -rotationDirection.z);

        // turn on thrusters
        foreach (GameObject thruster in thruster_particles_on)
        {
            thrusterParticles[thruster].Play();
        }

        foreach (GameObject thruster in all_thrusters.Except(thruster_particles_on))
        {
            thrusterParticles[thruster].Stop();
        }
        
    }

    private void addForces()
    {
        // add force for forward, backwards, left, right, up, and down
        rb.AddRelativeForce(thrustDirection.x*force_factor, thrustDirection.y*force_factor, thrustDirection.z*force_factor, ForceMode.Impulse);
        // add force for pitch, yaw, roll
        rb.AddRelativeTorque(rotationDirection.x*torque_factor, rotationDirection.y*torque_factor, rotationDirection.z*torque_factor, ForceMode.Impulse);
    }

    private void FireCannon(Transform cannon)
    {
        Rigidbody shell = Instantiate(cannonProjectile, cannon.position, cannon.rotation);
        shell.transform.Rotate(90,0,0, Space.Self);
        shell.transform.Translate(0,3,0, Space.Self);
        shell.velocity = rb.velocity + (transform.forward * cannonSpeed);
        CreateCannonFlash(cannon);
        ApplyCannonRecoil(cannon);
        cannon_alt = !cannon_alt;
    }

    private void CreateCannonFlash(Transform cannon)
    {
        GameObject flash = Instantiate(CannonFlash, cannon.position, cannon.rotation);
        flash.GetComponent<Rigidbody>().velocity = rb.velocity;
        Destroy(flash, 1);
    }

    private void ApplyCannonRecoil(Transform cannon)
    {
        rb.AddRelativeForce(0, 0, -0.05f, ForceMode.Impulse);
        rb.AddRelativeTorque(0, cannon.name == "Cannon1" ? -0.3f : 0.3f, 0, ForceMode.Impulse);
    }
}
