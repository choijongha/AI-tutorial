using System.Collections;
using UnityEngine;

public class JumpPoint
{
    public Vector3 jumpLocation;
    public Vector3 landingLocation;
    // ������������ ���������� ��ġ ��ȭ
    public Vector3 deltaPosition;

    public JumpPoint()
        : this(Vector3.zero, Vector3.zero)
    {

    }
    public JumpPoint(Vector3 a, Vector3 b)
    {
        this.jumpLocation = a;
        this.landingLocation = b;
        this.deltaPosition = this.landingLocation - this.jumpLocation;
    }
}
