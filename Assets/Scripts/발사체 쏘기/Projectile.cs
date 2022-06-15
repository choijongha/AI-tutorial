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

    /// <summary>
    /// ���� ������ �����ϱ⿡ �ռ� �߻�ü�� ���� �ε�������� (Ȥ�� Ư¡ ������ �����ϱ����) ���� �ð��� �ƴ� ���� �߿��ϴ�.
    /// ���� �ð� ��� �Լ�.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public float GetLandingTime(float height = 0.0f)
    {
        Vector3 position = transform.position;
        float time = 0.0f;
        float valueInt = (direction.y * direction.y) * (speed * speed);
        valueInt = valueInt - (Physics.gravity.y * 2 * (position.y - height));
        valueInt = Mathf.Sqrt(valueInt);
        float valueAdd = (-direction.y) * speed;
        float valueSub = (-direction.y) * speed;
        valueAdd = (valueAdd + valueInt) / Physics.gravity.y;
        valueSub = (valueSub - valueInt) / Physics.gravity.y;
        // �������� �ذ� �ϳ��̰ų� �� Ȥ�� ���� ���� �ֱ� ������ ���� NaN���� �ݵ�� ����.
        if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub))
            return valueSub;
        else if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub))
            return valueAdd;
        else if (float.IsNaN(valueAdd) && float.IsNaN(valueSub))
            return -1.0f;
        time = Mathf.Max(valueAdd, valueSub);
        return time;
    }
    /// <summary>
    /// ���� ������ �����ϴ� �Լ�.
    /// ������ ���̿� �־��� �߻�ü�� ���� ��ġ�� �ӷ����� ���� �����κ��� ���� �������� Ǯ��,
    /// �־��� ���̿� �����ϴ� �� �ɸ� �ð��� ���� �� �ִ�.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public Vector3 GetLandingPos(float height = 0.0f)
    {
        Vector3 landingPos = Vector3.zero;
        float time = GetLandingTime();
        // �������� 0���� ���� �ð��� ���� ���� ���, �߻�ü�� ��ǥ ���̿� ������ �� ����.
        if (time < 0.0f)
            return landingPos;
        landingPos.y = height;
        landingPos.x = firePos.x + direction.x * speed * time;
        landingPos.z = firePos.z + direction.z * speed * time;
        return landingPos;
    }
}
