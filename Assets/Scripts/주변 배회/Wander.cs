using System.Collections;
using UnityEngine;

// 다음에 가야 할 무작위 위치를 얻기 위해 두 가지 반지름을 고려했다.
// 전진하기 위해 무작위 위치를 바라본 이후 계산된 방향을 벡터 값으로 변환한다.
public class Wander : Face
{
    public float offset;
    public float radius;
    public float rate;
    public override void Awake()
    {
        target = new GameObject();
        target.transform.position = transform.position;
        base.Awake();
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
        float targetOrientation = wanderOrientation + agent.orientation;
        Vector3 orientationVec = GetOriAsVec(agent.orientation);
        Vector3 targetPosition = (offset * orientationVec) + transform.position;
        targetPosition += (GetOriAsVec(targetOrientation) * radius);
        targetAux.transform.position = targetPosition;
        steering = base.GetSteering();
        steering.linear = targetAux.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;
        return steering;
    }
}
