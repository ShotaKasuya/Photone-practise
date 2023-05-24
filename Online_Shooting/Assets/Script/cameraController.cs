using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    Vector3 diff = Vector3.zero;
    float timecount = 0f;
    [SerializeField] private Transform CameraPos;

    void FixedUpdate() {
        if (timecount <= 5f)
            timecount += Time.deltaTime;
        else if (diff == Vector3.zero)
            diff = CameraPos.position - this.transform.position;
        else
            transform.position = Vector3.Lerp(transform.position, CameraPos.position - diff, Time.deltaTime * 5.0f);
    }

    public void set_player(Transform player) {
        this.CameraPos = player;
    }
}
