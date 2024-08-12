using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    AimBaseState currentState;
    public HipFireState Hipfire = new HipFireState();
    public AimState Aiming = new AimState();

    [SerializeField] float rotationSpeed = 1;
    [SerializeField] Transform followPosition;
    [SerializeField] float maxVertAngle = 45;
    [SerializeField] float minVertAngle = -45;

    [SerializeField] public Camera mainCamera;
    public float adsFov = 35;
    [HideInInspector] public float currentFov;
    [HideInInspector] public float hipFov;
    public float fovSmoothTrans = 10;


    float rotationY;
    float rotationX;

    [HideInInspector] public Animator animator;
    public Transform aimPosition;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask; // prevents us from aiming at ourselves

    void Start()
    {

        LockCursor();
        mainCamera = Camera.main.GetComponent<Camera>();
        hipFov = mainCamera.fieldOfView;
        animator = GetComponent<Animator>();
        SwitchState(Hipfire);
    }

    // Update is called once per frame
    void Update()
    {
        //rotationX += Input.GetAxis("Mouse Y");
        // rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        //rotationX = Mathf.Clamp(rotationX, minVertAngle, maxVertAngle);
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Cursor.lockState == CursorLockMode.Locked)
            {

                UnlockCursor();
            }
            else
            {

                LockCursor();
            }
        }
        currentState.UpdateState(this);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, currentFov, fovSmoothTrans * Time.deltaTime);
        //smoothly transitioning from hipfire to aiming state

        Vector2 ScreenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(ScreenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPosition.position = Vector3.Lerp(aimPosition.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        // acts as a crosshair towards where we are aiming based on the ray from the camera
    }

    private void LateUpdate()
    {
        // followPosition.localEulerAngles = new Vector3(rotationX, followPosition.localEulerAngles.y,followPosition.localEulerAngles.z);
        // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, transform.eulerAngles.z);

    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
        //aim state manager to be passed in
    }

    public bool IsAiming()
    {
        return currentState == Aiming;
        //returns true if aiming
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    public Vector3 GetAimDirection()
    {
        return (aimPosition.position - transform.position).normalized;
    }

}
