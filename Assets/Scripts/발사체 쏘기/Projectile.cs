using System.Collections;
using UnityEngine;

// 공이나 수류탄 같은 중력에 영향을 받는 물체들을 다루는 시나리오의 시금석이 될 것.
// 발사체의 착륙 지점을 예측하고, 주어진 목표로 발사체를 효과적으로 발사.
// 포물선 움직임을 만들기 위해 고등학교 수준이 물리 지식을 사용
// 다른 접근 방법, Set 함수를 호출하는 대신 스크립트를 공개 프로퍼티들로 구현
// 또는 멤버 변수들을 public으로 선언하고, 프리펩의 기본값 비활성화를 한 상태에서 모든 프로퍼티들이 설정된 이후에 활성화되도록.
// 이 방법으로 쉽게 객체 풀 패턴(object pool pattern)을 적용할 수 있다.
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
        // 현장 정리를 위한 추가 검증
        if (transform.position.y < -1.0f)
            Destroy(this.gameObject); // 또는 set = false 로 숨김.
    }
    /// <summary>
    /// 게임 오브젝트를 발사하기 위한 Set함수.
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
    /// 착륙 지점을 예측하기에 앞서 발사체가 땅에 부딪히기까지 (혹은 특징 지점에 도달하기까지) 남은 시간을 아는 것이 중요하다.
    /// 착륙 시간 계산 함수.
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
        // 방정식의 해가 하나이거나 둘 혹은 없을 수도 있기 때문에 값이 NaN인지 반드시 검증.
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
    /// 착륙 지점을 예측하는 함수.
    /// 고정된 높이와 주어진 발사체의 현재 위치와 속력으로 이전 예제로부터 만든 방정식을 풀고,
    /// 주어진 높이에 도달하는 데 걸릴 시간을 구할 수 있다.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public Vector3 GetLandingPos(float height = 0.0f)
    {
        Vector3 landingPos = Vector3.zero;
        float time = GetLandingTime();
        // 착륙까지 0보다 작은 시간이 남아 있을 경우, 발사체가 목표 높이에 도달할 수 없음.
        if (time < 0.0f)
            return landingPos;
        landingPos.y = height;
        landingPos.x = firePos.x + direction.x * speed * time;
        landingPos.z = firePos.z + direction.z * speed * time;
        return landingPos;
    }
}
