using UnityEngine;

public class Waypoints : MonoBehaviour
{

    public static Transform[] points;

    void Awake() {
        // 배열 사이즈 지정
        points = new Transform[transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            // 자식들 transform 을 가져온다
            points[i] = transform.GetChild(i);
        }
    }

}
