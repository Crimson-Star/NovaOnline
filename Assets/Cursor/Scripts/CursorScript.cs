using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    [Header("References:")]
    public GameObject player;
    public Camera UICamera;
    public Interface inter;

    [Header("Textures:")]
    // Sprite do cursor normal
    public Texture2D cursorUpTexture;

    // Sprite do cursor clicando (para interagir)
    public Texture2D cursorDownTexture;

    // Sprite do cursor segurando botão esquerdo (para girar a tela)
    public Texture2D cursorGrabTexture;

    // Ponto exato de clique do cursor
    private Vector2 cursorHotSpot;

    private Transform content;
    private GameObject contextMenu;
    private GameObject carry;
    private Image image;
    private Text amount;

    private void Awake()
    {
        // Define o ponto de clique do cursor
        cursorHotSpot = new Vector2(0, 0);

        // Define o cursor padrão
        Cursor.SetCursor(cursorUpTexture, cursorHotSpot, CursorMode.Auto);

        content = this.transform.Find("Content");

        contextMenu = content.Find("Context Menu").gameObject;

        carry = content.Find("Carry").gameObject;

        image = carry.transform.Find("Image").GetComponent<Image>();

        amount = carry.transform.Find("Amount").GetComponent<Text>();
    }

    // Update é chamado uma vez por frame
    private void Update()
    {

        this.transform.position = UICamera.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 10);

        // Altera o cursor de acordo com o botão pressionado
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorDownTexture, cursorHotSpot, CursorMode.Auto);
        }
        else if (Input.GetMouseButton(1))
        {
            Cursor.SetCursor(cursorGrabTexture, cursorHotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorUpTexture, cursorHotSpot, CursorMode.Auto);
        }

        content.position = UICamera.ScreenToWorldPoint(Input.mousePosition);
        content.position = new Vector3(content.position.x, content.position.y, 0);

        try
        {
            image.sprite = inter.carrying.item.icon;
            image.color = new Color(1, 1, 1, 1);
            if (inter.carrying.amount > 1) { amount.text = inter.carrying.amount.ToString(); }
            else { amount.text = ""; }
        }
        catch
        {
            image.color = new Color(0, 0, 0, 0);
            amount.text = "";
        }
    }
}