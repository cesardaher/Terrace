using System.Collections;
using UnityEngine;

public class ObjMove : MonoBehaviour
{
    public Vector3 initialPos = new Vector3(10, -6.5f);
    public Vector3 endPos;
    public Animator animator;

    public float speed = 3f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        initialPos = transform.position;
    }

    public IEnumerator CatMove(bool moveIn)
    {
        // disable clicking (this shouldn't be needed, but for some reason CanClick is activated when cat moves
        StateMng.instance.CanClick = false;

        //start animation
        animator.SetBool("isSitting", false);

        Vector3 targetPos;

        // move into target position
        if (moveIn)
        {
            Debug.Log("moved in");
            targetPos = endPos;
        }
        else
        {
            targetPos = initialPos;
        }

        //move out
        Task movement = new Task(Move(targetPos));
        while (movement.Running) yield return null;

        //stop animation
        animator.SetBool("isSitting", true);
    }

    public IEnumerator Move(Vector3 position)
    {
        float dist = Vector3.Distance(transform.position, position);

        while (dist > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);

            dist = Vector3.Distance(transform.position, endPos);

            yield return null;
        }
    }

    public IEnumerator FriendMove(bool moveIn)
    {
        // disable clicking (this shouldn't be needed, but for some reason CanClick is activated when cat moves
        StateMng.instance.CanClick = false;

        //start animation
        animator.SetBool("isWalking", true);

        Vector3 targetPos;

        // move into target position
        if (moveIn)
        {
            Debug.Log("moved in");
            targetPos = endPos;
        }
        else
        {
            targetPos = initialPos;
        }

        //move out
        Task movement = new Task(Move(targetPos));
        while (movement.Running) yield return null;

        //stop animation
        animator.SetBool("isWalking", false);
    }

}
