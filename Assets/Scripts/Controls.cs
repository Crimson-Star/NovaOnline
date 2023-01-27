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

    // Valor para decidir se "InventoryUI" ser� ou n�o ser� exibido
    public bool activeInventory;

    private void Awake()
    {
        // Posiciona o destinyHud na posi��o do jogador
        destinyHud.transform.position = player.transform.position;

        // Ignora colis�es entre a camada "player" e a camada "prop"
        Physics.IgnoreLayerCollision(3, 9);

        clickedObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { activeInventory = !activeInventory; inventory.SetActive(activeInventory); }

        // Cria um raio a partir da camera at� a posi��o do mouse no plano 3D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycast))
        {
            hoverObj = raycast.collider.gameObject;
            // Posiciona o cursor na posi��o do raio
            transform.position = raycast.point;

            if (!EventSystem.current.IsPointerOverGameObject() && !activeInventory)
            {
                // Verifica se o bot�o esquerdo do mouse foi pressionado e n�o houve pressionamento de outros bot�es
                if (Input.GetMouseButton(0) && !(Input.GetMouseButton(1) || Input.GetMouseButton(2)))
                {

                    // Verifica se a camada do objeto atingido pelo raio n�o est� na lista de camadas ignoradas
                    if (!rayIgnore.Contains(hoverObj.layer))
                    {
                        // Posiciona o destinyHud na posi��o do raio
                        destinyHud.transform.position = raycast.point;
                        destinyHud.transform.Translate(Vector3.forward * -0.2f);
                        // Inicia a anima��o do destinyHud
                        destinyHud.GetComponent<Animator>().SetBool("IsMoving", true);
                    }
                }
                else
                {
                    // Para a anima��o do destinyHud
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
