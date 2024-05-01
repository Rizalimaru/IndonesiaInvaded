using System.Collections;
using UnityEngine;

public class UI_HUDManagement : MonoBehaviour
{
    [SerializeField] float idleTimeThreshold = 2f; // Waktu idle sebelum UI di sembunyikan
    [SerializeField] GameObject uiHUD; // Referensi ke UI HUD yang akan di sembunyikan
    [SerializeField] CanvasGroup UIGroup; // Komponen CanvasGroup untuk mengatur alpha

    private Coroutine idleCoroutine; // Coroutine untuk mengatur waktu idle

    void Start()
    {
        // Langsung mengakses instance PlayerAnimator
    }

    private void Update()
    {
        // Cek apakah player sedang bergerak
        bool isIdle = PlayerAnimator.instance.anim.GetBool("isIdle");
        bool isAttacking = PlayerAnimator.instance.anim.GetBool("hit1");
        bool isAttacking2 = PlayerAnimator.instance.anim.GetBool("hit2");
        bool isAttacking3 = PlayerAnimator.instance.anim.GetBool("hit3");
        bool isJumping = PlayerAnimator.instance.anim.GetBool("isJump");

        if (isIdle && !isAttacking && !isAttacking2 && !isAttacking3 && !isJumping)
        {
            // Memulai atau melanjutkan coroutine hanya jika player sedang idle
            if (idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(IdleTimer());
            }
        }
        else
        {
            // Hentikan coroutine jika player sedang bergerak
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            // Tampilkan UI jika sedang bergerak
            if (!uiHUD.activeSelf)
            {
                uiHUD.SetActive(true);
                StartCoroutine(ShowUI());
            }

        }
    }
    
    IEnumerator ShowUI()
    {
        // Mulai fadeIn
        yield return StartCoroutine(FadeIn(UIGroup));
    }

    IEnumerator IdleTimer()
    {
        // Tunggu selama idleTimeThreshold sebelum menyembunyikan UI
        yield return new WaitForSeconds(idleTimeThreshold);

        // Mulai fadeOut
        yield return StartCoroutine(FadeOut(UIGroup));
        
        // Setelah fadeOut selesai, sembunyikan UI
        uiHUD.SetActive(false);
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        // Transisi fadeOut
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        // Transisi fadeIn
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime*2;
            yield return null;
        }
    }
}
