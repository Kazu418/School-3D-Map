using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usingを追加
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(InstantSceneChange());
  }

  // Update is called once per frame
  void Update()
  {

  }
  public IEnumerator InstantSceneChange()
  {
    yield return null;
    SceneManager.LoadScene("Testscene"); //別シーンを呼び出す
  }
}
