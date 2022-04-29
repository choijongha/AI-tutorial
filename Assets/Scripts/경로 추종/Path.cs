using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Ư�� ������ ǥ�����κ��� ����� ����Ʈ���� �߻�ȭ�ϴ� Path Ŭ����.
/// <summary>
/// ����� ���׸�Ʈ��� ������ PathŬ����
/// ��� ����Ʈ ��� ������ ���������� ������ ���� �Ҵ� �� �� �ְ� �Ѵ�.
/// </summary>
public class Path : MonoBehaviour
{
    public List<GameObject> nodes;
    public List<PathSegment> segments;
    // ���׸�Ʈ�� ��������
    private void Start()
    {
        segments = GetSegments();
    }
    /// <summary>
    /// ���κ��� ���׸�Ʈ�� ����� �Լ�.
    /// </summary>
    /// <returns>���׸�Ʈ</returns>
    public List<PathSegment> GetSegments()
    {
        List<PathSegment> segments = new List<PathSegment>();
        int i;
        for (i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            PathSegment segment = new PathSegment(src, dst);
            segments.Add(segment);
        }
        return segments;
    }
    // �߻�ȭ�� ���� ù ��° �Լ�
    public float GetParam(Vector3 position, float lastParam)
    {
        //�ڵ� ��ü
        // � ���׸�Ʈ�� ������Ʈ�� ���� ������� ã�´�.
        float param = 0f;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach(PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if(lastParam <= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }
        if (currentSegment == null)
            return 0f;
        // �־��� ���� ��ġ�� ���� ��� �������� �������� ����.
        Vector3 currPos = position - currentSegment.a;
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        // ���� ����(projection)�� ���ؼ� ���׸�Ʈ ���� ����Ʈ�� ã�´�.
        Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);
        // GetParam �Լ����� ��� �� ���� ��ġ�� ��ȯ�Ѵ�.
        param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
        param += pointInSegment.magnitude;
        return param;
    }
    public Vector3 GetPosition(float param)
    {
        // �ڵ� ��ü
        // ��� ���̿� �����ϴ� �־��� ���� ��ġ�� ����, �����ϴ� ���׸�Ʈ�� ã�´�.
        Vector3 position = Vector3.zero;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach (PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if(param<= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }
        if (currentSegment == null)
            return Vector3.zero;
        // GetPosition �Լ����� �Ķ���͸� �������� ��ġ�� ��ȯ���� �� ��ȯ.
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
        tempParam = param - tempParam;
        position = currentSegment.a + segmentDirection * tempParam;
        return position;
    }
    // ��θ� �ð������� �� ���� ���� �ϱ� ����.
    private void OnDrawGizmos()
    {
        Vector3 direction;
        Color tmp = Gizmos.color;
        Gizmos.color = Color.magenta; //example color
        int i;
        for (i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            direction = dst - src;
            Gizmos.DrawRay(src, direction);
        }
        Gizmos.color = tmp;
    }

}
