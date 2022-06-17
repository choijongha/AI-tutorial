using System.Collections;
using UnityEngine;

/// <summary>
/// 속도 수학을 에물레이트해 도달할 수 있도록 기본 속도 매칭 알고리즘(matching-velocity)과 점프 패드 및 착륙 패드의 개념을 만들어야 함.
/// </summary>
public class VelocityMatch : AgentBehavior
{
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        steering.linear = target.GetComponent<Agent>().velocity - agent.velocity;
        steering.linear /= timeToTarget;
        if (steering.linear.magnitude > agent.maxAccel)
            steering.linear = steering.linear.normalized * agent.maxAccel;
        steering.angular = 0.0f;
        return steering;
    }
}
