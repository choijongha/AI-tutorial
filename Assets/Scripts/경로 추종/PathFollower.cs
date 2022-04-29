using System.Collections;
using UnityEngine;

// 2. ���� �������� ���� ��ġ�� ��� ���� �߻�ȭ�� ��θ� Ȱ���ϴ� PathFollower ���� Ŭ����.
// ���� ���� ���� ���� �� ��.
/// <summary>
/// PathŬ������ GetParam�� ������ ���� ��ħ�� ���� offset ����Ʈ�� �����ϸ�
/// GetPosition�� ����� �ش� �������� ���׸�Ʈ�� ���� 3���� �������� ��ġ�� ��ȯ�ϱ� ������ 
/// �����ӿ� ���� ���̵������ ����� ���� �ٰ��� �ȴ�.
/// </summary>
/// ��� ���� �˰����� ���ο� ��ġ�� ��ų� 
/// ����� ����update�ϰ� Ž��Seek������ �����ϱ� ���� ��� �Լ����� ����Ѵ�.
public class PathFollower : Seek
{
    public Path path;
    public float pathOffset = 0.0f;
    float currentParam;
    /// <summary>
    /// ����� �����ϴ� �ڵ� ����.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
        currentParam = 0f;
    }
    /// <summary>
    /// ��ǥ ��ġ�� �����ϰ� Seek Ŭ������ �����ϱ� ���ؼ� Path Ŭ������ �߻�ȭ�� �����Ѵ�.
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
