using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("3 basic objects of character")]
    [SerializeField] private Transform standObject;
    [SerializeField] private Transform swimObject;
    [SerializeField] private Transform lineObject;

    [Header(" ")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool reverse;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider col;

    private Vector3 offset;
    private bool ismove;
    private GameObject goj;
    private Vector3 playerPos;
    private int starNumber;
    private Vector3 originPos;
    private RaycastHit hit;
    private Vector3 stand;
    private Vector3 newpos;
    private bool startClick;

    private bool longer;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        originPos = new Vector3(0, 40, 0);
        starNumber = 0;
        col.enabled = false;
        StartCoroutine(DelayAppear());

        //tat collider
        SetColldier("disable");
        startClick = false;
    }
    private void Update()
    {
        ActionWhenClick();

        if (reverse)
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
        else
        {
            transform.Rotate(-rotateSpeed * Time.deltaTime * Vector3.up);
        }

        if (ismove)
        {
            transform.position = goj.transform.position + offset;        
        }

        if (longer)
        {
            StartCoroutine(SetLonger());

            longer = false;
        }
    }
    public void ActionWhenClick()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.StarGame()) // CHI KHI NAO GAMESTART = TRUE THI MOI NHAN DC
        {
            if(Screen.height <= 1300)
            {
                if (Input.mousePosition.y > 900)
                {
                    return;
                }
            }
            else
            {
                if (Input.mousePosition.y > 1700)
                {
                    return;
                }
            }    
            Debug.Log(Input.mousePosition.y);
            SoundManager.Instance.SoundWhenClick();
            if(!startClick)
            {
                SetColldier("enable");
                startClick = true;
            }

             stand = standObject.position;
             newpos = new Vector3(swimObject.position.x, 0, swimObject.position.z);

            transform.position = newpos;
            playerPos = transform.position;

            swimObject.position = stand;
            swimObject.transform.LookAt(standObject.position);  //swim object alway lookAt stand object for increase/decrease ranger

            lineObject.position = standObject.position;
            lineObject.transform.LookAt(swimObject.position);

            reverse = !reverse; //reverse rotation

            StartCoroutine(CheckPlatform());    //check what platform is standing or moving

            //healping count how many robber was colored last turn
            GameManager.Instance.SetCurrentPointLastPoint();
            GameManager.Instance.ResetCurrentPoint();
            GameManager.Instance.ToggleTextTrigeer();

            CheckGround();  //check what layer player standing
        }
    }
    IEnumerator DelayAppear()   
    {
        yield return new WaitForSeconds(0.2f);
        standObject.gameObject.SetActive(true);
        swimObject.gameObject.SetActive(true);
        lineObject.gameObject.SetActive(true);
    }

    // check current position is ground or water layer. If water => lose game
    public void CheckGround()
    {
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down), out hit,Mathf.Infinity,layer))
        {
            if (hit.collider.gameObject.CompareTag("Water"))
            {
                GameManager.Instance.ResetScene(true);
            }
        }  
    }

    //turn/off collider to check what platform is standed
    IEnumerator CheckPlatform()
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Ground"))
        {
            ismove = false;
            return;
        }
        //if standing move platform => set player position is archored by moving platform
        if (other.gameObject.CompareTag("Move"))
        {
            ismove = true;
            goj = other.gameObject;
            offset = playerPos - other.transform.position;
            return;
        }
    }
    #region calculate ranger when earn star
    public void SetStarNumber() // earn enough star to scale up
    {
        starNumber++;
        if (starNumber == 3)
        {
            StartCoroutine(SetLonger());   // after earn 3 star. player can be longer
        }
    }
    IEnumerator SetLonger()
    {
        SoundManager.Instance.ScaleUpSound();
        float time = 0.5f;
        float scale = 0;
        // scale by time
        while (time > 0)
        {
            scale = scale * 1.1f + Time.fixedDeltaTime;
            if (scale >= 1.8f) scale = 1.8f;

            swimObject.transform.Translate(-5 * Time.fixedDeltaTime * Vector3.forward, Space.Self);
            lineObject.transform.localScale = new Vector3(1, 1, scale);
            time -= Time.fixedDeltaTime;
            yield return null;
        }
        starNumber = 0; // after earn 3 star. set current star = 0 for next level
    }
    #endregion


    #region move to work position and scale up when star each round
    public void GetDown()
    {
        StartCoroutine(MoveDown());
    }
    IEnumerator MoveDown()
    {
        yield return new WaitForSeconds(0.5f);

        SoundManager.Instance.SoundWhenMoveDown();

        while (transform.position.y > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, 5 * Time.deltaTime);
            yield return null;
        }
        GameManager.Instance.SetStartGame();
        GameManager.Instance.SetFinish();

        //scale up 
        float a = 0;
        while (Vector3.Distance(swimObject.position, standObject.position) < 4)
        {
            a += Time.fixedDeltaTime;
            if (a > 1) a = 1;
            swimObject.transform.Translate(-5 * Time.fixedDeltaTime * Vector3.forward, Space.Self);
            lineObject.transform.localScale = new Vector3(1, 1, a);
            yield return null;
        }
        GameManager.Instance.ready = true;
        GameManager.Instance.SetStartGame();
    }
    #endregion

    #region scale down and move to origin position vector3(0,40,0)
    public void BackToOrigin(bool isRestart)
    {
        ismove = false;
        StartCoroutine(MoveToOrigin(isRestart));
    }
    IEnumerator MoveToOrigin(bool isRestart)
    {
        SetColldier("disable"); // TAT COLLIDER
        yield return new WaitForSeconds(0.5f);

        float scale = 1;
        //scale down before moving
        while (Vector3.Distance(swimObject.position, standObject.position) > 0.01f)
        {
            scale = scale - Time.deltaTime * 130;
            if (scale < 0) scale = 0;

            swimObject.transform.position = Vector3.MoveTowards(swimObject.position, standObject.position, 5 * Time.deltaTime);
            lineObject.transform.localScale = new Vector3(1, 1, scale);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.SoundWhenMoveUp();

        transform.position = originPos;     //move to origin position
        UIManager.Instance.NextLevelControl(isRestart);    //call nextLV panel

        GameManager.Instance.ready = true;  //set Game manager ready for pretending UImanager call to much when next lv panel open

        startClick = false;
    }
    #endregion

    public void SetColldier(string s)   // on/off collider for start/end each round
    {
        if(s == "enable")
        {
            standObject.GetComponent<Collider>().enabled = true;
            swimObject.GetComponent<Collider>().enabled = true;
            lineObject.GetChild(0).GetComponent<Collider>().enabled = true;
        }
        if (s == "disable")
        {
            standObject.GetComponent<Collider>().enabled = false;
            swimObject.GetComponent<Collider>().enabled = false;
            lineObject.GetChild(0).GetComponent<Collider>().enabled = false;
        }
    }

    #region help create a position for spawn a star
    public Vector3 RandomPositionFromSwim() //get a random position near swim object
    {
        Vector3 pos = swimObject.transform.position;
        return new Vector3(pos.x + Random.Range(2, 4), 2, pos.z + Random.Range(2, 4));
    }
    public Vector3 PositionFromSwim()   //get a current swim object position
    {
        Vector3 pos = swimObject.transform.position;
        return new Vector3(pos.x, 2, pos.z);
    }
    #endregion

}
