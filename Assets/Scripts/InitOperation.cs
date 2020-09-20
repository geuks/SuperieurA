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
using UnityEngine.UI;

public class InitOperation : MonoBehaviour
{

    [SerializeField]
    private Text op = null;
    [SerializeField]
    private Text result = null;

    /// <summary>
    /// Permets l'initialisation des cartes dans la liste de correction.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="r"></param>
    public void InitOp(string o, string r)
    {
        op.text = o;
        result.text = r;
    }
}
