/*
Copyright (C) 2020  Gökhan UNALAN

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Slider loadingBar = null;  //Barre de chargement

    /// <summary>
    /// Procédure qui permet de lancer la scene de jeu.
    /// </summary>
    /// <param name="nextScene"></param>
    public void PlayGame(string nextScene)
    {
        StartCoroutine(PlayGameCoroutine(nextScene));
    }

    /// <summary>
    /// Coroutine qui permet d'afficher la barre de chargement et d'ensuite lancer la scene de jeu.
    /// </summary>
    /// <param name="nextScene"></param>
    /// <returns></returns>
    private IEnumerator PlayGameCoroutine(string nextScene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Single);

        if (asyncLoad != null)
        {
            loadingBar.gameObject.SetActive(true);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                loadingBar.value = asyncLoad.progress + .1f;

                yield return null;

                if (loadingBar.value >= 1f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
        }
    }

    /// <summary>
    /// Procédure qui permet de quitter le jeu peut importe l'endroit ou il est lancé.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
#else
                Application.Quit();
#endif
    }
}
