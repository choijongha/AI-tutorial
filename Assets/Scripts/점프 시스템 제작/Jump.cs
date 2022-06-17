using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 알고리즘은 에이전트의 속력을 고려해 에이전트가 착륙 패드에 도착할지 말지 결정하는 알고리즘
/// 에이전트가 착륙 패드에 도착할지 말지 결정하는 것이 가능하다고 판단하면
/// 착륙 패드의 위치를 탐색하는 동안 대상의 수직 속도를 맞추려고 시도한다.
/// </summary>
public class Jump : VelocityMatch
{
    public JumpPoint jumpPoint;
    public float maxYVelocity;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    private bool canArchieve = false;
    // 다음 단계

    public void SetJumpPoint(Transform jumpPad, Transform landingPad)
    {
        jumpPoint = new JumpPoint(jumpPad.position, landingPad.position);
    }

    // 대상을 계산하는 함수
    protected void CalculateTarget()
    {
        target = new GameObject();
        target.AddComponent<Agent>();
        target.transform.position = jumpPoint.jumpLocation;
        // 처음 점프 시간을 계산한다.
        float sqrtTerm = Mathf.Sqrt(2f * gravity.y * jumpPoint.deltaPosition.y + maxYVelocity * agent.maxSpeed);
        float time = (maxYVelocity + sqrtTerm) / gravity.y;
        // 사용할 수 있는지 확인하고, 사용하지 못한다면 다음 번에 시도한다.
        if(!CheckJumpTime(time))
        {
            time = (maxYVelocity + sqrtTerm) / gravity.y;
        }
    }
    private bool CheckJumpTime (float time)
    {
        // 평면 속도를 계산한다.
        float vx = jumpPoint.deltaPosition.x / time;
        float vz = jumpPoint.deltaPosition.z / time;
        float sppedSq = vx * vx + vz * vz;
        // 유효한 해가 존재하는지 확인한다.
        if (sppedSq < agent.maxSpeed * agent.maxSpeed)
        {
            target.GetComponent<Agent>().velocity = new Vector3(vx, 0f, vz);
            canArchieve = true;
            return true;
        }
        return false;
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        if (target == null)
        {
            CalculateTarget();
        }
        if (!canArchieve)
        {
            return steering;
        }
        // 점프 포인트에 도달했는지 확인한다.
        if (Mathf.Approximately ((transform.position - target.transform.position).magnitude, 0f) && Mathf.Approximately((agent.velocity - target.GetComponent<Agent>().velocity).magnitude, 0f))
        {
            // Projectile 행동 기반으로 점프 메서드를 호출한다.
            return steering;
        }
        return base.GetSteering();
    }
}
