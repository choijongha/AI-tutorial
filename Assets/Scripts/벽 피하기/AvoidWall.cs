using System.Collections;
using UnityEngine;

/// <summary>
/// 앞에 보이는 것을 벽이나 장애물로 간주하고 동시에 원래 향하고자 했던 방향성을 유지하면서
/// 적당한 간격으로 떨어져 주변을 걷는다.
/// </summary>
public class AvoidWall : Seek
{
    // 코드 몸체
    public float avoidDistance;
    public float lookAhead;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
    }
    // 에이전트 앞에서 레이를 발사
    // 레이가 벽에 출동했을 때 대상 객체의 위치를 벽으로부터의 거리와 함께 선언된 안전 거리를 고려해 결정
    // 키 값(steering)계산은 Seek 행위 클래스에 위임
    // 이것은 에이전트가 벽을 피하는 듯한 착각을 일으킨다.
    public override Steering GetSteering()
    {
        // 레이 캐스팅을 위한 변수들 선언
        Steering steering = new Steering();
        Vector3 position = transform.position;
        Vector3 rayVector = agent.velocity.normalized * lookAhead;
        Vector3 direction = rayVector;
        RaycastHit hit;

        // 레이를 발사하고 벽에 맞았는지 적합한 계산 수행
        if(Physics.Raycast(position,direction,out hit, lookAhead))
        {
            position = hit.point + hit.normal * avoidDistance;
            target.transform.position = position;
            steering = base.GetSteering();
        }
        return steering;
    }
    // 더 정밀한 정확도를 위해 더듬이처럼 레이를 추가해 행위 확장을 할 수 있다.
    // 벽 피하기는 일반적으로 보통 추적(Pursue)행위와 같은 다른 움직임 행위와 함께 섞어서(blend)사용.
}
