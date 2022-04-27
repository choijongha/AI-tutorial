using UnityEngine;

public class Leave : AgentBehavior
{
    public float escapeRadius;
    public float dangerRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 direction = transform.position - target.transform.position;
        float distance = direction.magnitude;
        if (distance > dangerRadius)
            return steering;
        float reduce;
        if (distance < dangerRadius)
            reduce = 0f;
        else
            reduce = distance / dangerRadius * agent.maxSpeed;
        float targetSpeed = agent.maxSpeed - reduce;

        // steering 값을 설정하고 최대 속도에 맞춰 값을 제한
        Vector3 desiredVelocity = direction;
        desiredVelocity.Normalize();
        desiredVelocity *= targetSpeed;
        steering.linear = desiredVelocity - agent.velocity;
        steering.linear /= timeToTarget;
        if (steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }
        return steering;
    }
}
