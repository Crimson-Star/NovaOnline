using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    // Variáveis públicas que serão configuradas no Unity
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

    // Variáveis privadas
    private float lastClick;
    private float desiredZoom;
    private Quaternion desiredRotation;
    private bool doubleClicked;
    private Vector3 prevPos;
    private CinemachineTransposer offset;

    private void Start()
    {
        // Obtém o componente CinemachineTransposer da câmera virtual
        offset = virtualCam.GetCinemachineComponent<CinemachineTransposer>();

        // Obtém a câmera principal
        mainCam = Camera.main;

        // Define a rotação inicial do objeto
        focalPoint.eulerAngles = new Vector3(45, 0, 0);

        // Define o zoom inicial
        desiredZoom = -30;
    }

    // Update é chamado uma vez por frame
    private void Update()
    {
        // Posiciona o objeto na posição do jogador
        focalPoint.position = player.transform.position;

        if (!controls.activeInventory)
        {
            // Verifica se o botão direito do mouse foi pressionado
            if (Input.GetMouseButtonDown(1))
            {
                // Verifica se foi um duplo clique
                if (Time.time - lastClick <= clickDelay)
                {
                    // Deixa explícito que houve clique duplo
                    doubleClicked = true;

                    // Coloca a câmera na sua posição inicial padrão
                    desiredRotation = Quaternion.Euler(45, 0, 0);
                    desiredZoom = -30;
                }
                lastClick = Time.time;
            }
            // Verifica se só o botão direito do mouse está sendo pressionado e não houve duplo clique
            else if (Input.GetMouseButton(1) && (!doubleClicked && !(Input.GetMouseButton(0) || Input.GetMouseButton(2))))
            {
                // Obtém a direção do movimento do mouse
                Vector3 direction = prevPos - mainCam.ScreenToViewportPoint(Input.mousePosition);

                float xDir = direction.x * 180;
                float yDir = direction.y * 180;
                if (yDir > 0.9f) { yDir = 0.9f; }
                if (yDir < -0.9f) { yDir = -0.9f; }

                float rotX = focalPoint.eulerAngles.x + yDir * xCamSpeed;

                // Limitando a rotação em X
                if (rotX > maxXRot) { rotX = maxXRot; }
                if (rotX < minXRot) { rotX = minXRot; }
                desiredRotation = Quaternion.Euler(rotX, focalPoint.eulerAngles.y - xDir * yCamSpeed, 0);
            }
            if (Input.GetMouseButtonUp(1))
            {
                doubleClicked = false;
            }
            prevPos = mainCam.ScreenToViewportPoint(Input.mousePosition);

            // Obtém o valor do scroll do mouse
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

        // Obtém o zoom atual
        float actualZoom = offset.m_FollowOffset.z;

        // Aplica a rotação e o zoom desejados com suavidade
        focalPoint.rotation = Quaternion.Slerp(focalPoint.rotation, desiredRotation, smoothness);
        offset.m_FollowOffset = Vector3.Lerp(new Vector3(0, 0, actualZoom), new Vector3(0, 0, desiredZoom), smoothness);
    }
}