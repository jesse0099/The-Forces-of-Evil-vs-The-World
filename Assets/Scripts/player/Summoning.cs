using UnityEngine;
using Assets.Scripts.Player.Invocations;
using Assets.Scripts.Enemies.Hero_Knight;
using Assets.Commons;

public class Summoning : MonoBehaviour
{
    public GameObject allyPrefab;
    public KeyCode summonKey = KeyCode.V;
    public int manaCost;
    public int invocationHealth;
    public string invocationRiseAnimation;

    private string currentAnimation;

    private PlayerStats summonerStats;
    private GameObject allyInstance;
    private Animator allyAnimator;
    private SpriteRenderer allyRenderer;

    private void Start()
    {
        summonerStats = GetComponent<PlayerStats>();
    }

    void Update() {
        //&& !allyAlive
        if (Input.GetKeyDown(summonKey)) {
            SummonAlly();
        }
    }

    void SummonAlly() {
        //Apply mana cost on invocation
        if (summonerStats.player_mana - manaCost < 0) return;

        summonerStats.player_mana -= manaCost;

        //Instantiate the summoned ally
        allyInstance = Instantiate(allyPrefab, transform.position +
            new Vector3(transform.localScale.x * 0.4f, 0f, 0f), 
            Quaternion.identity);

        //Adding InvocationStats
        var invocationStats = allyInstance.AddComponent<InvocationStats>();
        invocationStats.health = invocationHealth;
        //Setting death animation
        invocationStats.deathAnimation = Literals.ALLY_SKELETON_ANIMATIONS.
            ally_skeleton_clothed_death.ToString();
        invocationStats.damage = 8;

        //Adding InvocationMovement
        var invocationMovement = allyInstance.AddComponent<InvocationMovement>();
        invocationMovement.attackDelay = 0.3f;


        allyAnimator = allyInstance.GetComponent<Animator>();
        allyRenderer = allyInstance.GetComponent<SpriteRenderer>();

        if (transform.localScale.x > 0)
            allyRenderer.flipX = true;
        else
            allyRenderer.flipX = false;

        ChangeAnimationState(invocationRiseAnimation, ref allyAnimator);
    }

    void ChangeAnimationState(string newAnimation, ref Animator animator) {
        if (newAnimation != invocationRiseAnimation)
            if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
