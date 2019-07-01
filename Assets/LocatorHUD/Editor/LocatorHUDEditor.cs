using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// This class is the custom editor for a locator hud.
/// </summary>
[CustomEditor(typeof(LocatorHUD))]
public class LocatorHUDEditor : Editor
{
    #region Enumerations

    /// <summary>
    /// This enum is used to identify direction variables in the locator hud.
    /// </summary>
    public enum DirectionVariable
    {
        Rotation,
        BaseScale,
        Color,
        DistScalingEnabled,
        DistScalingMinDistance,
        DistScalingMaxDistance,
        DistScalingNearScale,
        DistScalingFarScale,
        Texture
    }

    #endregion

    #region Private Member Variables

    /// <summary>
    /// This array is used to determine whether or not the foldout element for each direction is currently open.
    /// </summary>
    private bool[] m_bShowDirection = new bool[(int)LocatorHUD.Direction.NumDirections];

    #endregion

    #region Init/Deinit

    /// <summary>
    /// Main constructor.
    /// </summary>
    public LocatorHUDEditor()
    {
        for (int i = 0; i < (int)LocatorHUD.Direction.NumDirections; i++)
        {
            m_bShowDirection[i] = false;
        }
    }

    #endregion

    #region GUI

    /// <summary>
    /// This is called when the inspector gui for this should be displayed.
    /// </summary>
    public override void OnInspectorGUI()
    {
        //make this look like inspect and make sure the target is valid
        EditorGUIUtility.LookLikeInspector();
        LocatorHUD locator = target as LocatorHUD;
        if (locator == null)
            return;

        //display basic property editors
        locator.isVisible = EditorGUILayout.Toggle("  Is Visible", locator.isVisible);
        locator.horizontalSpacing = EditorGUILayout.IntField("  Horizontal Spacing", locator.horizontalSpacing);
        locator.verticalSpacing = EditorGUILayout.IntField("  Vertical Spacing", locator.verticalSpacing);
        locator.fadeInSpeed = Mathf.Clamp(EditorGUILayout.FloatField("  Fade In Speed", locator.fadeInSpeed), 0.01f, 100.0f);
        locator.fadeOutSpeed = Mathf.Clamp(EditorGUILayout.FloatField("  Fade Out Speed", locator.fadeOutSpeed), 0.01f, 100.0f);
        locator.baseAlpha = Mathf.Clamp(EditorGUILayout.FloatField("  Alpha", locator.baseAlpha), 0.0f, 1.0f);

        //loop through all directions
        for (int i = 0; i < (int)LocatorHUD.Direction.NumDirections; i++)
        {
            //get the display name for this direction
            string enumName = "  " + ((LocatorHUD.Direction)i).ToString().Replace('_', ' ') + " Indicator";

            //show fold out
            m_bShowDirection[i] = EditorGUILayout.Foldout(m_bShowDirection[i], enumName + (locator.directionEnabled[i] ? " (Enabled)" : " (Disabled)"));
            
            //display options if foldout is open.
            if (m_bShowDirection[i])
            {
                locator.directionEnabled[i] = EditorGUILayout.Toggle("      Enabled", locator.directionEnabled[i]);
                if (locator.directionEnabled[i])
                {
                    DisplayVariableInspector("        Texture", locator, (LocatorHUD.Direction)i, DirectionVariable.Texture, locator.directionTexture[i]);
                    DisplayVariableInspector("        Color", locator, (LocatorHUD.Direction)i, DirectionVariable.Color, locator.directionColor[i]);
                    DisplayVariableInspector("        Base Scale", locator, (LocatorHUD.Direction)i, DirectionVariable.BaseScale, locator.directionBaseScale[i]);
                    DisplayVariableInspector("        Rotation", locator, (LocatorHUD.Direction)i, DirectionVariable.Rotation, locator.directionRotation[i]);
                    DisplayVariableInspector("        Scale with Distance", locator, (LocatorHUD.Direction)i, DirectionVariable.DistScalingEnabled, locator.directionDistScalingEnabled[i]);

                    if (locator.directionDistScalingEnabled[i])
                    {
                        DisplayVariableInspector("          Min Distance", locator, (LocatorHUD.Direction)i, DirectionVariable.DistScalingMinDistance, locator.directionDistScalingMinDistance[i]);
                        DisplayVariableInspector("          Max Distance", locator, (LocatorHUD.Direction)i, DirectionVariable.DistScalingMaxDistance, locator.directionDistScalingMaxDistance[i]);
                        DisplayVariableInspector("          Near Scale", locator, (LocatorHUD.Direction)i, DirectionVariable.DistScalingNearScale, locator.directionDistScalingNearScale[i]);
                        DisplayVariableInspector("          Far Scale", locator, (LocatorHUD.Direction)i, DirectionVariable.DistScalingFarScale, locator.directionDistScalingFarScale[i]);

                        if (locator.directionDistScalingMaxDistance[i] < locator.directionDistScalingMinDistance[i])
                            locator.directionDistScalingMaxDistance[i] = locator.directionDistScalingMinDistance[i];
                    }
                }
            }
        }
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Displays the inspector for a direction variable.
    /// </summary>
    protected void DisplayVariableInspector(string displayName, 
                                            LocatorHUD locator, 
                                            LocatorHUD.Direction direction, 
                                            DirectionVariable variable, 
                                            float currentValue)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.MinHeight(10));
        float newValue = EditorGUILayout.FloatField(displayName, currentValue);
        if (newValue != currentValue)
            SetDirectionVariable(locator, direction, variable, newValue);
        if (GUILayout.Button("-->", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
            SetDirectionVariableToAll(locator, variable, newValue);
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Displays the inspector for a direction variable.
    /// </summary>
    protected void DisplayVariableInspector(string displayName,
                                            LocatorHUD locator,
                                            LocatorHUD.Direction direction,
                                            DirectionVariable variable,
                                            Color currentValue)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.MinHeight(10));
        Color newValue = EditorGUILayout.ColorField(displayName, currentValue);
        if (newValue != currentValue)
            SetDirectionVariable(locator, direction, variable, newValue);
        if (GUILayout.Button("-->", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
            SetDirectionVariableToAll(locator, variable, newValue);
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Displays the inspector for a direction variable.
    /// </summary>
    protected void DisplayVariableInspector(string displayName,
                                            LocatorHUD locator,
                                            LocatorHUD.Direction direction,
                                            DirectionVariable variable,
                                            bool currentValue)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.MinHeight(10));
        bool newValue = EditorGUILayout.Toggle(displayName, currentValue);
        if (newValue != currentValue)
            SetDirectionVariable(locator, direction, variable, newValue);
        if (GUILayout.Button("-->", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
            SetDirectionVariableToAll(locator, variable, newValue);
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Displays the inspector for a direction variable.
    /// </summary>
    protected void DisplayVariableInspector(string displayName,
                                            LocatorHUD locator,
                                            LocatorHUD.Direction direction,
                                            DirectionVariable variable,
                                            Texture2D currentValue)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.MinHeight(10));
        Texture2D newValue = (Texture2D)EditorGUILayout.ObjectField(displayName, currentValue, typeof(Texture2D), false);
        if (newValue != currentValue)
            SetDirectionVariable(locator, direction, variable, newValue);
        if (GUILayout.Button("-->", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
            SetDirectionVariableToAll(locator, variable, newValue);
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Sets a direction variable to a specific direction.
    /// </summary>
    protected void SetDirectionVariable(LocatorHUD locator, LocatorHUD.Direction direction, DirectionVariable variable, object value)
    {
        int index = (int)direction;
        if (index < 0 || index >= (int)LocatorHUD.Direction.NumDirections)
            return;

        switch (variable)
        {
            case DirectionVariable.Rotation:
                locator.directionRotation[index] = (float)value;
                break;
            case DirectionVariable.BaseScale:
                locator.directionBaseScale[index] = (float)value;
                break;
            case DirectionVariable.Color:
                locator.directionColor[index] = (Color)value;
                break;
            case DirectionVariable.DistScalingEnabled:
                locator.directionDistScalingEnabled[index] = (bool)value;
                break;
            case DirectionVariable.DistScalingMinDistance:
                locator.directionDistScalingMinDistance[index] = (float)value;
                break;
            case DirectionVariable.DistScalingMaxDistance:
                locator.directionDistScalingMaxDistance[index] = (float)value;
                break;
            case DirectionVariable.DistScalingNearScale:
                locator.directionDistScalingNearScale[index] = (float)value;
                break;
            case DirectionVariable.DistScalingFarScale:
                locator.directionDistScalingFarScale[index] = (float)value;
                break;
            case DirectionVariable.Texture:
                locator.directionTexture[index] = (Texture2D)value;
                break;
            default:
                break;
        }

        EditorUtility.SetDirty(target);
    }

    /// <summary>
    /// Sets a direction variable to all direction.
    /// </summary>
    protected void SetDirectionVariableToAll(LocatorHUD locator, DirectionVariable variable, object value)
    {
        for (int i = 0; i < (int)LocatorHUD.Direction.NumDirections; i++)
        {
            SetDirectionVariable(locator, (LocatorHUD.Direction)i, variable, value);
        }
    }

    #endregion
}
