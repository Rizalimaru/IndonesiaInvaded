using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    
    private void Start(){

    }

    private void OnCollisionEnter(Collision collision)
    {

        Collider other = collision.collider;

        bool isAttacking = PlayerAnimator.instance.anim.GetBool("hit1");
        bool isAttacking2 = PlayerAnimator.instance.anim.GetBool("hit2");
        bool isAttacking3 = PlayerAnimator.instance.anim.GetBool("hit3");


        // Periksa apakah objek yang bertabrakan memiliki tag "CheckHitScore"
        if(other.tag == "CheckHitScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            // Tambah skor jika kriteria terpenuhi
            ScoreManager.instance.AddScore(100);
        }

        if(other.tag == "Enemy_Melee" && (isAttacking || isAttacking2 || isAttacking3))
        {
            // Tambah skor jika kriteria terpenuhi
            ScoreManager.instance.AddScore(100);
        }

        

        if(other.tag == "AmountEnemyScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            Debug.Log("Enemy Defeated");
            Destroy(other.gameObject);
            ScoreManager.instance.AddEnemyDefeats(1);

        }
        if(other.tag == "AmountBossScore" && (isAttacking || isAttacking2 || isAttacking3))
        {
            Destroy(other.gameObject);
            ScoreManager.instance.AddBossDefeats(1);

        }
    }

}
