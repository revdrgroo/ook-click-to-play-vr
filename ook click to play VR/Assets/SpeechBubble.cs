using UnityEngine;  
using System.Collections;  

[ExecuteInEditMode]  
public class SpeechBubble : MonoBehaviour  
{  
  private string bubbleText = "Bubble Text";

  //this game object's transform  
  private Transform goTransform;  
  //the game object's position on the screen, in pixels  
  private Vector3 goScreenPos;  
  private Vector3 headScreenPos;
  private bool leftBubble;
  //the game objects position on the screen  
  private Vector3 goViewportPos;  

  //the width of the speech bubble  
  public int bubbleWidth = 200;  
  //the height of the speech bubble  
  public int bubbleHeight = 150;  

  //an offset, to better position the bubble  
  public float offsetX = 0;  
  public float offsetY = 0;  

  public bool rightBubble = true;

  //an offset to center the bubble  
  private int centerOffsetX;  
  private int centerOffsetY;  

  //a material to render the triangular part of the speech balloon  
  public Material mat;  
  //a guiSkin, to render the round part of the speech balloon  
  public GUISkin guiSkin;  

  //use this for early initialization  
  void Awake ()  
  {  
    //get this game object's transform  
    goTransform = this.GetComponent<Transform>();  
  }  

  //use this for initialization  
  void Start()  
  {  
    //if the material hasn't been found  
    if (!mat)  
    {  
      Debug.LogError("Please assign a material on the Inspector.");  
      return;  
    }  

    //if the guiSkin hasn't been found  
    if (!guiSkin)  
    {  
      Debug.LogError("Please assign a GUI Skin on the Inspector.");  
      return;  
    }  

    //Calculate the X and Y offsets to center the speech balloon exactly on the center of the game object  
    centerOffsetX = bubbleWidth/2;  
    centerOffsetY = bubbleHeight/2;  
  }  

  public void SetText(string text) {
    bubbleText = text;
  }

  //Called once per frame, after the update  
  void LateUpdate()  
  {  
    //find out the position on the screen of this game object  
    goScreenPos = Camera.main.WorldToScreenPoint(goTransform.position); 
    headScreenPos = Camera.main.WorldToScreenPoint(goTransform.position + goTransform.forward);

    leftBubble = (goScreenPos.x > headScreenPos.x) != rightBubble;
    //    Debug.Log("goScreenPos: " + goScreenPos + " headScreenPos: " + headScreenPos + " leftBubble: " + leftBubble);

    //Could have used the following line, instead of lines 70 and 71  
    goViewportPos = Camera.main.WorldToViewportPoint(goTransform.position);
  }  

  //Draw GUIs  
  void OnGUI()  
  {  
    //Begin the GUI group centering the speech bubble at the same position of this game object. After that, apply the offset  
    float xPos = leftBubble ? (bubbleWidth / 2): (-bubbleWidth / 2);
    GUI.BeginGroup(new Rect(
      goScreenPos.x - (centerOffsetX + offsetX) + xPos,
      Screen.height - goScreenPos.y - centerOffsetY - offsetY,
      bubbleWidth,
      bubbleHeight));  

    //Render the round part of the bubble  
    GUI.Label(new Rect(0, 0, bubbleWidth, bubbleHeight),"",guiSkin.customStyles[0]);  

    //Render the text  
    float xMargin = 70;
    float yMargin = 28;
    GUI.Label(new Rect(xMargin, yMargin, bubbleWidth - xMargin * 2, bubbleHeight - yMargin * 2),
      bubbleText, guiSkin.label);  

    GUI.EndGroup();  
  }  

  //Called after camera has finished rendering the scene  
  void OnRenderObject()  
  {  
    //push current matrix into the matrix stack  
    GL.PushMatrix();  
    //set material pass  
    mat.SetPass(0);  
    //load orthogonal projection matrix  
    GL.LoadOrtho();  
    //a triangle primitive is going to be rendered  
    GL.Begin(GL.TRIANGLES);  

    //set the color  
    GL.Color(Color.red);  

    //Define the triangle vetices  
    if (leftBubble) {
      GL.Vertex3(goViewportPos.x, goViewportPos.y , 0.1f);  
      GL.Vertex3(goViewportPos.x + (bubbleWidth/8)/(float)Screen.width, goViewportPos.y+offsetY/Screen.height, 0.1f);  
      GL.Vertex3(goViewportPos.x + (bubbleWidth/3)/(float)Screen.width, goViewportPos.y+offsetY/Screen.height, 0.1f);  
    } else {
      GL.Vertex3(goViewportPos.x, goViewportPos.y , 0.1f);  
      GL.Vertex3(goViewportPos.x - (bubbleWidth/3)/(float)Screen.width, goViewportPos.y+offsetY/Screen.height, 0.1f);  
      GL.Vertex3(goViewportPos.x - (bubbleWidth/8)/(float)Screen.width, goViewportPos.y+offsetY/Screen.height, 0.1f);  
    }

    GL.End();  
    //pop the orthogonal matrix from the stack  
    GL.PopMatrix();  
  }  
}  