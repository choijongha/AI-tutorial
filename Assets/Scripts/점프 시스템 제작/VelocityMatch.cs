using System.Collections;
using UnityEngine;

/// <summary>
/// �ӵ� ������ ��������Ʈ�� ������ �� �ֵ��� �⺻ �ӵ� ��Ī �˰���(matching-velocity)�� ���� �е� �� ���� �е��� ������ ������ ��.
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
