using Invector.CharacterController;
using UnityEngine;
#if UNITY_5_3_OR_NEWER
#endif

public class InputController : MonoBehaviour
{
    #region variables
    [Header("General")]
    public KeyCode interact = KeyCode.F;
    public KeyCode inventory = KeyCode.I;
    public KeyCode character = KeyCode.C;

    [Header("Default Inputs")]
    public string horizontalInput = "Horizontal";
    public string verticallInput = "Vertical";
    public KeyCode jumpInput = KeyCode.Space;
    public KeyCode strafeInput = KeyCode.Tab;
    public KeyCode sprintInput = KeyCode.LeftShift;

    [Header("Camera Settings")]
    public string rotateCameraXInput = "Mouse X";
    public string rotateCameraYInput = "Mouse Y";

    protected vThirdPersonCamera tpCamera;                // acess camera info        
    [HideInInspector]
    public string customCameraState;                    // generic string to change the CameraState        
    [HideInInspector]
    public string customlookAtPoint;                    // generic string to change the CameraPoint of the Fixed Point Mode        
    [HideInInspector]
    public bool changeCameraState;                      // generic bool to change the CameraState        
    [HideInInspector]
    public bool smoothCameraState;                      // generic bool to know if the state will change with or without lerp  
    [HideInInspector]
    public bool keepDirection;                          // keep the current direction in case you change the cameraState

    protected vThirdPersonController cc;                // access the ThirdPersonController component                

    #endregion

    public delegate void OnInteract(Player player); public event OnInteract onInteract;
    public delegate void OnInventory(); public event OnInventory onInventory;
    public delegate void OnCharacter(); public event OnCharacter onCharacter;

    protected virtual void Start()
    {
        CharacterInit();
    }

    protected virtual void CharacterInit()
    {
        cc = FindObjectOfType<vThirdPersonController>();
        if (cc != null)
            cc.Init();

        tpCamera = FindObjectOfType<vThirdPersonCamera>();
        if (tpCamera) tpCamera.SetMainTarget(cc.transform);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void LateUpdate()
    {
        if (cc == null) return;             // returns if didn't find the controller	
        if (Input.GetKeyDown(interact))
            onInteract?.Invoke(cc.GetComponent<Player>());
        if (Input.GetKeyDown(inventory))
            onInventory?.Invoke();
        InputHandle();                      // update input methods
        UpdateCameraStates();               // update camera states
    }

    protected virtual void FixedUpdate()
    {
        cc.AirControl();
        CameraInput();
    }

    protected virtual void Update()
    {
        cc.UpdateMotor();                   // call ThirdPersonMotor methods               
        cc.UpdateAnimator();                // call ThirdPersonAnimator methods		               
    }

    protected virtual void InputHandle()
    {
        ExitGameInput();

        CameraInput();
        if (!cc.lockMovement)
        {
            MoveCharacter();
            SprintInput();
            StrafeInput();
            JumpInput();
        }
    }

    #region Basic Locomotion Inputs      

    protected virtual void MoveCharacter()
    {
        cc.input.x = Input.GetAxis(horizontalInput);
        cc.input.y = Input.GetAxis(verticallInput);
    }

    protected virtual void StrafeInput()
    {
        if (Input.GetKeyDown(strafeInput))
            cc.Strafe();
    }

    protected virtual void SprintInput()
    {
        if (Input.GetKeyDown(sprintInput))
            cc.Sprint(true);
        else if (Input.GetKeyUp(sprintInput))
            cc.Sprint(false);
    }

    protected virtual void JumpInput()
    {
        if (Input.GetKeyDown(jumpInput))
            cc.Jump();
    }

    protected virtual void ExitGameInput()
    {
        // just a example to quit the application 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cursor.visible)
                Cursor.visible = true;
            else
                Application.Quit();
        }
    }

    #endregion

    #region Camera Methods

    protected virtual void CameraInput()
    {
        if (tpCamera == null)
            return;
        var Y = Input.GetAxis(rotateCameraYInput);
        var X = Input.GetAxis(rotateCameraXInput);

        tpCamera.RotateCamera(X, Y);

        // tranform Character direction from camera if not KeepDirection
        if (!keepDirection)
            cc.UpdateTargetDirection(tpCamera != null ? tpCamera.transform : null);
        // rotate the character with the camera while strafing        
        RotateWithCamera(tpCamera != null ? tpCamera.transform : null);
    }

    protected virtual void UpdateCameraStates()
    {
        // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData
        if (tpCamera == null)
        {
            tpCamera = FindObjectOfType<vThirdPersonCamera>();
            if (tpCamera == null)
                return;
            if (tpCamera)
            {
                tpCamera.SetMainTarget(this.transform);
                tpCamera.Init();
            }
        }
    }

    protected virtual void RotateWithCamera(Transform cameraTransform)
    {
        if (cc.isStrafing && !cc.lockMovement && !cc.lockMovement)
        {
            cc.RotateWithAnotherTransform(cameraTransform);
        }
    }

    #endregion
}