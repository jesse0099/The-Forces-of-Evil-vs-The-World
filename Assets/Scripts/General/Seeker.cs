using UnityEngine;

public class Seeker : MonoBehaviour
{
    public string seekForTagPrio0;
    public string seekForTagPrio1;
    public string hunterWalkAnimation;
    public string hunterJumpAnimation;
    public string hunterIdleAnimation;

    private string currentAnimation = string.Empty;

    public float hunterSafeDistance = 0.1f;

    public float huntInterval = 1f;
    public float timeSinceLastHunt = 0f;

    
    [SerializeField] private float huntSpeed = 5f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float rayCastDistance = 1f;
    public LayerMask obstacleLayer;

    Animator animator;
    Rigidbody2D rigidBody;
    GameObject nearestTarget = null;
    GameObject[] targets;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Find all gameobjects with the Enemy tag
        //It could not work as Im using composed Tags separated by comma
        //A prio set of strings could work, as even if composed and distinct tags,
        //they'll share the same attribute, be a fucking enemy
        targets = GameObject.FindGameObjectsWithTag(seekForTagPrio0);
        if (targets is null)
            targets = GameObject.FindGameObjectsWithTag(seekForTagPrio1);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastHunt += Time.deltaTime;
        if(timeSinceLastHunt >= huntInterval)
        {
            UpdateNearestTarget();
            timeSinceLastHunt = 0f;
        }

        //Check if an enemy was found
        if (nearestTarget is not null)
        {
            try
            {
                //Avoid hunter to get stuck under target feets
                var targetDistance = Vector2.Distance(transform.position, nearestTarget.
                    transform.position);

                //Walk animation for summoned allies
                if (tag.Equals("Ally,Knockout"))
                    ChangeAnimationState(hunterWalkAnimation);

                //Hunter Safe distance
                if (targetDistance <= hunterSafeDistance) return;

                // Calculate the movement direction
                Vector2 moveDirection = new(nearestTarget.transform.position.x - transform.position.x, 0f);

                // Face the target
                if (moveDirection.x < 0f)
                    GetComponent<SpriteRenderer>().flipX = false;
                else if (moveDirection.x > 0f)
                    GetComponent<SpriteRenderer>().flipX = true;

                //Limit the hunt to the X axis
                var targetXAxisPosition = new Vector2(nearestTarget.transform.position.x,
                    transform.position.y);

                //Check for obstacles using raycast
                RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, rayCastDistance,
                    obstacleLayer);
                //Apply a force in Y equals to the mass multiplied by a jump factor
                if (hit.collider != null)
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                //Move towards nearest enemy
                transform.position = Vector2.MoveTowards(transform.position,
                        targetXAxisPosition,
                        huntSpeed * Time.deltaTime);
            }
            catch (MissingReferenceException)
            {
                //Dead target
                //Ally Idle animation while waiting for another target
                ChangeAnimationState(hunterIdleAnimation);
            }
        }

    }

    void UpdateNearestTarget()
    {
        targets = GameObject.FindGameObjectsWithTag(seekForTagPrio0);
        if (targets is null)
            targets = GameObject.FindGameObjectsWithTag(seekForTagPrio1);
        //Look for targets
        nearestTarget = null;

        //Find the nearest enemy
        var minDistance = Mathf.Infinity;
        foreach (GameObject target in targets)
        {
            var distance = Vector2.Distance(transform.position, target.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target;
            }
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}