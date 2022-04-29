using System.Collections;
using UnityEngine;

// 2. 따라갈 공간상의 실제 위치를 얻기 위해 추상화된 경로를 활용하는 PathFollower 행위 클래스.
// 실행 순서 설정 잊지 말 것.
/// <summary>
/// Path클래스는 GetParam에 의존해 내부 지침에 따라 offset 포인트를 매핑하며
/// GetPosition을 사용해 해당 참조점을 세그먼트를 따라 3차원 공간상의 위치로 변환하기 때문에 
/// 움직임에 대한 가이드라인을 만들기 위한 근간이 된다.
/// </summary>
/// 경로 추종 알고리즘은 새로운 위치를 얻거나 
/// 대상을 갱신update하고 탐색Seek행위를 적용하기 위해 경로 함수들을 사용한다.
public class PathFollower : Seek
{
    public Path path;
    public float pathOffset = 0.0f;
    float currentParam;
    /// <summary>
    /// 대상을 지정하는 코드 구현.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
        currentParam = 0f;
    }
    /// <summary>
    /// 목표 위치를 지정하고 Seek 클래스를 적용하기 위해서 Path 클래스의 추상화에 의존한다.
    /// </summary>
    /// <returns></returns>
    public override Steering GetSteering()
    {
        currentParam = path.GetParam(transform.position, currentParam);
        float targetParam = currentParam + pathOffset;
        target.transform.position = path.GetPosition(targetParam);
        return base.GetSteering();
    }
}
