using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CinemachineVirtualCamera _camera;

    private void OnEnable()
    {
        Star.OnStarExploded += OnStarExploded;
    }

    private void OnDisable()
    {
        Star.OnStarExploded -= OnStarExploded;
    }

    private void OnStarExploded(Star star, int ending)
    {
        _camera.enabled = true;
        StartCoroutine(Ending());

        IEnumerator Ending()
        {
            yield return new WaitForSeconds(2f);
            star.gameObject.SetActive(false);
            Time.timeScale = 0f;
            _animator.SetInteger("Ending", ending);

            yield return new WaitForSecondsRealtime(10f);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
