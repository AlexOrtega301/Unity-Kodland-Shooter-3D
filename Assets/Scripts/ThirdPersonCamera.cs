using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour 
{        
    [SerializeField] GameObject player;
    [SerializeField][Range(0.5f, 2f)]
    float mouseSense = 1; 
    [SerializeField][Range(-20, -10)]
     int lookUp = -15;
    [SerializeField][Range(15, 25)]
    int lookDown = 20;
    Animator anim;
    public bool isSpectator;
    [SerializeField] float speed = 50f;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    void Update()
    {
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        if (!isSpectator)
        {
            Vector3 rotCamera = transform.rotation.eulerAngles;
            Vector3 rotPlayer = player.transform.rotation.eulerAngles;
            rotCamera.x = (rotCamera.x > 180) ? rotCamera.x - 360 : rotCamera.x;
            rotCamera.x = Mathf.Clamp(rotCamera.x, -15, 30);
            rotCamera.x -= rotateY;
            rotCamera.z = 0;
            rotPlayer.y += rotateX;
            transform.rotation = Quaternion.Euler(rotCamera);
            player.transform.rotation = Quaternion.Euler(rotPlayer);
        }
        else
        {
            Vector3 rotCamera = transform.rotation.eulerAngles;
            rotCamera.x -= rotateY;
            rotCamera.z = 0;
            rotCamera.y += rotateX;
            transform.rotation = Quaternion.Euler(rotCamera);
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            //float y = 0;
            //if (Input.GetKey(KeyCode.E))
            //{
            //    y = 1;
            //}
            //else if (Input.GetKey(KeyCode.Q))
            //{
            //    y = -1;
            //}
            Vector3 dir = transform.right * x /*+ transform.up * y */+ transform.forward * z;
            transform.position += dir * speed * Time.deltaTime;
        }

        // Si el jugador presiona la tecla Escape, entonces
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Si el cursor del mouse está bloqueado, entonces...
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                // Desbloquear el cursor
                Cursor.lockState = CursorLockMode.None;
            }
            // De lo contrario...
            else
            {
                // Bloquear el cursor
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
 
