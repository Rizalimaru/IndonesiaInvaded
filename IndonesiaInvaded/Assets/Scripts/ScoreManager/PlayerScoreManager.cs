using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{

    public Collider collider;
    
    private void Start(){

    }

    private void OnTriggerEnter(Collider collider)
    {
        bool isAttacking = PlayerAnimator.instance.anim.GetBool("hit1");
        bool isAttacking2 = PlayerAnimator.instance.anim.GetBool("hit2");
        bool isAttacking3 = PlayerAnimator.instance.anim.GetBool("hit3");


        // Periksa apakah objek yang bertabrakan memiliki tag "CheckHitScore"
        if(collider.tag == "CheckHitScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            // Tambah skor jika kriteria terpenuhi
            ScoreManager.instance.AddScore(100);
        }

        if(collider.tag == "Enemy_Melee" && (isAttacking || isAttacking2 || isAttacking3))
        {
            // Tambah skor jika kriteria terpenuhi
            ScoreManager.instance.AddScore(100);
        }

        

        if(collider.tag == "AmountEnemyScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            Debug.Log("Enemy Defeated");
            Destroy(collider.gameObject);
            ScoreManager.instance.AddEnemyDefeats(1);

        }
        if(collider.tag == "AmountBossScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            Destroy(collider.gameObject);
            ScoreManager.instance.AddBossDefeats(1);

        }
    }

}
