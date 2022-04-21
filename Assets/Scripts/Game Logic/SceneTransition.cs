using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : GenericSingleton<SceneTransition>
{
    public Animator anim;

    public void StartTransition(int index)
    {
        StartCoroutine(ChangeScene(index));
    }

    IEnumerator ChangeScene(int index)
    {
        anim.SetTrigger("canTransit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
