using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static CharacterManager _instance;

    // 외부에서 접근할 수 있는 정적 인스턴스
    public static CharacterManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새 GameObject에 생성
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // 실제 플레이어 인스턴스 보관용
    public Player _player;

    // 외부에서 접근할 수 있는 Player 프로퍼티
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        // 인스턴스가 없으면 자신을 인스턴스로 설정하고 파괴되지 않게 유지
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 자기 자신 파괴 (중복 방지)
            if (_instance == this)
            {
                Destroy(gameObject);
            }
        }
    }
}

