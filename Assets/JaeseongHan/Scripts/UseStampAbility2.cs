using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseStampAbility2 : MonoBehaviour
{
    [Tooltip("None(0), Red(1), Blue(2), Yellow(3)")]
    [SerializeField] StampType2.StampType curType;
    private BaseState2[] states = new BaseState2[(int)StampType2.StampType.Size];

 //  Todo: 스크립트 병합시 사용
 //  [Header("State Reference Field")]
 //  [SerializeField] RedMover redMover;
   [SerializeField] BlueMover2 blueMover;
   [SerializeField] YellowMover2 yellowMover;

    /// <summary>
    /// 스탬프가 변경이 되었을 때 변경작업을 해주는 함수
    /// </summary>
    /// <param name="type">변경될 스탬프의 종류</param>
    public void ChangeAbility(StampType2.StampType type)
    {
        states[(int)curType].Exit();
        curType = type;
        states[(int)curType].Enter();
    }

    private void Awake()
    {
        // 처음 시작할 때 면에 아무것도 없음
        curType = StampType2.StampType.None;
    }

    private void Start()
    {
        // Todo: 스크립트 병합시 사용
        // redMover = new RedMover(this);
         blueMover = new BlueMover2(this);
         yellowMover = new YellowMover2(this);

        // 스크립트를 따로 생성하는거라 Awake 타이밍에 없을 수도 있어서
        // Start에서 추가
        states[(int)StampType2.StampType.None] = new NoneAblicity2(this);
        states[(int)StampType2.StampType.Red] = new NoneAblicity2(this);
        states[(int)StampType2.StampType.Blue] = blueMover;
        states[(int)StampType2.StampType.Yellow] = yellowMover;

        states[(int)curType].Enter();
    }

    private void Update()
    {
        states[(int)curType].Update();
    }

    /// <summary>
    /// 스탬프가 없는 상태일 때 -> 능력이 없는 상태
    /// </summary>
    private class NoneAblicity2 : BaseState2
    {
        // 작동할 로직은 따로 없음
        public NoneAblicity2(UseStampAbility2 parent) : base(parent)
        {
        }
    }

}

