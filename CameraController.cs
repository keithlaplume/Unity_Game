using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraFollowTarget;
    public Transform cameraCockpit;
    public Transform cameraFreeLookTarget;
    public Transform FreeLookPivot;
    public float timeFactor;
    public GameObject SpaceshipObject;

    private Spaceship Spaceship;
    private Vector3 velocity = Vector3.zero;
    private enum CameraMode { Follow, Cockpit, FreeLook }
    private CameraMode currentMode = CameraMode.Follow;
    private float t = 0.0f;
    private float fieldOfViewTarget = 60f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = cameraFollowTarget.position;
        transform.rotation = cameraFollowTarget.rotation;
        Spaceship = SpaceshipObject.GetComponent<Spaceship>();
    }

    // Update is called once per frame
    void Update()
    {
        //switch view
        if(Input.GetKey("1"))
        {
            SetCameraMode(CameraMode.Cockpit);
        }
        if(Input.GetKey("2"))
        {
            SetCameraMode(CameraMode.Follow);
        }
        if(Input.GetKey("3"))
        {
            SetCameraMode(CameraMode.FreeLook);
        }
        
        //zoom
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            fieldOfViewTarget = 20f;
            t = 0.0f;
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            fieldOfViewTarget = 60f;
            t = 0.0f;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fieldOfViewTarget, t);
        t += Time.deltaTime;
        
        if (currentMode == CameraMode.Cockpit)
        {
            transform.position = cameraCockpit.position;
            transform.rotation = cameraCockpit.rotation;
        }
        if (currentMode == CameraMode.FreeLook)
        {
            transform.position = cameraFreeLookTarget.position;
            transform.rotation = cameraFreeLookTarget.rotation;

            FreeLookPivot.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0, Space.Self);
        }
    }
    void FixedUpdate()
    {
        if (currentMode == CameraMode.Follow)
        {
            transform.position = Vector3.SmoothDamp(transform.position, cameraFollowTarget.position, ref velocity, timeFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraFollowTarget.rotation, timeFactor);
        }     
    }

    private void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
        if (mode == CameraMode.FreeLook)
        {
            Spaceship.isMouseToCamera = true;
            FreeLookPivot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Spaceship.isMouseToCamera = false;
        }
    }
}
