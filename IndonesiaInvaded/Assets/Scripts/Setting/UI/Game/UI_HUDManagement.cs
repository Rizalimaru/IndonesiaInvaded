using System.Collections;
using UnityEngine;

public class UI_HUDManagement : MonoBehaviour
{
    [SerializeField] float idleTimeThreshold = 2f; // Waktu idle sebelum UI di sembunyikan
    [SerializeField] GameObject[] uiHUD; // Referensi ke UI HUD yang akan di sembunyikan
    [SerializeField] CanvasGroup[] UIGroup; // Komponen CanvasGroup untuk mengatur alpha

    private Coroutine idleCoroutine; // Coroutine untuk mengatur waktu idle

    private void Update()
    {
        // Cek apakah player sedang bergerak
        bool isIdle = PlayerAnimator.instance.anim.GetBool("isIdle");
        bool isAttacking = PlayerAnimator.instance.anim.GetBool("hit1");
        bool isAttacking2 = PlayerAnimator.instance.anim.GetBool("hit2");
        bool isAttacking3 = PlayerAnimator.instance.anim.GetBool("hit3");
        bool isJumping = PlayerAnimator.instance.anim.GetBool("isJump");

        //Jika ada tag enemy atau boss yang terdeteksi dengan jarak 30, maka UI tidak akan hilang
        if (!(GameObject.FindGameObjectWithTag("Enemy") || GameObject.FindGameObjectWithTag("Boss")))
        {
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

                // Tampilkan UI jika player tidak sedang idle
                foreach (var uiGroup in UIGroup)
                {
                    if (uiGroup.alpha == 0)
                    {
                        StartCoroutine(ShowUI(uiGroup));
                    }
                }
            }
        }
    }

    IEnumerator ShowUI(CanvasGroup canvasGroup)
    {
        // Mulai fadeIn
        yield return StartCoroutine(FadeIn(canvasGroup));
    }

    IEnumerator IdleTimer()
    {
        // Tunggu idleTimeThreshold sebelum menyembunyikan UI
        yield return new WaitForSeconds(idleTimeThreshold);
        // Sembunyikan semua UI
        foreach (var uiGroup in UIGroup)
        {
            //yield return StartCoroutine(FadeOut(uiGroup));
            uiGroup.gameObject.SetActive(false);
            uiGroup.alpha = 0;
        }
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
        canvasGroup.gameObject.SetActive(true);
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 2;
            yield return null;
        }
    }
}
