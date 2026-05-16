using UnityEngine;
public class WalkEemy : Enemy
{
    // La velocidad del escarabajo
    [SerializeField] float speed = 10;
    // Área de detección del escarabajo
    [SerializeField] float detectionDistance = 100;  
    public override void Move()
        {
            // Si la distancia entre el enemigo y el jugador es menor que el radio de detección del escarabajo
            // Y la distancia entre el enemigo y el jugador es mayor que el radio de ataque, entonces:
            if (distance < detectionDistance && distance > attackDistance)
            {
                // Girando al enemigo hacia el jugador
                transform.LookAt(player.transform);
                // Habilitar la animación de correr
                anim.SetBool("Run", true);
                // Moviendo el escarabajo hacia adelante
                rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            }
            // De lo contrario:
            else
            {
                // Desactivar la animación de correr
                anim.SetBool("Run", false);
            }
        }  
        public override void Attack()
            {
                // Habilitar el temporizador
                timer += Time.deltaTime;
                // Si la distancia entre el enemigo y el jugador es menor que la distancia de ataque y el valor del temporizador es mayor que el tiempo de recuperación del ataque
                if (distance < attackDistance && timer > cooldown)
                {
                    // Reinicio del temporizador
                    timer = 0;
                    // Obtener el script del jugador y llamar a la función de resta de salud
                    player.GetComponent<PlayerController>().ChangeHealth(damage);
                    // Habilitar la animación de ataque
                    anim.SetBool("Attack", true);
                }
                // De lo contrario...
                else
                {
                    // Desactivar la animación de ataque
                    anim.SetBool("Attack", false);
                }
            }
}