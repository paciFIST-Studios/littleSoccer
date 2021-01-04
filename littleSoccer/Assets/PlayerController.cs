
using UnityEngine;
using UnityEngine.UI;

using Mirror;

public class PlayerController : NetworkBehaviour
{
    public Text playerNameText;

    public float speed = 10.0f;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName = "Guest";

    [SyncVar(hook = nameof(OnNameColorChanged))]
    public Color playerNameColor;


    // NOTE: params are ignored here
    private void OnNameChanged(string _old, string _new)
    {
        //print("OnNameChanged(\"" + _old + "\", \"" + _new + "\")");
        playerNameText.text = playerName;
        //print("playerNameText.text = \"" + playerNameText.text + "\";");
    }

    private void OnNameColorChanged(Color _old, Color _new)
    {
        playerNameText.color = _new;
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -5);

        playerNameText = this.GetComponentInChildren<Text>();
        if(playerNameText)
        {
            var pos = playerNameText.rectTransform.position;
            pos = new Vector3(0, 30, 0);
            playerNameText.rectTransform.position = pos;
        }
        else
        {
            print("Could not find Player Name Text Object!");
        }

        string name = "Player " + Random.Range(0, 99).ToString() ;

        Color color = new Color(
              Random.Range(0f, 1f)
            , Random.Range(0f, 1f)
            , Random.Range(0f, 1f));

        CmdSetupPlayer(name, color);
        print("Player Setup Success!");
    }

    [Command]
    public void CmdSetupPlayer(string name, Color color)
    {
        print("Setting Player Name: \"" + name + "\";");
        //print("Setting Player Name Color: <" + color.r + "," + color.g + "," + color.b + "," + color.a + ">");
        // sent to server, which syncs to clients
        playerName = name;
        playerNameColor = color;
    }


    void Update()
    {
        if(!isLocalPlayer) { return; }

        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(moveX, moveY, 0);


        var screen = Camera.main.WorldToScreenPoint(transform.position);
        var pos = playerNameText.rectTransform.position;
        pos = screen;
        pos.y += 30;
        playerNameText.rectTransform.position = pos;

    }
}
