using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;       // 현재 값
    public float startValue;     // 시작 시 초기값
    public float maxValue;       // 최대값
    public float passiveValue;   // 초당 자동 증감량
    public Image uiBar;          // UI 표시용 이미지 (fillAmount로 상태 표현)

    void Start()
    {
        // 시작 시 현재 값을 초기값으로 설정
        curValue = startValue;
    }

    void Update()
    {
        // 현재 값을 비율로 변환해 UI 바에 반영
        uiBar.fillAmount = GetPercentage();
    }

    // 현재 상태 비율 반환 (0.0 ~ 1.0)
    float GetPercentage()
    {
        return curValue / maxValue;
    }

    // 값을 증가시킴 (최대값을 넘지 않도록 제한)
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    // 값을 감소시킴 (0보다 작아지지 않도록 제한)
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
