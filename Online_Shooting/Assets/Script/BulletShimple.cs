using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShimple : MonoBehaviour
{
    Vector2 cursorPos, bodyPos, move_vector;
    [SerializeField] private Rigidbody2D rbody2d;
    [SerializeField] private float speed;
    [SerializeField] private int power;
    [SerializeField] private int bounse;

    void Start() {
        rbody2d.velocity = move_vector * speed;
    }

    public void SetValues(Vector2 cursor, Vector2 body) {
        this.cursorPos = cursor;
        this.bodyPos = body;

        this.move_vector = calculation();
    }

    Vector2 calculation() {
        return new Vector2(cursorPos.x - bodyPos.x, cursorPos.y - bodyPos.y);
    }

    public int getPower() {
        return power;
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {
        Debug.Log("Trriger Enter");
        bounse--;
        if (bounse <= 0)
            Destroy(this.gameObject);
        if (collider2D.tag == "sidewall")
            rbody2d.velocity = new Vector2(-rbody2d.velocity.x, rbody2d.velocity.y);
        if (collider2D.tag == "FBwall")
            rbody2d.velocity = new Vector2(rbody2d.velocity.x, -rbody2d.velocity.y);
    }
}
