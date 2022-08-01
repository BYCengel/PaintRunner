using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public ParticleSystem drawingParticle;
    public ParticleSystem erasingParticle;

    public Camera mainCamera;
    public BrushSample brush;

    public InkBar inkBar;

    private AudioManager audioManager;
    private LineRenderer currentLineRenderer;
    private BrushSample brushInstance;
    private List<Vector2> pathPoints = new List<Vector2>();

    Vector2 lastPosition;

    private void Start() {
        lastPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mainCamera = Camera.main;
        audioManager = FindObjectOfType<AudioManager>();
        inkBar = FindObjectOfType<InkBar>();
    }

    private void Update() {

        if(Input.GetMouseButtonDown(1)){
            Erase();
        }else{
            Drawing();
        }
        
    }

    private void Drawing(){
        if(inkBar.DoeshaveInk()){
            if(Input.GetMouseButtonDown(0)){

                pathPoints.Clear();
                CreateBrush();
                // ses başlat
                audioManager.Play("draw");
                

            }else if(Input.GetMouseButton(0)){

                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                drawingParticle.transform.position = new Vector2(mousePosition.x, mousePosition.y);
                drawingParticle.Play();
                if(mousePosition != lastPosition){
                    
                    if(currentLineRenderer != null){
                        AddAPoint(mousePosition);
                        pathPoints.Add(lastPosition);
                        pathPoints.Add(mousePosition);
                        lastPosition = mousePosition;
                    }
                }
                inkBar.DecreaseInk();

            }else{
                
                drawingParticle.Stop();
                //ses durdur
                audioManager.Stop("draw");                
                
                if(currentLineRenderer!= null){
                    brushInstance.SetPathForCollider(pathPoints);
                }
                currentLineRenderer = null;

            }
        } else{
            drawingParticle.Stop();
            audioManager.Stop("draw");

            if (currentLineRenderer != null){
                brushInstance.SetPathForCollider(pathPoints);
                currentLineRenderer = null;
            }
        }
        

    }

    private void Erase(){//erases all drawings in the game

        erasingParticle.transform.position = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
            mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        erasingParticle.Play();
        audioManager.Play("pop");

        BrushSample[] eraseArray = FindObjectsOfType<BrushSample>();

        for(int i = 0; i < eraseArray.Length; i++){
            eraseArray[i].Erase();
        }
    }

    private void CreateBrush(){
        brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePosition);
        currentLineRenderer.SetPosition(1, mousePosition);
        lastPosition = mousePosition;
    }

    private void AddAPoint(Vector2 pointPosition){

        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPosition);

    }
}
