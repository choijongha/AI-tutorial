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
}
