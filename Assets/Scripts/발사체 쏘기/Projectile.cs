using System.Collections;
using UnityEngine;

// ���̳� ����ź ���� �߷¿� ������ �޴� ��ü���� �ٷ�� �ó������� �ñݼ��� �� ��.
// �߻�ü�� ���� ������ �����ϰ�, �־��� ��ǥ�� �߻�ü�� ȿ�������� �߻�.
// ������ �������� ����� ���� ����б� ������ ���� ������ ���
// �ٸ� ���� ���, Set �Լ��� ȣ���ϴ� ��� ��ũ��Ʈ�� ���� ������Ƽ��� ����
// �Ǵ� ��� �������� public���� �����ϰ�, �������� �⺻�� ��Ȱ��ȭ�� �� ���¿��� ��� ������Ƽ���� ������ ���Ŀ� Ȱ��ȭ�ǵ���.
// �� ������� ���� ��ü Ǯ ����(object pool pattern)�� ������ �� �ִ�.
// https://learn.unity.com/search/?k=%5B%22q%3Apooling%22%5D
public class Projectile : MonoBehaviour
{
    private bool set = false;
    private Vector3 firePos;
    private Vector3 direction;
    private float speed;
    private float timeElapsed;
    private void Update()
    {
        if (!set)
            return;
        timeElapsed += Time.deltaTime;
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;
        // ���� ������ ���� �߰� ����
        if (transform.position.y < -1.0f)
            Destroy(this.gameObject); // �Ǵ� set = false �� ����.
    }
    /// <summary>
    /// ���� ������Ʈ�� �߻��ϱ� ���� Set�Լ�.
    /// </summary>
    /// <param name="firePos"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
   public  void Set (Vector3 firePos, Vector3 direction, float speed)
    {
        this.firePos = firePos;
        this.direction = direction.normalized;
        this.speed = speed;
        transform.position = firePos;
        set = true;
    }
}
