using System.Collections;
using UnityEngine;

// Flee에 의존하며, 대상의 속도를 고려.
// 다음에 어디로 갈지 예측하기 위해 내부 여분 개체를 사용해 목표 지점으로 향함.
public class Evade : Flee
{
    public float maxPrediction;
    private GameObject targetAux;
    private Agent targetAgent;

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject();
    }
    // 내부 객체를 적절하게 다루기 위해
    private void OnDestroy()
    {
        Destroy(targetAux);
    }
    public override Steering GetSteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position;
        float distance = direction.magnitude;
        float speed = agent.velocity.magnitude;
        float prediction;
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;
        target.transform.position = targetAux.transform.position;
        target.transform.position += targetAgent.velocity * prediction;
        return base.GetSteering();
    }
}
