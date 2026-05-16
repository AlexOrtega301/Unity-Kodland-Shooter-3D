using UnityEngine;
using Photon.Pun;
 
 
public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int health = 100; 
    [SerializeField] protected float attackDistance = 3;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float cooldown = 2;
    
    protected GameObject player;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance;
    protected float timer;
    bool dead = false;
    protected GameObject[] players;
    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }
 
    public virtual void Move()
    {
        
    }
    public virtual void Attack()
    {
        
    }
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject; 
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CheckPlayers();
    }
    private void Update()
    {
        //Declarar una variable que almacenará una distancia mínima
        //Mathf.Infinity - infinito positivo
        float closestDistance = Mathf.Infinity;
        //Pasando por la lista de jugadores
        foreach (GameObject closestPlayer in players)
        {
            //Calculando la distancia entre el enemigo y el jugador
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //Si la distancia hasta este jugador es menor que la distancia hasta el jugador anterior, entonces...
            if (checkDistance < closestDistance)
            {
                //Si el jugador anterior está vivo
                if(closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Guardar el jugador actual como el jugador más cercano
                    player = closestPlayer;
                    //Cambiando el valor de closestDistance a la distancia a este jugador
                    closestDistance = checkDistance;
                }
            }
        }
        //Comprobando si la variable jugador tiene un jugador en ella
        //Esta comprobación nos ayudará a prevenir errores
        if (player != null)
        {
            //El resto del script no cambió con respecto a las lecciones anteriores
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (!dead)
            {
                Attack();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!dead && player != null)
        {
            Move();
        }
    }
    
    [PunRPC]
    public void ChangeHealth(int count)
    {
        // restando salud
        health -= count;
        // si la salud cae a cero o menos, entonces ...
        if(health <= 0)
        {
            // cambiando el valor de la variable muerta, lo que significa que las llamadas a las funciones Ataque y Movimiento ya no funcionarán
            dead = true;
            // Desactivar el colisionador del enemigo
            GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            // Habilitando la animación de muerte
            anim.SetBool("Die", true);
        }
    }
    public void GetDamage(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }
}