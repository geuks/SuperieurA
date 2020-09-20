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

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    SerializedProperty resume;
    SerializedProperty feedbackLose;
    SerializedProperty feedbackWin;
    SerializedProperty rightMessage;
    SerializedProperty wrongMessage;
    SerializedProperty gaugeWinValue;
    SerializedProperty gaugeValue;
    SerializedProperty gaugeSpeed;
    SerializedProperty gaugeDelay;
    SerializedProperty operations;

    bool hideData = false;

    GUIStyle title = null;

    ReorderableList reorderableList;
    GameManager gm;

    

    private void OnEnable()
    {
        gm = (GameManager)target;

        resume = serializedObject.FindProperty("resume");
        feedbackLose = serializedObject.FindProperty("feedbackLose");
        feedbackWin = serializedObject.FindProperty("feedbackWin");
        rightMessage = serializedObject.FindProperty("rightMessage");
        wrongMessage = serializedObject.FindProperty("wrongMessage");
        gaugeWinValue = serializedObject.FindProperty("gaugeWinValue");
        gaugeValue = serializedObject.FindProperty("gaugeValue");
        gaugeSpeed = serializedObject.FindProperty("gaugeSpeed");
        gaugeDelay = serializedObject.FindProperty("gaugeDelay");
        operations = serializedObject.FindProperty("operations");
        
        reorderableList = new ReorderableList(serializedObject, operations, true, false, true, true);

        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;

        reorderableList.drawElementBackgroundCallback += DrawElementCallback;
    }

    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;

        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;

        reorderableList.drawElementBackgroundCallback -= DrawElementCallback;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "List A");
    }


    /// <summary>
    /// Draws background of one element of the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    protected void DrawElementCallback(Rect rect, int index, bool active, bool focused)
    {
        rect.height = 18;
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(0.33f, 0.66f, 1f, 0.2f));
        tex.Apply();
        if (active)
            GUI.DrawTexture(rect, tex as Texture);
    }

    /// <summary>
    /// Draws one element of the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    protected void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        gm.operations[index].isHide = !active;
        Rect r = rect;
        EditorGUI.LabelField(rect, "Calcul : " + gm.operations[index].text + " = " + gm.operations[index].result);
    }

    private void AddItem(ReorderableList list)
    {
        if(gm.operations.Count < 10)
            gm.operations.Add(new GameManager.A());
        list.index = list.count;
        serializedObject.Update();
    }

    private void RemoveItem(ReorderableList list)
    {
        gm.operations.RemoveAt(list.index);

        list.index = gm.operations.Count - 1;
        serializedObject.Update();
    }

    public override void OnInspectorGUI()
    {
        if (title == null)
        {
            title = (GUIStyle)"MeTransitionSelect";
            title.normal.textColor = Color.white;
            title.fontSize = 18;
        }

        serializedObject.Update();

        GUILayout.Space(20);

        GUILayout.BeginVertical("box");

        GUILayout.Space(20);

        GUILayout.Label("Textes", title, GUILayout.Height(25));

        GUILayout.Space(5);

        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Consignes :");
        
        resume.stringValue = GUILayout.TextArea(resume.stringValue, 150, GUILayout.Width(200), GUILayout.Height(90), GUILayout.MinWidth(40));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Message Réussite:");
        rightMessage.stringValue = GUILayout.TextArea(rightMessage.stringValue, 150, GUILayout.Width(200), GUILayout.Height(30), GUILayout.MinWidth(20));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Message Échec:");
        wrongMessage.stringValue = GUILayout.TextArea(wrongMessage.stringValue, 150, GUILayout.Width(200), GUILayout.Height(30), GUILayout.MinWidth(20));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Feedback Réussite:");
        feedbackWin.stringValue = GUILayout.TextArea(feedbackWin.stringValue, 150, GUILayout.Width(200), GUILayout.Height(90), GUILayout.MinWidth(40));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Feedback Échec:");
        feedbackLose.stringValue = GUILayout.TextArea(feedbackLose.stringValue, 150, GUILayout.Width(200), GUILayout.Height(90), GUILayout.MinWidth(40));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();


        GUILayout.Space(20);

        GUILayout.Label("Jauge (Timer)", title, GUILayout.Height(25));

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 200;
        EditorGUIUtility.fieldWidth = 25;
        gaugeWinValue.floatValue = EditorGUILayout.FloatField("Valeur Gain/Perte:",gaugeWinValue.floatValue, GUILayout.Width(250));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 200;
        EditorGUIUtility.fieldWidth = 25;
        gaugeValue.floatValue = EditorGUILayout.FloatField("Valeur:",gaugeValue.floatValue, GUILayout.Width(250));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 200;
        EditorGUIUtility.fieldWidth = 25;
        gaugeSpeed.floatValue = EditorGUILayout.FloatField("Vitesse:",gaugeSpeed.floatValue, GUILayout.Width(250));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 200;
        EditorGUIUtility.fieldWidth = 25;
        gaugeDelay.floatValue = EditorGUILayout.FloatField("Délais:", gaugeDelay.floatValue, GUILayout.Width(250));
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        GUILayout.Space(20);
        GUILayout.Label("Liste des cartes", title, GUILayout.Height(25));
        GUILayout.Space(5);

        reorderableList.DoLayoutList();
        for (int i = 0; i < gm.operations.Count; i++)
        {
            if (!gm.operations[i].isHide)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(i + 1 + " :");
                GUILayout.Label("Opération ");
                gm.operations[i].text = GUILayout.TextArea(gm.operations[i].text, 50, GUILayout.Width(200), GUILayout.Height(35), GUILayout.MinWidth(40));

                GUILayout.Label("Résultat ");
                gm.operations[i].result = GUILayout.TextArea(gm.operations[i].result, 50, GUILayout.Width(200), GUILayout.Height(35), GUILayout.MinWidth(40));


                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(20);
        GUILayout.Label("DATA", title, GUILayout.Height(25));

        if (GUILayout.Button("Afficher / Cacher - DATA", GUI.skin.box))
        {
            hideData = !hideData;
        }

        if(!hideData)
            base.OnInspectorGUI();

        GUILayout.EndVertical();

        EditorUtility.SetDirty(gm);
        
    }
}

