using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���߽ù����̼� ���ӿ��� ���� ��� �ý��ۿ����� ����ó�� �ؿ��ϴ� ������Ʈ���� ���ڿ�������.
// ��ǥ�� ������Ʈ�鳢�� ��ȣ �浹�� �Ͼ�� �ʴ�(peer-evasion)�������� ����ϴ� ������Ʈ�� ����� ��.
/// <summary>
/// �浹 ȸ�� �ݰ�� ȸ���� ������Ʈ�� ������� ������ Ŭ���� ����.
/// �� ������ 2�忡 �Ϻ� ���Ե� �ִ� ȥ��(blending)����� ����� �ٸ� ������  �Բ� ����� �� �� ����.
/// ȥ�ձ���� ������� �ʴ´ٸ� ���θ��� �浹 ȸ�� �˰����� ����� ���� �������� �� ���� �ִ�.
/// </summary>
public class AvoidAgent : AgentBehavior
{
    // �浹 ȸ�� �ݰ�� ȸ���� ������Ʈ�� ������� ������ 
    public float collisionRadius = 0.4f;
    private GameObject[] targets;
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Agent");
    }
    // ����Ʈ���� ���� ����� ������Ʈ�� ����ߴ�.
    // �׸��� ����� ��������� ��, ���� ����� ������Ʈ�� �ӵ� ���� �������� ���� ��θ� ������ �浹���� �ʰ� �Ѵ�.
    public override Steering GetSteering()
    {
        // �ڵ� ��ü
        // ��ó�� �ִ� ������Ʈ���� �ӵ��� �Ÿ��� ����ϱ� ���ؼ� ������ �������� �߰�.
        Steering steering = new Steering();
        float shortestTime = Mathf.Infinity;
        GameObject firstTarget = null;
        float firstMinSeparation = 0.0f;
        float firstDistance = 0.0f;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        // ���� �浹�� Ȯ���� �ſ� ���� ���� ����� ������Ʈ�� ã�´�.
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
