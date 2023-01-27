using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    // Vari�veis p�blicas que ser�o configuradas no Unity
    public Transform player;
    public Transform focalPoint;

    [Space]

    public Camera mainCam;
    public CinemachineVirtualCamera virtualCam;
    public Controls controls;

    [Space]

    public float minXRot;
    public float maxXRot;
    public float xCamSpeed;
    public float yCamSpeed;
    public float zoomSpeed;
    public float zoomMin;
    public float zoomMax;
    public float smoothness;
    public float clickDelay;

    // Vari�veis privadas
    private float lastClick;
    private float desiredZoom;
    private Quaternion desiredRotation;
    private bool doubleClicked;
    private Vector3 prevPos;
    private CinemachineTransposer offset;

    private void Start()
    {
        // Obt�m o componente CinemachineTransposer da c�mera virtual
        offset = virtualCam.GetCinemachineComponent<CinemachineTransposer>();

        // Obt�m a c�mera principal
        mainCam = Camera.main;

        // Define a rota��o inicial do objeto
        focalPoint.eulerAngles = new Vector3(45, 0, 0);

        // Define o zoom inicial
        desiredZoom = -30;
    }

    // Update � chamado uma vez por frame
    private void Update()
    {
        // Posiciona o objeto na posi��o do jogador
        focalPoint.position = player.transform.position;

        if (!controls.activeInventory)
        {
            // Verifica se o bot�o direito do mouse foi pressionado
            if (Input.GetMouseButtonDown(1))
            {
                // Verifica se foi um duplo clique
                if (Time.time - lastClick <= clickDelay)
                {
                    // Deixa expl�cito que houve clique duplo
                    doubleClicked = true;

                    // Coloca a c�mera na sua posi��o inicial padr�o
                    desiredRotation = Quaternion.Euler(45, 0, 0);
                    desiredZoom = -30;
                }
                lastClick = Time.time;
            }
            // Verifica se s� o bot�o direito do mouse est� sendo pressionado e n�o houve duplo clique
            else if (Input.GetMouseButton(1) && (!doubleClicked && !(Input.GetMouseButton(0) || Input.GetMouseButton(2))))
            {
                // Obt�m a dire��o do movimento do mouse
                Vector3 direction = prevPos - mainCam.ScreenToViewportPoint(Input.mousePosition);

                float xDir = direction.x * 180;
                float yDir = direction.y * 180;
                if (yDir > 0.9f) { yDir = 0.9f; }
                if (yDir < -0.9f) { yDir = -0.9f; }

                float rotX = focalPoint.eulerAngles.x + yDir * xCamSpeed;

                // Limitando a rota��o em X
                if (rotX > maxXRot) { rotX = maxXRot; }
                if (rotX < minXRot) { rotX = minXRot; }
                desiredRotation = Quaternion.Euler(rotX, focalPoint.eulerAngles.y - xDir * yCamSpeed, 0);
            }
            if (Input.GetMouseButtonUp(1))
            {
                doubleClicked = false;
            }
            prevPos = mainCam.ScreenToViewportPoint(Input.mousePosition);

            // Obt�m o valor do scroll do mouse
            float scroll = Input.mouseScrollDelta.y;

            // Ajusta o zoom desejado baseado no scroll do mouse
            if (scroll != 0)
            {
                if (scroll > 0 && desiredZoom <= -zoomMax)
                {
                    desiredZoom += zoomSpeed;
                }
                if (scroll < 0 && desiredZoom >= -zoomMin)
                {
                    desiredZoom -= zoomSpeed;
                }
            }
        }

        // Obt�m o zoom atual
        float actualZoom = offset.m_FollowOffset.z;

        // Aplica a rota��o e o zoom desejados com suavidade
        focalPoint.rotation = Quaternion.Slerp(focalPoint.rotation, desiredRotation, smoothness);
        offset.m_FollowOffset = Vector3.Lerp(new Vector3(0, 0, actualZoom), new Vector3(0, 0, desiredZoom), smoothness);
    }
}