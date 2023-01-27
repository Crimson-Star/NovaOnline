using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    [Header("References:")]
    // Pega o "InventoryUI"
    public GameObject inventory;
    public GameObject player;

    // Local de destino do jogador
    public GameObject destinyHud;

    [Header("Click Ignore Layers:")]
    // Camadas que ignoraremos quando clicarmos
    public List<int> rayIgnore;

    public GameObject clickedObj { get; private set; }
    public GameObject hoverObj { get; private set; }

    [Space]

    // Valor para decidir se "InventoryUI" será ou não será exibido
    public bool activeInventory;

    private void Awake()
    {
        // Posiciona o destinyHud na posição do jogador
        destinyHud.transform.position = player.transform.position;

        // Ignora colisões entre a camada "player" e a camada "prop"
        Physics.IgnoreLayerCollision(3, 9);

        clickedObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { activeInventory = !activeInventory; inventory.SetActive(activeInventory); }

        // Cria um raio a partir da camera até a posição do mouse no plano 3D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycast))
        {
            hoverObj = raycast.collider.gameObject;
            // Posiciona o cursor na posição do raio
            transform.position = raycast.point;

            if (!EventSystem.current.IsPointerOverGameObject() && !activeInventory)
            {
                // Verifica se o botão esquerdo do mouse foi pressionado e não houve pressionamento de outros botões
                if (Input.GetMouseButton(0) && !(Input.GetMouseButton(1) || Input.GetMouseButton(2)))
                {

                    // Verifica se a camada do objeto atingido pelo raio não está na lista de camadas ignoradas
                    if (!rayIgnore.Contains(hoverObj.layer))
                    {
                        // Posiciona o destinyHud na posição do raio
                        destinyHud.transform.position = raycast.point;
                        destinyHud.transform.Translate(Vector3.forward * -0.2f);
                        // Inicia a animação do destinyHud
                        destinyHud.GetComponent<Animator>().SetBool("IsMoving", true);
                    }
                }
                else
                {
                    // Para a animação do destinyHud
                    destinyHud.GetComponent<Animator>().SetBool("IsMoving", false);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    clickedObj = hoverObj;
                }
            }
        }
    }
}
