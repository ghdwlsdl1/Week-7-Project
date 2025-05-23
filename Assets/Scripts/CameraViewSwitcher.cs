using UnityEngine;
using UnityEngine.InputSystem;

public enum CameraViewMode
{
    FirstPerson,
    ThirdPersonBack
}

public class CameraViewSwitcher : MonoBehaviour
{
    public Transform firstPersonView;
    public Transform thirdPersonBackView;
    public Transform mainCameraTransform;

    [Header("1인칭 회전 설정")]
    public float lookSensitivity = 0.1f;
    public float minXLook = -85f;
    public float maxXLook = 85f;

    private float camCurXRot;
    private CameraViewMode currentMode = CameraViewMode.FirstPerson;

    private void Awake()
    {
        currentMode = CameraViewMode.FirstPerson;
        SwitchTo(currentMode);
    }

    private void LateUpdate()
    {
        if (currentMode != CameraViewMode.FirstPerson)
            return;

        var controller = CharacterManager.Instance.Player.controller;

        if (!controller.canLook)
            return;

        Vector2 mouseDelta = controller.GetMouseDelta();

        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        mainCameraTransform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.Rotate(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void SwitchView()
    {
        currentMode = (CameraViewMode)(((int)currentMode + 1) % 2);
        SwitchTo(currentMode);
    }

    private void SwitchTo(CameraViewMode mode)
    {
        var thirdPersonCam = mainCameraTransform.GetComponent<ThirdPersonCamera>();

        switch (mode)
        {
            case CameraViewMode.FirstPerson:
                if (thirdPersonCam != null)
                    thirdPersonCam.enabled = false;

                SetParentAndReset(mainCameraTransform, firstPersonView);
                break;

            case CameraViewMode.ThirdPersonBack:
                SetParentAndReset(mainCameraTransform, thirdPersonBackView);

                if (thirdPersonCam != null)
                {
                    thirdPersonCam.SetSensitivity(lookSensitivity);
                    thirdPersonCam.enabled = true;
                }
                break;
        }
    }

    private void SetParentAndReset(Transform camera, Transform target)
    {
        camera.SetParent(target);
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public bool IsThirdPerson()
    {
        return currentMode == CameraViewMode.ThirdPersonBack;
    }
}
