using UnityEngine;

public class Arrive : AgentBehavior
{
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        // 두 반경 변수들에 따라 변경되는 대상과의 거리에 영향을 받아 적합한 속도를 계산.
        Steering steering = new Steering();
        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;
        if (distance < targetRadius)
            return steering;
        if (distance > targetRadius)
            targetSpeed = agent.maxSpeed;
        else
            targetSpeed = agent.maxSpeed * distance / slowRadius;

        // steering 값을 설정하고 최대 속도에 맞춰 값을 제한
        Vector3 desiredVelocity = direction;
        desiredVelocity.Normalize();
        desiredVelocity *= targetSpeed;
        steering.linear = desiredVelocity - agent.velocity;
        steering.linear /= timeToTarget;
        if(steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }
        return steering;
    }
}
