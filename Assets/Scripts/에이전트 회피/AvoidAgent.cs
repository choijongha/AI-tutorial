using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 군중시물레이션 게임에서 물리 기반 시스템에서의 입자처럼 해옹하는 에이전트들은 부자연스럽다.
// 목표는 에이전트들끼리 상호 충돌이 일어나지 않는(peer-evasion)움직임을 모방하는 에이전트를 만드는 것.
/// <summary>
/// 충돌 회피 반경과 회피할 에이전트의 목록으로 구성된 클래스 제작.
/// 이 행위는 2장에 일부 포함돼 있는 혼합(blending)기술을 사용해 다른 행위와  함께 사용할 때 잘 동작.
/// 혼합기술을 사용하지 않는다면 본인만의 충돌 회피 알고리즘을 만드는 좋은 시작점이 될 수도 있다.
/// </summary>
public class AvoidAgent : AgentBehavior
{
    // 충돌 회피 반경과 회피할 에이전트의 목록으로 구성된 
    public float collisionRadius = 0.4f;
    private GameObject[] targets;
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Agent");
    }
    // 리스트에서 가장 가까운 에이전트를 고려했다.
    // 그리고 충분히 가까워졌을 때, 가장 가까운 에이전트의 속도 값을 기준으로 기존 경로를 수정해 충돌하지 않게 한다.
    public override Steering GetSteering()
    {
        // 코드 몸체
        // 근처에 있는 에이전트들의 속도와 거리를 계산하기 위해서 다음의 변수들을 추가.
        Steering steering = new Steering();
        float shortestTime = Mathf.Infinity;
        GameObject firstTarget = null;
        float firstMinSeparation = 0.0f;
        float firstDistance = 0.0f;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        // 현재 충돌할 확률이 매우 높은 가장 가까운 에이전트를 찾는다.
        foreach (GameObject t in targets)
        {
            Vector3 relativePos;
            Agent targetAgent = t.GetComponent<Agent>();
            relativePos = t.transform.position - transform.position;
            Vector3 relativeVel = targetAgent.velocity - agent.velocity;
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = Vector3.Dot(relativePos, relativeVel);
            timeToCollision /= relativeSpeed * relativeSpeed * -1;
            float distance = relativePos.magnitude;
            float minSepatation = distance - relativeSpeed * timeToCollision;
            if (minSepatation > 2 * collisionRadius)
                continue;
            if(timeToCollision > 0.0f && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = t;
                firstMinSeparation = minSepatation;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
        if (firstTarget == null)
            return steering;
        if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius)
            firstRelativePos = firstTarget.transform.position;
        else
            firstRelativePos += firstRelativeVel * shortestTime;
        firstRelativePos.Normalize();
        steering.linear = -firstRelativePos * agent.maxAccel;
        return steering;
    }
}
