using System.Collections;
using UnityEngine;

// facing 마주보기 알고리즘의 기초
// 회전을 다룬다는 점을 제외하고 Arrive 클래스와 같은 원리.
public class Align : AgentBehavior
{
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        float targetOrientation = target.GetComponent<Agent>().orientation;
        float rotation = targetOrientation - agent.orientation;
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);
        if (rotationSize < targetRadius)
            return steering;
        float targetRotation;
        if (rotationSize > slowRadius)
            targetRotation = maxRotation;
        else
            targetRotation = maxRotation * rotationSize / slowRadius;
        targetRotation *= rotation / rotationSize;
        steering.angular = targetRotation - agent.rotation;
        steering.angular /= timeToTarget;
        float angularAccel = Mathf.Abs(steering.angular);

        if(angularAccel > maxAngularAccel)
        {
            steering.angular /= angularAccel;
            steering.angular *= maxAngularAccel;
        }
        return steering;
    }
}
