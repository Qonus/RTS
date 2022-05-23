using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerControls controls;

    [SerializeField] float rotationSens = 0.6f;
    [SerializeField] float moveSpeed = 0.3f;
    [SerializeField] float zoomSens = 0.01f;
    [SerializeField] float zoomSmooth = 0.3f;
    [SerializeField] float minZoom = 1f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] AnimationCurve zoomCurve;
    Vector2 moveInput;
    float rotationAxis;
    bool rotate;
    float zoom;
    float dist;

    Transform cam;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        cam = Camera.main.transform;
        dist = Vector3.Distance(cam.position, transform.position);
    }

    void Update()
    {
        Inputs();

        if (rotate)
        {
            transform.eulerAngles += new Vector3(0, rotationAxis, 0);
        }

        transform.position += moveInput.y * transform.forward + moveInput.x * transform.right;

        cam.LookAt(transform);
        dist -= zoom * zoomSens;
        dist = Mathf.Clamp(dist, minZoom, maxZoom);
        float y = (zoomCurve.Evaluate(dist / (maxZoom - minZoom)) * (maxZoom - minZoom)) + minZoom;
        Vector3 target = (dist * (cam.position - transform.position).normalized) + transform.position;
        target.y = y;
        cam.position = Vector3.Lerp(cam.position, target, zoomSmooth * Time.deltaTime);
    }

    void Inputs()
    {
        moveInput = controls.Cam.Move.ReadValue<Vector2>() * moveSpeed;
        rotationAxis = controls.Cam.Rotate.ReadValue<float>() * rotationSens;
        rotate = controls.Cam.RotateToggle.inProgress;
        zoom = controls.Cam.Zoom.ReadValue<float>();
    }
}
