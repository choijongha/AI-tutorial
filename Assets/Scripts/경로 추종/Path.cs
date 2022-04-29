using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 특정 공간상 표현으로부터 경로의 포인트들을 추상화하는 Path 클래스.
/// <summary>
/// 노드들과 세그먼트들로 구성된 Path클래스
/// 노드 리스트 멤버 변수는 공개형으로 선언해 직접 할당 할 수 있게 한다.
/// </summary>
public class Path : MonoBehaviour
{
    public List<GameObject> nodes;
    public List<PathSegment> segments;
    // 세그먼트를 설정해줌
    private void Start()
    {
        segments = GetSegments();
    }
    /// <summary>
    /// 노드로부터 세그먼트를 만드는 함수.
    /// </summary>
    /// <returns>세그먼트</returns>
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
    // 추상화를 위한 첫 번째 함수
    public float GetParam(Vector3 position, float lastParam)
    {
        //코드 몸체
        // 어떤 세그먼트가 에이전트에 가장 가까운지 찾는다.
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
        // 주어진 현재 위치를 통해 어느 방향으로 가야할지 결정.
        Vector3 currPos = position - currentSegment.a;
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        // 벡터 투영(projection)을 통해서 세그먼트 내의 포인트를 찾는다.
        Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);
        // GetParam 함수에서 경로 중 다음 위치를 반환한다.
        param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
        param += pointInSegment.magnitude;
        return param;
    }
    public Vector3 GetPosition(float param)
    {
        // 코드 몸체
        // 경로 사이에 존재하는 주어진 현재 위치를 통해, 상응하는 세그먼트를 찾는다.
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
        // GetPosition 함수에서 파라미터를 공간상의 위치로 변환해준 후 반환.
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
        tempParam = param - tempParam;
        position = currentSegment.a + segmentDirection * tempParam;
        return position;
    }
    // 경로를 시각적으로 더 조기 좋게 하기 위함.
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
