using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to display a directional indicator on the HUD for the game object this script is added to.
/// </summary>
[AddComponentMenu("Locator HUD/Locator HUD Modern")]
public class LocatorHUDModern : MonoBehaviour
{
    #region Enums

    /// <summary>
    /// This enumeration is used to identify different directions for the locator hud
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Indicates the locator hud should be shown on the left of the screen.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Indicates the locator hud should be shown on the right of the screen.
        /// </summary>
        Right,

        /// <summary>
        /// Indicates the locator hud should be shown on the up of the screen.
        /// </summary>
        Up,

        /// <summary>
        /// Indicates the locator hud should be shown on the down of the screen.
        /// </summary>
        Down,

        /// <summary>
        /// Indicates the locator hud should be shown on the top left of the screen.
        /// </summary>
        Left_Up,

        /// <summary>
        /// Indicates the locator hud should be shown on the top right of the screen.
        /// </summary>
        Right_Up,

        /// <summary>
        /// Indicates the locator hud should be shown on the bottom left of the screen.
        /// </summary>
        Left_Down,

        /// <summary>
        /// Indicates the locator hud should be shown on the bottom right of the screen.
        /// </summary>
        Right_Down,

        /// <summary>
        /// Indicates the locator hud should be shown on the screen.
        /// </summary>
        On_Screen,

        /// <summary>
        /// This is the number of directions that can be displayed
        /// </summary>
        NumDirections
    }

    #endregion

    #region Public Member Variables

    /// <summary>
    /// This indicates if this locator hud is currently visible and should be displayed to the screen.
    /// </summary>
    public bool isVisible = true;

    /// <summary>
    /// This is the horizontal spacing in pixels to use on the left and right of the screen.
    /// </summary>
    public int horizontalSpacing = 10;

    /// <summary>
    /// This is the vertical spacing in pixels to use on the top and bottom of the screen.
    /// </summary>
    public int verticalSpacing = 10;

    /// <summary>
    /// This is the speed at which the alpha of this locator hud fades per second in from 0.0 alpha to 1.0 alpha.
    /// </summary>
    public float fadeInSpeed = 2.0f;

    /// <summary>
    /// This is the speed at which the alpha of this locator hud fades per second out from 1.0 alpha to 0.0 alpha.
    /// </summary>
    public float fadeOutSpeed = 4.0f;

    /// <summary>
    /// This is the base alpha of this locator hud.  The final alpha is calculated as follows: baseAlpha * m_fCurrentAlpha
    /// </summary>
    public float baseAlpha = 1.0f;

    /// <summary>
    /// This is an array by each direction that indicates if that direction is enabled.
    /// </summary>
    public bool[] directionEnabled = new bool[(int)Direction.NumDirections];

    /// <summary>
    /// This is an array by each direction that indicates the rotation of that direction gui element.
    /// </summary>
    public float[] directionRotation = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This is an array by each direction that indicates the base scale of that direction gui element.
    /// </summary>
    public float[] directionBaseScale = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This is an array by each direction that indicates the color of that direction gui element.
    /// </summary>
    public Color[] directionColor = new Color[(int)Direction.NumDirections];

    /// <summary>
    /// This is an array by each direction that indicates if the scaling by distance is enabled for that direction gui element.
    /// For example:
    /// MinDist = 1.0f
    /// MaxDist = 5.0f
    /// NearScale = 1.0f
    /// FarScale = 0.5f
    /// AND
    /// CurrentDistToObject = 4.0f
    /// THEN
    /// CurrentScale = 0.625f 
    /// OR
    /// CurrentDistToObject = 3.0f
    /// THEN
    /// CurrentScale = 0.75f 
    /// </summary>
    public bool[] directionDistScalingEnabled = new bool[(int)Direction.NumDirections];

    /// <summary>
    /// This indicates the min distance for each direction to determine how that element scales based on distance.
    /// </summary>
    public float[] directionDistScalingMinDistance = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This indicates the max distance for each direction to determine how that element scales based on distance.
    /// </summary>
    public float[] directionDistScalingMaxDistance = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This indicates the near scale for each direction to determine how that element scales based on distance.
    /// </summary>
    public float[] directionDistScalingNearScale = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This indicates the far scale for each direction to determine how that element scales based on distance.
    /// </summary>
    public float[] directionDistScalingFarScale = new float[(int)Direction.NumDirections];

    /// <summary>
    /// This is the texture to use for each direction.
    /// </summary>
    public Texture2D[] directionTexture = new Texture2D[(int)Direction.NumDirections];

    #endregion

    #region Private Member Variables

    /// <summary>
    /// This is the last screen position this locator hud was drawn at.
    /// </summary>
    private Vector3 m_oLastScreenPos = Vector3.zero;

    /// <summary>
    /// This is the last direction that was drawn for this locator hud.
    /// </summary>
    private Direction m_eLastDirection = Direction.On_Screen;

    /// <summary>
    /// This was the horizontal offset the last time this locator hud was drawn.
    /// </summary>
    private Direction m_eLastHorizontalOffset = Direction.On_Screen;

    /// <summary>
    /// This was the vertical offset the last time this locator hud was drawn.
    /// </summary>
    private Direction m_eLastVerticalOffset = Direction.On_Screen;

    /// <summary>
    /// This is the current alpha for this locator hud.
    /// </summary>
    private float m_fCurrentAlpha = 0.0f;

    #endregion

    #region Init/Deinit Functions

    /// <summary>
    /// This is called when this script needs to be reset.
    /// </summary>

    public Camera cam;
    public void Reset()
    {
        isVisible = true;

        horizontalSpacing = 10;
        verticalSpacing = 10;
        fadeInSpeed = 2.0f;
        fadeOutSpeed = 4.0f;
        baseAlpha = 1.0f;

        for (int i = 0; i < (int)Direction.NumDirections; i++)
        {
            directionEnabled[i] = true;
            directionRotation[i] = 0.0f;
            directionBaseScale[i] = 1.0f;
            directionColor[i] = Color.white;

            directionDistScalingEnabled[i] = false;
            directionDistScalingMinDistance[i] = 2.0f;
            directionDistScalingMaxDistance[i] = 100.0f;
            directionDistScalingNearScale[i] = 2.0f;
            directionDistScalingFarScale[i] = 0.25f;
        }

        directionEnabled[(int)Direction.On_Screen] = false;

        directionTexture[(int)Direction.Left] = (Texture2D)Resources.Load("LeftIndicator");
        directionTexture[(int)Direction.Right] = (Texture2D)Resources.Load("RightIndicator");
        directionTexture[(int)Direction.Up] = (Texture2D)Resources.Load("UpIndicator");
        directionTexture[(int)Direction.Down] = (Texture2D)Resources.Load("DownIndicator");
        directionTexture[(int)Direction.Left_Up] = (Texture2D)Resources.Load("LeftIndicator");
        directionTexture[(int)Direction.Right_Up] = (Texture2D)Resources.Load("UpIndicator");
        directionTexture[(int)Direction.Left_Down] = (Texture2D)Resources.Load("DownIndicator");
        directionTexture[(int)Direction.Right_Down] = (Texture2D)Resources.Load("RightIndicator");
        directionTexture[(int)Direction.On_Screen] = (Texture2D)Resources.Load("OnScreenIndicator");
        
        directionRotation[(int)Direction.Left_Up] = 45.0f;
        directionRotation[(int)Direction.Right_Up] = 45.0f;
        directionRotation[(int)Direction.Left_Down] = 45.0f;
        directionRotation[(int)Direction.Right_Down] = 45.0f;
    }

    /// <summary>
    /// This is called when this script is enabled.
    /// </summary>
    void OnEnable()
    {
        m_fCurrentAlpha = 0.0f;
    }

    /// <summary>
    /// This is called when this script is disabled.
    /// </summary>
    void OnDisable()
    {
        m_fCurrentAlpha = 0.0f;
    }

    #endregion

    #region Accessor Functions

    /// <summary>
    /// This will show this locator hud and fade it in.
    /// </summary>
    public void Show()
    {
        isVisible = true;
    }

    /// <summary>
    /// This will hide this locator hud and fade it out.
    /// </summary>
    public void Hide()
    {
        isVisible = false;
    }

    /// <summary>
    /// This will set the color of all directions for this locator hud.
    /// </summary>
    /// <param name="color">The color it should be set to.</param>
    public void SetColorToAll(Color color)
    {
        for (int i = 0; i < (int)Direction.NumDirections; i++)
        {
            directionColor[i] = color;
        }
    }

    #endregion

    #region Utility Functions

    /// <summary>
    /// This maps a distance to scale using the passed in params.
    /// </summary>
    /// <param name="distance">The current distance to map.</param>
    /// <param name="distanceMin">The min distance that should be scaled from.</param>
    /// <param name="distanceMax">The max distance that should be scaled to.</param>
    /// <param name="scaleNear">The scale for objects closer to the min distance.</param>
    /// <param name="scaleFar">The scale for objects closer to the max distance.</param>
    /// <returns>The remapped scale</returns>
    protected static float MapDistanceToScale(float distance, float distanceMin, float distanceMax, float scaleNear, float scaleFar)
    {
        //calc the percentage
        float percentage = 0.0f;
        if (distance <= distanceMin)
            percentage = 0.0f;
        else if (distance >= distanceMax)
            percentage = 1.0f;
        else
            percentage = (distance - distanceMin) / (distanceMax - distanceMin);

        //return the new value
        return Mathf.Lerp(scaleNear, scaleFar, percentage);
    }

    #endregion

    #region GUI Functions

    /// <summary>
    /// This gets the basic viewport position for this object.
    /// </summary>
    /// <returns>The basic viewport position.</returns>
    protected Vector3 GetViewportPosBasic()
    {
        return cam.WorldToViewportPoint(transform.position);
    }

    /// <summary>
    /// This gets the advanced viewport position for this object using horizontal and vertical angle
    /// towards this object from the current camera.
    /// </summary>
    /// <param name="lerpAmount">This is how much to lerp from the current viewport pos to this advanced viewport pos. 0.0 to 1.0.</param>
    /// <param name="currentViewportPos">This is the current viewport pos to lerp from.</param>
    /// <returns>The advanced viewport position.</returns>
    protected Vector3 GetViewportPosAdvanced(float lerpAmount, Vector3 currentViewportPos)
    {
        //get rotated dir from camera
        Vector3 dirFromCamera = (transform.position - cam.transform.position).normalized;
        Quaternion dirQuat = Quaternion.LookRotation(dirFromCamera);
        float horizAngle = Mathf.DeltaAngle(cam.transform.eulerAngles.y, dirQuat.eulerAngles.y);
        float vertAngle = -Mathf.DeltaAngle(cam.transform.eulerAngles.x, dirQuat.eulerAngles.x);

        //calculate horizontal perc
        float horizFov = cam.fieldOfView * cam.aspect;
        float horizPerc = 0.0f;
        if (horizAngle < -(horizFov * 0.5f))
            horizPerc = 0.0f;
        else if (horizAngle > (horizFov * 0.5f))
            horizPerc = 1.0f;
        else
            horizPerc = (horizAngle + horizFov * 0.5f) / horizFov;

        //calculate vertical perc
        float vertFov = cam.fieldOfView;
        float vertPerc = 0.0f;
        if (vertAngle < -(vertFov * 0.5f))
            vertPerc = 0.0f;
        else if (vertAngle > (vertFov * 0.5f))
            vertPerc = 1.0f;
        else
            vertPerc = (vertAngle + vertFov * 0.5f) / vertFov;

        //lerp between the viewport positions
        return Vector3.Lerp(currentViewportPos, new Vector3(horizPerc, vertPerc, currentViewportPos.z), lerpAmount);
    }

    /// <summary>
    /// This is called when the gui for this script should be displayed.
    /// </summary>
    protected void OnGUI()
    {
        //make srue there is a current camera
        if (cam != null)
        {
            //make sure this is visible
            if (!isVisible && m_fCurrentAlpha <= 0.0f)
                return;

            //get the basic viewport pos
            Vector3 viewportPos = GetViewportPosBasic();

            //gets advanced viewport pos if needed
            const float beginThreshold = 0.3f;
            const float endThreshold = 1.0f;
            const float thresholdRange = endThreshold - beginThreshold;
            if (viewportPos.z < 0.0f)
            {
                viewportPos = GetViewportPosAdvanced(1.0f, viewportPos);
            }
            else if (viewportPos.x < -beginThreshold)
            {
                if (viewportPos.x <= -endThreshold)
                    viewportPos = GetViewportPosAdvanced(1.0f, viewportPos);
                else
                    viewportPos = GetViewportPosAdvanced(1.0f - ((endThreshold - Mathf.Abs(viewportPos.x)) / thresholdRange), viewportPos);
            }
            else if (viewportPos.x > 1.0f + beginThreshold)
            {
                if (viewportPos.x >= 1.0f + endThreshold)
                    viewportPos = GetViewportPosAdvanced(1.0f, viewportPos);
                else
                    viewportPos = GetViewportPosAdvanced(1.0f - ((endThreshold - (viewportPos.x - 1.0f)) / thresholdRange), viewportPos);
            }
            else if (viewportPos.y < -beginThreshold)
            {
                if (viewportPos.y <= -endThreshold)
                    viewportPos = GetViewportPosAdvanced(1.0f, viewportPos);
                else
                    viewportPos = GetViewportPosAdvanced(1.0f - ((endThreshold - Mathf.Abs(viewportPos.y)) / thresholdRange), viewportPos);
            }
            else if (viewportPos.y > 1.0f + beginThreshold)
            {
                if (viewportPos.y >= 1.0f + endThreshold)
                    viewportPos = GetViewportPosAdvanced(1.0f, viewportPos);
                else
                    viewportPos = GetViewportPosAdvanced(1.0f - ((endThreshold - (viewportPos.y - 1.0f)) / thresholdRange), viewportPos);
            }

            //calculate screen position
            Vector3 screenPos = new Vector3(Mathf.Lerp(0.0f, cam.pixelWidth, Mathf.Clamp(viewportPos.x, 0.0f, 1.0f)),
                                            Mathf.Lerp(0.0f, cam.pixelHeight, Mathf.Clamp(viewportPos.y, 0.0f, 1.0f)),
                                            viewportPos.z);

            //get direction and offset screen pos
            Direction direction = Direction.On_Screen;
            Direction horizontalOffset = Direction.On_Screen;
            Direction verticalOffset = Direction.On_Screen;
            if (screenPos.x <= horizontalSpacing)
            {
                if (direction == Direction.On_Screen)
                    direction = Direction.Left;
                horizontalOffset = Direction.Left;
                screenPos.x = 0.0f;
            }
            else if (screenPos.x >= cam.pixelWidth - horizontalSpacing)
            {
                if (direction == Direction.On_Screen)
                    direction = Direction.Right;
                horizontalOffset = Direction.Right;
                screenPos.x = cam.pixelWidth;
            }
            if (screenPos.y <= verticalSpacing)
            {
                if (direction == Direction.On_Screen)
                    direction = Direction.Down;
                verticalOffset = Direction.Down;
                screenPos.y = 0.0f;
            }
            if (screenPos.y >= cam.pixelHeight - verticalSpacing)
            {
                if (direction == Direction.On_Screen)
                    direction = Direction.Up;
                verticalOffset = Direction.Up;
                screenPos.y = cam.pixelHeight;
            }

            //update the direction if it is in the corner
            if (horizontalOffset == Direction.Left && verticalOffset == Direction.Up && directionEnabled[(int)Direction.Left_Up])
                direction = Direction.Left_Up;
            else if (horizontalOffset == Direction.Right && verticalOffset == Direction.Up && directionEnabled[(int)Direction.Right_Up])
                direction = Direction.Right_Up;
            else if (horizontalOffset == Direction.Left && verticalOffset == Direction.Down && directionEnabled[(int)Direction.Left_Down])
                direction = Direction.Left_Down;
            else if (horizontalOffset == Direction.Right && verticalOffset == Direction.Down && directionEnabled[(int)Direction.Right_Down])
                direction = Direction.Right_Down;

            //check if the current direction is enabled and this locator hud is visibled
            if (isVisible && directionEnabled[(int)direction])
            {//this is visible and the direction is enabled
                //set last displayed info
                m_oLastScreenPos = screenPos;
                m_eLastDirection = direction;
                m_eLastHorizontalOffset = horizontalOffset;
                m_eLastVerticalOffset = verticalOffset;

                //fade in if needed
                if (m_fCurrentAlpha < 1.0f)
                {
                    m_fCurrentAlpha += fadeInSpeed * Time.deltaTime;
                    if (m_fCurrentAlpha > 1.0f)
                        m_fCurrentAlpha = 1.0f;
                }

                //display the directions
                DisplayDirection(screenPos, direction, horizontalOffset, verticalOffset);
            }
            //check if the current alpha makes this locator hud still visible
            else if (m_fCurrentAlpha > 0.0f)
            {
                //fade out
                m_fCurrentAlpha -= fadeOutSpeed * Time.deltaTime;
                if (m_fCurrentAlpha <= 0.0f)
                {//done fading out, make sure alpha is 0.0
                    m_fCurrentAlpha = 0.0f;
                }
                else
                {//still fading out, display at last pos
                    DisplayDirection(m_oLastScreenPos, m_eLastDirection, m_eLastHorizontalOffset, m_eLastVerticalOffset);
                }
            }
        }
    }

    /// <summary>
    /// This displays a specific direction for this locator hud.
    /// </summary>
    /// <param name="screenPos">The screen position to display this hud to.</param>
    /// <param name="direction">The direction to display.</param>
    /// <param name="horizontalOffset">The horizontal offset to use for displaying the locator hud.</param>
    /// <param name="verticalOffset">The vertical offset to use for displaying the locator hud.</param>
    protected void DisplayDirection(Vector3 screenPos, Direction direction, Direction horizontalOffset, Direction verticalOffset)
    {
        //get the index, color, texture, and size
        int index = (int)direction;
        Texture2D texture = directionTexture[index];
        Color color = directionColor[index];
        Vector2 size = new Vector2(texture.width, texture.height);
        size *= directionBaseScale[index];

        //check if dist scaling is enabled
        if (directionDistScalingEnabled[index])
        {
            //update distance to be more accurate
            screenPos.z = Vector3.Distance(cam.transform.position, transform.position);

            //scale dist
            size *= MapDistanceToScale(Mathf.Abs(screenPos.z),
                                       directionDistScalingMinDistance[index],
                                       directionDistScalingMaxDistance[index],
                                       directionDistScalingNearScale[index],
                                       directionDistScalingFarScale[index]);
        }

        //calc half size
        Vector2 halfSize = size / 2.0f;

        //calc offset
        Vector2 offset = Vector2.zero;
        if (horizontalOffset == Direction.Left)
            offset = new Vector2(horizontalSpacing + halfSize.x, 0.0f);
        else if (horizontalOffset == Direction.Right)
            offset = new Vector2(-horizontalSpacing - halfSize.x, 0.0f);
        if (verticalOffset == Direction.Up)
            offset = new Vector2(offset.x, verticalSpacing + halfSize.y);
        else if (verticalOffset == Direction.Down)
            offset = new Vector2(offset.x, -verticalSpacing - halfSize.y);

        //get the rect to display this locator hud to
        Rect rect = new Rect(screenPos.x - halfSize.x + offset.x,
                             (cam.pixelHeight - screenPos.y) - halfSize.y + offset.y,
                             size.x,
                             size.y);

        //offset the rectangle if needed
        if (direction != Direction.On_Screen)
        {
            if (rect.xMin < horizontalSpacing)
                rect.x += horizontalSpacing - rect.xMin;
            else if (rect.xMax > cam.pixelWidth - horizontalSpacing)
                rect.x -= rect.xMax - (cam.pixelWidth - horizontalSpacing);
            if (rect.yMin < verticalSpacing)
                rect.y += verticalSpacing - rect.yMin;
            else if (rect.yMax > cam.pixelHeight - verticalSpacing)
                rect.y -= rect.yMax - (cam.pixelHeight - verticalSpacing);
        }

        //rotate this gui if needed.
        Matrix4x4 oldMat = GUI.matrix;
        if (directionRotation[index] > 0.0f || directionRotation[index] < 360.0f)
        {
            GUIUtility.RotateAroundPivot(directionRotation[index], new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f));
        }

        //draw this gui element
        GUI.BeginGroup(rect);
        GUI.color = new Color(color.r, color.g, color.b, m_fCurrentAlpha * baseAlpha);
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), texture);
        GUI.EndGroup();

        //restore old matrix for gui
        GUI.matrix = oldMat;
    }

    #endregion
}
