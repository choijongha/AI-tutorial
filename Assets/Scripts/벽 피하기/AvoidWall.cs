using System.Collections;
using UnityEngine;

/// <summary>
/// �տ� ���̴� ���� ���̳� ��ֹ��� �����ϰ� ���ÿ� ���� ���ϰ��� �ߴ� ���⼺�� �����ϸ鼭
/// ������ �������� ������ �ֺ��� �ȴ´�.
/// </summary>
public class AvoidWall : Seek
{
    // �ڵ� ��ü
    public float avoidDistance;
    public float lookAhead;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
    }
    // ������Ʈ �տ��� ���̸� �߻�
    // ���̰� ���� �⵿���� �� ��� ��ü�� ��ġ�� �����κ����� �Ÿ��� �Բ� ����� ���� �Ÿ��� ����� ����
    // Ű ��(steering)����� Seek ���� Ŭ������ ����
    // �̰��� ������Ʈ�� ���� ���ϴ� ���� ������ ����Ų��.
    public override Steering GetSteering()
    {
        // ���� ĳ������ ���� ������ ����
        Steering steering = new Steering();
        Vector3 position = transform.position;
        Vector3 rayVector = agent.velocity.normalized * lookAhead;
        Vector3 direction = rayVector;
        RaycastHit hit;

        // ���̸� �߻��ϰ� ���� �¾Ҵ��� ������ ��� ����
        if(Physics.Raycast(position,direction,out hit, lookAhead))
        {
            position = hit.point + hit.normal * avoidDistance;
            target.transform.position = position;
            steering = base.GetSteering();
        }
        return steering;
    }
    // �� ������ ��Ȯ���� ���� ������ó�� ���̸� �߰��� ���� Ȯ���� �� �� �ִ�.
    // �� ���ϱ�� �Ϲ������� ���� ����(Pursue)������ ���� �ٸ� ������ ������ �Բ� ���(blend)���.
}
