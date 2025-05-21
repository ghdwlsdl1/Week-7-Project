using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 15f; // 점프대로 튕겨줄 힘

    private void OnCollisionEnter(Collision collision)
    {
        // 닿은 대상에 Rigidbody가 있으면
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 수직 방향으로 힘을 가함
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
