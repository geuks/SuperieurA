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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class A
    {
        [SerializeField]
        public string text;
        [SerializeField]
        public string result;
#if UNITY_EDITOR
        [SerializeField]
        public bool isHide;
#endif

        public A()
        {
            text = "";
            result = "";
#if UNITY_EDITOR
            isHide = false;
#endif
        }
    }

    private const int MAXERROR = 4; //Maximum de chance du joueur

    //Variable en public afin de pouvoir le modifier dans l'Editor plus simplement
    [HideInInspector, SerializeField]
    public List<A> operations = new List<A>();

    //Valeur dans l'éditeur à modifié
    [SerializeField, HideInInspector]
    private float gaugeWinValue = 1f;
    [SerializeField, HideInInspector]
    private float gaugeValue = 10f;
    [SerializeField, HideInInspector]
    private float gaugeSpeed = .2f;
    [SerializeField, HideInInspector]
    private float gaugeDelay = 2f;
    [SerializeField, HideInInspector]
    private string resume = "";
    [SerializeField, HideInInspector]
    private string feedbackLose = "";
    [SerializeField, HideInInspector]
    private string feedbackWin = "";
    [SerializeField, HideInInspector]
    private string rightMessage = "Bravo !";
    [SerializeField, HideInInspector]
    private string wrongMessage = "Dommage !";
    //

    //DATA
    [SerializeField]
    private List<Text> resumeText = new List<Text>();
    [SerializeField]
    private List<GameObject> errorGameObject = new List<GameObject>();
    [SerializeField]
    private Text leftCardOperation = null;
    [SerializeField]
    private Text leftCardResult = null;
    [SerializeField]
    private Text rightCardOperation = null;
    [SerializeField]
    private Text rightCardResult = null;
    [SerializeField]
    private Text condition = null;
    [SerializeField]
    private Text answerText = null;
    [SerializeField]
    private GameObject zoomCard = null;
    [SerializeField]
    private GameObject validateButton = null;
    [SerializeField]
    private GameObject continueButton = null;
    [SerializeField]
    private GameObject resultButton = null;
    [SerializeField]
    private Button switchButton = null;
    [SerializeField]
    private ResultsEnd resultsEnd = null;
    [SerializeField]
    private TimerGauge timerGauge = null;
    [SerializeField]
    private GraphicRaycaster graphicRay = null;
    //

    private bool switched = false;
    private int error = 0;
    private Vector2 startPosition = Vector2.zero;
    private Vector2 endPosition = Vector2.zero;
    private bool isIn = false;
    private GameObject lastCard = null;
    private float timerTouch = 0f;
    private bool isLongPressing = false;
    private Vector2 lastSizeCard = Vector2.zero;

    /// <summary>
    /// Démarrage init les valeurs des consignes
    /// </summary>
    private void Start()
    {
        foreach(Text t in resumeText)
            t.text = resume;
    }

    /// <summary>
    /// Gestion des inputs touch
    /// </summary>
    private void Update()
    {
        if (Input.touchCount == 1)  //si le joueur appuie sur l'écran
        {
            PointerEventData pointerEvent = new PointerEventData(null);
            List<RaycastResult> results = new List<RaycastResult>();
            
            pointerEvent.position = Input.GetTouch(0).position;
            graphicRay.Raycast(pointerEvent, results);

            if (Input.GetTouch(0).phase == TouchPhase.Stationary)   //s'il ne bouge pas
            {
                timerTouch += Time.deltaTime;

                if(timerTouch > .5f)
                {
                    isLongPressing = true;
                    if (results.Count > 0)
                    {
                        if (results[0].gameObject.tag == "Card")
                        {
                            //On affiche l'image "zoomé"
                            zoomCard.GetComponentInChildren<Text>().text = results[0].gameObject.GetComponentInChildren<Text>().text;
                            zoomCard.SetActive(true);
                        }
                    }
                }
            }

            if (!isLongPressing)    //s'il appuie ou reste appuyer en bougeant
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (results.Count > 0)
                    {
                        if (results[0].gameObject.tag == "Card")    //On verifie d'ou le joueur a appuyer
                        {
                            startPosition = pointerEvent.position;
                            isIn = true;
                            lastCard = results[0].gameObject;
                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (results.Count > 0)
                    {
                        //On verifie si il relache au bon endroit
                        if (results[0].gameObject.tag == "Card" && lastCard.name != results[0].gameObject.name)
                        {
                            endPosition = pointerEvent.position;
                            if (Vector2.Distance(startPosition, endPosition) > 100 && isIn)
                            {
                                SwitchCards();  //On remplace
                                isIn = false;
                            }
                        }

                    }
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)    //s'il relache et que le joueur et en "zoom"
            {
                isLongPressing = false;
                zoomCard.SetActive(false);  //On désactive l'image "zoomé"
            }
            
        }
    }

    /// <summary>
    /// Démarre le timer de la jauge
    /// </summary>
    public void StartTimerGauge()
    {
        timerGauge.StartTimer(gaugeDelay, gaugeSpeed, gaugeValue);
    }

    /// <summary>
    /// Echange les cartes entre elles
    /// </summary>
    public void SwitchCards()
    {
        string tmpOperation = "";
        string tmpResult = "";
        if (!switched)
        {
            tmpOperation = rightCardOperation.text;
            tmpResult = rightCardResult.text;

            rightCardOperation.text = leftCardOperation.text;
            rightCardResult.text = leftCardResult.text;

            leftCardOperation.text = tmpOperation;
            leftCardResult.text = tmpResult;

            switched = true;
        }
        else
        {
            tmpOperation = leftCardOperation.text;
            tmpResult = leftCardResult.text;

            leftCardOperation.text = rightCardOperation.text;
            leftCardResult.text = rightCardResult.text;

            rightCardOperation.text = tmpOperation;
            rightCardResult.text = tmpResult;
            switched = false;
        }
    }

    /// <summary>
    /// Modifie l'UI principalement
    /// </summary>
    /// <param name="error"></param>
    public void BeforeCheckAnswer(bool error = false)
    {
        if(error)
            answerText.text = wrongMessage;
        rightCardResult.gameObject.SetActive(true);
        leftCardResult.gameObject.SetActive(true);
        answerText.gameObject.SetActive(true);
        validateButton.SetActive(false);
        continueButton.SetActive(true);
        switchButton.interactable = false;
    }

    /// <summary>
    /// Verifie si le joueur à répondu correctement.
    /// </summary>
    public void CheckAnswer()
    {
        float tmpTimer = 0f;

        timerGauge.PauseTimer();

        if (condition.text == "<")
        {
            if(int.Parse(leftCardResult.text) < int.Parse(rightCardResult.text))
            {
                tmpTimer += gaugeWinValue;
                answerText.text = rightMessage;
            }
            else
            {
                tmpTimer -= gaugeWinValue;
                answerText.text = wrongMessage;
            }
        }
        else
        {
            if (int.Parse(leftCardResult.text) > int.Parse(rightCardResult.text))
            {
                tmpTimer += gaugeWinValue;
                answerText.text = rightMessage;
            }
            else
            {
                tmpTimer -= gaugeWinValue;
                answerText.text = wrongMessage;
            }
        }
        
        BeforeCheckAnswer();

        if(timerGauge.GetTime() + tmpTimer > gaugeValue)    //si la jauge est supérieur au maximum, alors il a gagné
        {
            resultsEnd.SetFeedback(feedbackWin);
            resultsEnd.SetEndText(rightMessage);

            continueButton.SetActive(false);
            switchButton.gameObject.SetActive(false);
            resultButton.SetActive(true);

            resultsEnd.SetPoints(error);
        }
        else    //s'il n'a pas gagné on ajoute le temps (positif/negatif)
            timerGauge.AddTime(tmpTimer);
    }

    /// <summary>
    /// Ajoute une erreur au joueur.
    /// </summary>
    public void AddError()
    {
        errorGameObject[error].SetActive(true);
        error++;
        resultsEnd.RemoveStar();

        if (error == errorGameObject.Count)
        {
            continueButton.SetActive(false);
            switchButton.gameObject.SetActive(false);
            resultButton.SetActive(true);
            resultsEnd.SetFeedback(feedbackLose);
            resultsEnd.SetEndText(wrongMessage);
        }
    }

    /// <summary>
    /// Retourne le nombre d'erreur.
    /// </summary>
    /// <returns></returns>
    public int GetError()
    {
        return error;
    }

    /// <summary>
    /// Retourne la valeur de la jauge.
    /// </summary>
    /// <returns></returns>
    public float GetGaugeValue()
    {
        return gaugeValue;
    }

    /// <summary>
    /// Génére une carte aléatoirement depuis la liste A.
    /// </summary>
    public void GenerateCard()
    {
        int n = GetRandom(operations.Count);
        int nb = n;

        if (n > 0 && n < operations.Count - 1)  //Si le chiffre n'est pas le premier ou le dernier
        {
            if (GetRandom(2) == 0)  //On récupère le chiffre suivant ou précedent aléatoirement
                nb += 1;
            else
                nb -= 1;
        }
        else if (n == 0)    //Si le chiffre est 0 on prend le chiffre suivant
            nb += 1;
        else
            nb -= 1;    //Si le chiffre est le dernier on prend le chiffre précedent

        if (GetRandom(2) == 0)  //On ajoute le signe inférieur ou supérieur aléatoirement.
            condition.text = "<";
        else
            condition.text = ">";

        if (GetRandom(2) == 0)  //On ajoute les valeurs de la carte à gauche ou à droite (aléatoirement)
        {
            rightCardOperation.text = operations[n].text;
            rightCardResult.text = operations[n].result;

            leftCardOperation.text = operations[nb].text;
            leftCardResult.text = operations[nb].result;
        }
        else
        {
            leftCardOperation.text = operations[n].text;
            leftCardResult.text = operations[n].result;

            rightCardOperation.text = operations[nb].text;
            rightCardResult.text = operations[nb].result;
        }
    }

    /// <summary>
    /// Retourne une valeur aléatoire depuis un maximum donné en paramètre.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private int GetRandom(int n)
    {
        return Random.Range(0, n);
    }
}