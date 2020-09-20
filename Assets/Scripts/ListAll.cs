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

using UnityEngine;

public class ListAll : MonoBehaviour
{
    [SerializeField]
    private GameManager gm = null;  //Gamemanager
    [SerializeField]
    private GameObject operation = null;    //prefab Operation pour afficher la correction.

    /// <summary>
    /// Génére la liste des cartes pour la correction.
    /// </summary>
    void Start()
    {
        for(int i = 0; i < gm.operations.Count; i++)
        {
            GameObject op = Instantiate(operation, gameObject.transform);
            op.GetComponent<InitOperation>().InitOp(gm.operations[i].text, gm.operations[i].result);
        }
    }
}
