using UnityEngine;

// 일정 낙하 속도 이상일 때 데미지를 주는 컴포넌트
[RequireComponent(typeof(Rigidbody))]
public class FallDamageHandler : MonoBehaviour
{
    // 데미지를 받을 최소 낙하 속도
    public float fallThreshold = -10f;

    // 낙하 속도에 곱해지는 데미지 배율
    public float damageMultiplier = 2f;

    // 마지막 프레임의 Y축 속도 저장용
    private float lastYVelocity;

    // 리지드바디
    private Rigidbody rb;

    // 데미지를 전달할 대상
    private IDamagalbe damageReceiver;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        damageReceiver = GetComponent<IDamagalbe>();
    }

    // 매 프레임 낙하 속도 저장
    void Update()
    {
        lastYVelocity = rb.velocity.y;
    }

    // 충돌 시 낙하 속도가 임계값보다 크면 데미지 적용
    void OnCollisionEnter(Collision collision)
    {
        if (lastYVelocity < fallThreshold && damageReceiver != null)
        {
            float damage = Mathf.Abs(lastYVelocity) * damageMultiplier;
            damageReceiver.TakePhysicaIDamage(Mathf.RoundToInt(damage));
        }
    }
}
