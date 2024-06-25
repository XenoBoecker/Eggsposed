using UnityEngine;

public class TestMovement : MonoBehaviour
{
    PlayerControls controls;

    [SerializeField] float movSpeed = 5;
        [SerializeField] float rotSpeed = 45;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        Move(controls.Player.Move.ReadValue<Vector2>());
    }

    private void Move(Vector2 dir)
    {
        print("Dir: " + dir);

        transform.Translate(dir.y * transform.forward * movSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, dir.x * rotSpeed * Time.deltaTime);
    }
}