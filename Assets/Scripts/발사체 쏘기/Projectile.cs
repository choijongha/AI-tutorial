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
    /// <summary>
    /// 필요한 방향을 얻기 위해 고정된 속도가 주어지면 해당하는 이차 방정식을 푼다.(적어도 하나의 시간 값은 유효해야 한다.)
    /// 이 방향 벡터는 정규화할 필요가 없는데, 발사체를 설정하는 동안 이미 정규화했기 때문.
    /// </summary>
    /// <param name="startPos">시작위치</param>
    /// <param name="endPos">끝위치</param>
    /// <param name="speed">속도</param>
    /// <param name="isWall">벽과 같은 장애물을 넘어 발사해야 할 때</param>
    /// <returns></returns>
    public static Vector3 GetFireDirection(Vector3 startPos, Vector3 endPos, float speed, bool isWall)
    {
        // 발사체 착륙 지점 관련 이차 방정식의 해를 구한다.
        Vector3 direction = Vector3.zero;
        Vector3 delta = endPos - startPos;
        float a = Vector3.Dot(Physics.gravity, Physics.gravity);
        float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
        float c = 4 * Vector3.Dot(delta, delta);
        if (4 * a * c > b * b)
            return direction;
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));

        // 주어진 파라미터를 토대로 발사체를 발사할 수 있으면, 영이 안닌 방향 벡터를 반환.
        float time;
        if (time0 < 0.0f)
        {
            if (time1 < 0)
                return direction;
            time = time1;
        }
        else
        {
            if (time1 < 0)
                time = time0;
            else
                time = Mathf.Min(time0, time1);
        }
        // 시간 값이 음수일 때, 빈 방향(blank direction)을 반환하는 것을 고려하라. 이것은 속도가 충분하지 않다는 것을 의미.
        // 극복하는 방법은 다른 속도들을 테스트하는 함수를 정의하고, 발사체를 발사하는 것.
        // 또 다른 향상된 방법은 두 가지 유효한 시간이 존재할 때(이 의미는 두 종류의 포물선이 가능하다는 것)
        // 그리고 벽과 같은 장애물을 넘어 발사해야 할 때에는 bool 파라미터를 추가한다.
        if (isWall)
            time = Mathf.Max(time0, time1);
        else
            time = Mathf.Min(time0, time1);

        direction = 2 * delta - Physics.gravity * (time * time);
        direction = direction / (2 * speed * time);
        return direction;
    }
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
