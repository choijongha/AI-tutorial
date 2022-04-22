using UnityEngine;

public class Leave : AgentBehavior
{
    public float escapeRadius;
    public float dangerRadius;
    public float timeToTager = 0.1f;

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
        return base.GetSteering();
    }
}
