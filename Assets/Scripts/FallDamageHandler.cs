using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDamageHandler : MonoBehaviour
{
    public float fallThreshold = -10f;
    public float damageMultiplier = 2f;

    private float lastYVelocity;
    private Rigidbody rb;
    private IDamagalbe damageReceiver;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        damageReceiver = GetComponent<IDamagalbe>();
    }

    void Update()
    {
        lastYVelocity = rb.velocity.y;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (lastYVelocity < fallThreshold && damageReceiver != null)
        {
            float damage = Mathf.Abs(lastYVelocity) * damageMultiplier;
            damageReceiver.TakePhysicaIDamage(Mathf.RoundToInt(damage));
        }
    }
}
