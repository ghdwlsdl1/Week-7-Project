using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;             // 피격 시 표시할 이미지
    public float flashSpeed;       // 깜빡임이 사라지는 속도

    private Coroutine coroutine;   // 현재 실행 중인 코루틴

    void Start()
    {
        // 플레이어가 데미지를 받을 때 Flash() 실행
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    // 데미지를 받을 때 호출됨
    public void Flash(int damage)
    {
        // 이전 깜빡임 효과가 남아 있다면 중단
        if (coroutine != null) StopCoroutine(coroutine);

        // 데미지에 비례해서 깜빡임 속도 설정 (데미지 클수록 오래 깜빡임)
        flashSpeed = damage * 0.05f;

        // 이미지 초기 상태 설정 및 활성화
        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f); // 불투명한 붉은 색

        // 서서히 사라지는 코루틴 시작
        coroutine = StartCoroutine(FadeAway());
    }

    // 이미지의 알파값을 줄이며 사라지게 만드는 코루틴
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;    // 시작 알파값
        float a = startAlpha;

        // 알파가 0보다 클 동안 점점 줄임
        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        // 다 사라졌으면 이미지 비활성화
        image.enabled = false;
    }
}
