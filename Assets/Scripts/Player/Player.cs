using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;     // 플레이어 움직임 제어 컴포넌트
    public PlayerCondition condition;       // 체력, 스태미너 등 상태 관리 컴포넌트

    public Action addItem;                  // 아이템 획득 시 호출될 이벤트

    public Transform dropPosition;          // 아이템 드롭 위치

    private void Awake()
    {
        // 전역 접근용 싱글톤 설정
        CharacterManager.Instance.Player = this;

        // 컴포넌트 가져오기
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}

