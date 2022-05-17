using UnityEngine;

// 대부분의 행위에 대한 템플릿 클래스
public class AgentBehavior : MonoBehaviour
{
    public GameObject target;
    protected Agent agent;

    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;

    // 섞기 기술을 활용해 새로운 종류의 하이브리드(hybrid)에이전트가 필요할 때마다
    // 새로운 스크립트 작성 없이 새로운 행위들을 추가하거나 섞을 수 있다.
    // 가중치를 통한 섞기 기술은 1장에서 가장 유용한 기술 중 하나.
    // 강력함과 구현에 들어가는 적은 노력 때문에 아마도 가장 널리 쓰이는 행위 섞기 접근 방식.
    public float weight = 1.0f;
    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
    }
    public virtual void Update()
    {
        // 가중치(weight)값들은 steering 행위 결괏값을 증폭시키는 데 사용되고,
        // 주요 키값(steering)구조에 더해진다.
        agent.SetSteering(GetSteering(), weight);
    }
    public  virtual Steering GetSteering()
    {
        return new Steering();
    }
    /// <summary>
    /// 두 방향 값을 뺀 후 실제 회전 방향을 찾는다.
    /// </summary>
    /// <param name="rotation">회전 방향</param>
    /// <returns>실제 회전 방향</returns>
    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }
    /// <summary>
    /// 방향 값을 벡터로 변경해주는 함수
    /// </summary>
    /// <param name="orientation">방향 값</param>
    /// <returns>방향 값 벡터</returns>
    public Vector3 GetOriAsVec (float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
