using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GatesController : MonoBehaviour
{
    public RectTransform leftGate;
    public RectTransform leftGateEnd;
    public RectTransform rightGate;
    public RectTransform rightGateEnd;
    public bool closed;
    public MenuManager menuManager;
    public Menu menu;

    [SerializeField]
    private float gateOpenSpeed;                // How fast gate will be open 
    [SerializeField]
    private float gateCloseSpeed;               // how fast gate will be close
    [SerializeField]
    private float closedTime = 1;               // how long gates will be closed
    [SerializeField]
    private float openTime = 1;                 // after how many time gate will be open


    private float leftGateBeginX;
    private float rightGateBeginX;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        leftGateEnd.position = new Vector3(leftGateEnd.position.x * 3 / 2, leftGateEnd.position.y, leftGateEnd.position.z);
        rightGateEnd.position = new Vector3(rightGateEnd.position.x * 3 / 2, rightGateEnd.position.y, rightGateEnd.position.z);

        leftGateBeginX = leftGate.transform.position.x;
        rightGateBeginX = rightGate.transform.position.x;

        if (closed)
            BeginFromClosedGates();
        else
            BeginFromOpenedGates();
    }


    public void OpenGates()
    {
        StartCoroutine(IEOpenGates());
    }


    public void CloseGates()
    {
        StartCoroutine(IECloseGates());
    }

    public void CloseGatesAndChangeSceene()
    {
        StartCoroutine(IECloseGatesAndChangeSceene());
    }


    IEnumerator IECloseGatesAndChangeSceene()
    {
        yield return StartCoroutine(IECloseGates());
        yield return new WaitForSeconds(closedTime);

        menu.gameObject.GetComponent<GatesController>().UpdateGateState(false);
        menuManager.ShowMenu(menu);
        GameMaster.instance.ClearScenee();
    }


    IEnumerator IEOpenGatesAfterTime()
    {
        yield return new WaitForSeconds(openTime);
        OpenGates();
    }


    IEnumerator IEOpenGates()
    {
        audioSource.Play();

        while (Mathf.Abs((leftGate.position - leftGateEnd.position).magnitude) > 0.01f)
        {
            leftGate.position = Vector3.Lerp(leftGate.position, leftGateEnd.position, gateOpenSpeed * Time.deltaTime);
            rightGate.position = Vector3.Lerp(rightGate.position, rightGateEnd.position, gateOpenSpeed * Time.deltaTime);
            yield return null;
        }
    }


    IEnumerator IECloseGates()
    {
        Vector3 leftGateBegin = new Vector3(leftGateBeginX, leftGate.position.y, leftGate.position.z);
        Vector3 rightGateBegin = new Vector3(rightGateBeginX, rightGate.position.y, rightGate.position.z);

        audioSource.Play();

        while ((rightGate.position - rightGateBegin).magnitude > 0.1f)
        {
            leftGate.position = Vector3.Lerp(leftGate.position, leftGateBegin, gateCloseSpeed * Time.deltaTime);
            rightGate.position = Vector3.Lerp(rightGate.position, rightGateBegin, gateCloseSpeed * Time.deltaTime);
            yield return null;
        }

        leftGate.position = leftGateBegin;      
        rightGate.position = rightGateBegin;
    }


    public void UpdateGateState(bool open)
    {
        if (open)
            BeginFromOpenedGates();
        else
        {
            BeginFromClosedGates();
            StartCoroutine(IEOpenGatesAfterTime());
        }
    }
    

    void BeginFromClosedGates()
    {
        leftGate.position = new Vector3(leftGateBeginX, leftGate.position.y, leftGate.position.z);
        rightGate.position = new Vector3(rightGateBeginX, rightGate.position.y, rightGate.position.z);
    }


    void BeginFromOpenedGates()
    {
        leftGate.position = new Vector3(leftGateEnd.position.x, leftGate.position.y, leftGate.position.z);
        rightGate.position = new Vector3(rightGateEnd.position.x, rightGate.position.y, rightGate.position.z);
    }
}   // Karol Sobański
