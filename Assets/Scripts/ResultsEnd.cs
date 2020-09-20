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
using UnityEngine.UI;

public class ResultsEnd : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stars = new List<GameObject>();    //toutes les étoiles
    [SerializeField]
    private Text endText = null;
    [SerializeField]
    private Text feedBack = null;
    [SerializeField]
    private Text points = null;

    private int starIndexRemoved = 2;   //index de l'étoile à cacher

    /// <summary>
    /// Setter
    /// </summary>
    /// <param name="s"></param>
    public void SetFeedback(string s)
    {
        feedBack.text = s;
    }

    /// <summary>
    /// Setter
    /// </summary>
    /// <param name="s"></param>
    public void SetEndText(string s)
    {
        endText.text = s;
    }

    /// <summary>
    /// Retire une étoile sur l'écran de fin de jeu.
    /// </summary>
    public void RemoveStar()
    {
        if (stars[starIndexRemoved])
        {
            stars[starIndexRemoved].SetActive(false);
            if(starIndexRemoved>0)
                starIndexRemoved--;
        }
    }

    /// <summary>
    /// affiche les points en fonction du nombre d'erreurs
    /// </summary>
    /// <param name="error"></param>
    public void SetPoints(int error)
    {
        switch (error)
        {
            case 0: points.text = "500 points";
                break;
            case 1:
                points.text = "400 points";
                break;
            case 2:
                points.text = "250 points";
                break;
            case 3:
                points.text = "250 points";
                break;
            case 4:
                points.text = "0 points";
                break;
        }
    }
}
