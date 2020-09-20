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

public class TimerGauge : MonoBehaviour
{
    private Slider timerSlider = null;
    [SerializeField]
    private GameManager gm = null;
    private bool timerStarted = false;  //indique si le timer à démarrer ou non

    private float d, s, v;  //sauvegarde des données du timer.

    /// <summary>
    /// Init du slider
    /// </summary>
    private void Start()
    {
        timerSlider = GetComponent<Slider>();
        timerSlider.maxValue = gm.GetGaugeValue();
        timerSlider.value = gm.GetGaugeValue();
    }

    /// <summary>
    /// Init et lance la coroutine pour démarrer le timer de la jauge.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="speed"></param>
    /// <param name="value"></param>
    public void StartTimer(float delay, float speed, float value)
    {
        d = delay;
        s = speed;
        v = value;

        StartCoroutine(Timer(delay, speed, value));
        timerStarted = true;
    }

    /// <summary>
    /// Redémarre le timer au début.
    /// </summary>
    private void RestartTimer()
    {
        StartCoroutine(Timer(d, s, v));
        PauseTimer();
    }

    /// <summary>
    /// Mets en pause le timer.
    /// </summary>
    public void PauseTimer()
    {
        timerStarted = false;
    }

    /// <summary>
    /// Redémarre le timer.
    /// </summary>
    public void ResumeTimer()
    {
        timerStarted = true;
    }

    /// <summary>
    /// Retourne si le timer à démarrer ou non.
    /// </summary>
    /// <returns></returns>
    public bool IsStarted()
    {
        return timerStarted;
    }

    /// <summary>
    /// Ajout du temps au timer.
    /// </summary>
    /// <param name="time"></param>
    public void AddTime(float time)
    {
        timerSlider.value += time;
    }

    /// <summary>
    /// Retourne le temps restant du timer.
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return timerSlider.value;
    }

    /// <summary>
    /// Coroutine pour démarrer le timer.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="speed"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private IEnumerator Timer(float delay, float speed, float value)
    {
        while (gm.GetError() < 4)   //Si il reste une chance au joueur.
        {
            yield return new WaitForSeconds(delay);

            while (timerSlider.value > 0f)
            {
                while (!timerStarted)
                    yield return null;
                timerSlider.value -= Time.deltaTime * speed;
                yield return null;
            }

            //Si le timer arrive à 0, on retire une chance et on le redémarre.
            if (timerSlider.value <= 0) 
            {
                PauseTimer();
                gm.BeforeCheckAnswer(true);
                gm.AddError();
                timerSlider.value = timerSlider.maxValue / 2;
            }
            yield return null;
        }
    }
}
