using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Serializable]
    public struct MonsterInfo
    {
        [Header("참조 정보")]
        [SerializeField] private Transform _monster;

        [Header("몬스터 정보")]
        [SerializeField] private string _name;
        [SerializeField] private float _hp;
        [Range(0f, 100f)]
        [SerializeField] private float _atk;
        [SerializeField] private float _def;
        [SerializeField] private float _spd;
        [SerializeField] private bool _isBoss;

        public Transform Monster => _monster;
        public string Name => _name;
        public float HP => _hp;
        public float ATK => _atk;
        public float def => _def;
        public float Speed => _spd;
        public bool IsBoss => _isBoss;
    }

    [Header("참조 정보")]
    [SerializeField] private Transform _player;
    [SerializeField] private MonsterInfo[] _monsters;


    private void Start()
    {
        void Title(string title) => Debug.Log($"▶ [{title}]");
        void Section(string section) => Debug.Log($"=====[{section}]=====");
        void Line() => Debug.Log("=======================");
        void Log(string msg) => Debug.Log($"=== [Log]{msg}");
        void Warn(string msg) => Debug.Log($"=== [Warn]{msg}");

        Title("몬스터 위험도 분석기");
        Section("!경고! 분석을 시작합니다. !경고!");

        Line();

        if (_player == null)
        {
            Log("플레이어가 배정되지 않았습니다.");
            return;
        }

        if (_monsters == null)
        {
            Log("몬스터가 배정되지 않았습니다.");
            return;
        }

        Line();

        Log($"플레이어 위치 : {_player.position}");

        Line();

        foreach (MonsterInfo monsterinfo in _monsters)
        {
            if (monsterinfo.Monster == null)
            {
                Warn("참조되지 않은 몬스터가 있습니다.");
                continue;
            }


            Vector3 playerPos = _player.position;
            Vector3 monsterPos = monsterinfo.Monster.position;
            Vector3 tomonsterPos = monsterPos - playerPos;

            float distance = tomonsterPos.magnitude;

            string dangerLevel;

            if (distance <= 3f && monsterinfo.ATK >= 70f)
            {
                dangerLevel = "매우 위험";
            }

            else if (distance <= 7f && monsterinfo.ATK >= 40f)
            {
                dangerLevel = "위험";
            }

            else if (distance <= 12f)
            {
                dangerLevel = "경계";
            }

            else
            {
                dangerLevel = "안전";
            }

            dangerLevel = IncreaseDangerLevel(dangerLevel, monsterinfo.IsBoss);

            Section(monsterinfo.Name);
            Log($"몬스터 위치: {monsterPos}");
            Log($"거리: {distance:F2}");
            Log($"체력: {monsterinfo.HP}");
            Log($"공격력: {monsterinfo.ATK}");
            Log($"방어력: {monsterinfo.def}");
            Log($"이동 속도: {monsterinfo.Speed}");
            Log($"보스 여부: {monsterinfo.IsBoss}");

            if (distance == 0)
            {
                Warn("플레이어와 몬스터가 같은 위치에 있습니다.");
            }

            if (dangerLevel == "위험" || dangerLevel == "매우 위험")
            {
                Warn($"위험도 : {dangerLevel}");
            }

            else
            {
                Log($"위험도 : {dangerLevel}");
            }

            Line();
        }
    }

    private string IncreaseDangerLevel(string dangerLevel, bool isBoss)
    {
        if(!isBoss)
        {
            return dangerLevel;
        }

        switch (dangerLevel)
        {
            case "안전":
                return "경계";

            case "경계":
                return "위험";

            case "위험":
                return "매우 위험";

            case "매우 위험":
                return "매우 위험";

            default:
                return dangerLevel;
        }
    }


}

