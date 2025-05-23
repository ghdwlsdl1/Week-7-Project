using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector2 sensitivity = new Vector2(1f, 1f); // 외부에서 설정 가능하게

    public float distance = 5f;
    public float height = 2f;

    private float yaw = 0f;
    private float pitch = 10f;

    void LateUpdate()
    {
        if (!enabled || target == null)
            return;

        if (!CharacterManager.Instance.Player.controller.canLook)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;

        yaw += mouseDelta.x;
        pitch -= mouseDelta.y;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset = rotation * new Vector3(0f, 0f, -distance) + Vector3.up * height;
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * height);
    }

    public void SetSensitivity(float value)
    {
        sensitivity = new Vector2(value, value);
    }
}
