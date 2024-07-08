using UnityEngine;

public class MovingBG : MonoBehaviour
{
    [SerializeField] GameObject[] BGs;
    [SerializeField] float speed = 1f;

    [SerializeField] float bgSize = 20.48f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bg in BGs)
        {
            bg.transform.position += Vector3.left * speed * Time.deltaTime;

            if (bg.transform.position.x < -bgSize)
            {
                bg.transform.position += new Vector3(2 * bgSize, 0,0);
            }
        }
    }
}
