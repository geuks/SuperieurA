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
using UnityEngine.UI;

public class TimerSetup : MonoBehaviour
{
    [SerializeField]
    private Text timerText = null;  
    [SerializeField]
    private int timeToWait = 3; //temps à attendre (décompte)
    [SerializeField]
    private GameManager gm = null;

    /// <summary>
    /// Démarre le timer lorsque le gameobject et activé
    /// </summary>
    private void OnEnable()
    {
        StartTimer(timeToWait);
        gm.GenerateCard();
    }

    /// <summary>
    /// Lance la coroutine pour démarrer le timer.
    /// </summary>
    /// <param name="timeToWait"></param>
    private void StartTimer(int timeToWait)
    {
        StartCoroutine(Timer(timeToWait));
    }

    /// <summary>
    /// Coroutine qui permet de démarrer le timer
    /// </summary>
    /// <param name="timeToWait"></param>
    /// <returns></returns>
    private IEnumerator Timer(int timeToWait)
    {
        float timer = timeToWait + 1f; //+1 car la première seconde n'est pas compté.

        while(timer >= 0f)
        {
            timerText.text = ((int)timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Démarre le timer lors de la désactivation du gameobject.
    /// </summary>
    private void OnDisable()
    {
        gm.StartTimerGauge();
    }
}
