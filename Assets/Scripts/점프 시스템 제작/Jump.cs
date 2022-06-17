using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �˰����� ������Ʈ�� �ӷ��� ����� ������Ʈ�� ���� �е忡 �������� ���� �����ϴ� �˰���
/// ������Ʈ�� ���� �е忡 �������� ���� �����ϴ� ���� �����ϴٰ� �Ǵ��ϸ�
/// ���� �е��� ��ġ�� Ž���ϴ� ���� ����� ���� �ӵ��� ���߷��� �õ��Ѵ�.
/// </summary>
public class Jump : VelocityMatch
{
    public JumpPoint jumpPoint;
    public float maxYVelocity;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    private bool canArchieve = false;
    // ���� �ܰ�

    public void SetJumpPoint(Transform jumpPad, Transform landingPad)
    {
        jumpPoint = new JumpPoint(jumpPad.position, landingPad.position);
    }

    // ����� ����ϴ� �Լ�
    protected void CalculateTarget()
    {
        target = new GameObject();
        target.AddComponent<Agent>();
        target.transform.position = jumpPoint.jumpLocation;
        // ó�� ���� �ð��� ����Ѵ�.
        float sqrtTerm = Mathf.Sqrt(2f * gravity.y * jumpPoint.deltaPosition.y + maxYVelocity * agent.maxSpeed);
        float time = (maxYVelocity + sqrtTerm) / gravity.y;
        // ����� �� �ִ��� Ȯ���ϰ�, ������� ���Ѵٸ� ���� ���� �õ��Ѵ�.
        if(!CheckJumpTime(time))
        {
            time = (maxYVelocity + sqrtTerm) / gravity.y;
        }
    }
    private bool CheckJumpTime (float time)
    {
        // ��� �ӵ��� ����Ѵ�.
        float vx = jumpPoint.deltaPosition.x / time;
        float vz = jumpPoint.deltaPosition.z / time;
        float sppedSq = vx * vx + vz * vz;
        // ��ȿ�� �ذ� �����ϴ��� Ȯ���Ѵ�.
        if (sppedSq < agent.maxSpeed * agent.maxSpeed)
        {
            target.GetComponent<Agent>().velocity = new Vector3(vx, 0f, vz);
            canArchieve = true;
            return true;
        }
        return false;
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        if (target == null)
        {
            CalculateTarget();
        }
        if (!canArchieve)
        {
            return steering;
        }
        // ���� ����Ʈ�� �����ߴ��� Ȯ���Ѵ�.
        if (Mathf.Approximately ((transform.position - target.transform.position).magnitude, 0f) && Mathf.Approximately((agent.velocity - target.GetComponent<Agent>().velocity).magnitude, 0f))
        {
            // Projectile �ൿ ������� ���� �޼��带 ȣ���Ѵ�.
            return steering;
        }
        return base.GetSteering();
    }
}
