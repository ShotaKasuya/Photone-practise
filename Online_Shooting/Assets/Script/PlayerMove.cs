using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    public int PlayerNumber;
    const int MAX_HP = 10;
    public int HP;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    public PhotonLogin login;

    #region Shot
    [SerializeField] private Transform shot_Point;
    [SerializeField] private GameObject nomal_bullet;
    [SerializeField] private GameObject lightning_bullet;
    [SerializeField] private float interval_LT;
    float interval_LC;
    [SerializeField] private float interval_NT;
    float interval_NC;


    #endregion

    #region Input
    [SerializeField] private PlayerInput playerInput;
    InputAction attack1,attack2,move,rotation;
    private Vector2 moveInput;
    float _atk1, _atk2;
    float _rotation;

    #endregion
    void Awake()
    {
        HP = MAX_HP;
        input_format();
    }
    void FixedUpdate()
    {
        getInput();
        player_action();

        if (interval_LC < interval_LT)
            interval_LC += Time.deltaTime;
        if (interval_NC < interval_NT)
            interval_NC += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.tag == "Bullet") {
            var dam = collider2D.GetComponent<BulletShimple>();

            HP -= dam.getPower();
            if (HP <= 0) {
                login.game_set(PlayerNumber);
            }

            Destroy(collider2D.gameObject);
        }
    }

    void input_format() {
        attack1 = playerInput.actions["Attack1"];
        attack2 = playerInput.actions["Attack2"];
        // attack3 = playerInput.actions["attack3"];
        move = playerInput.actions["Move"];
        rotation = playerInput.actions["Rotation"];
    }

    void getInput() {
        _atk1 = attack1.ReadValue<float>();
        _atk2 = attack2.ReadValue<float>();
        moveInput = move.ReadValue<Vector2>();
        _rotation = rotation.ReadValue<float>();
    }

    void player_action() {
        rb.velocity = moveInput * moveSpeed;
        transform.Rotate(0f,0f, -_rotation * 0.5f);

        if (_atk1 > 0 && interval_LC > interval_LT) {
            interval_LC = 0;
            shot(nomal_bullet);
        }
        if (_atk2 > 0 && interval_NC > interval_NT) {
            interval_NC = 0;
            shot(lightning_bullet);
        }
    }

    void shot(GameObject Bullet) {
        var obj = PhotonNetwork.Instantiate(Bullet.name, shot_Point.position, Quaternion.identity);

        var script = obj.GetComponent<BulletShimple>();

        script.SetValues(shot_Point.position, this.transform.position);
    }
}
