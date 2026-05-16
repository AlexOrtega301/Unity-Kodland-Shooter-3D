using UnityEngine;
using Photon.Pun;
public class MiniMap : MonoBehaviourPunCallbacks

{
    // Velocidad de movimiento
    [SerializeField] private float scrollSpeed = 1f;
    // Nivel mínimo de zoom
    [SerializeField] private float minValue = 10f;
    // Nivel máximo de zoom
    [SerializeField] private float maxValue = 60f;
    // Nivel actual de zoom
    private float currentValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0){
            currentValue += scrollSpeed;
        }
        else if (scrollDelta < 0){
            currentValue -= scrollSpeed;
        }
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        gameObject.GetComponent<Camera>().orthographicSize = currentValue;

    }
}
