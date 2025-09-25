using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject goPlayerBrickPrefab;
    [SerializeField] private Transform tfPlayerModel; //vi tri cua hinh anh nhan vat
    [SerializeField] private Transform tfPlayerBrick; //vi tri cua gach duoi chan
    [SerializeField] private float speed; //toc do di chuyen
    [SerializeField] private Vector2 startTouch; //vi tri cham dau tien
    [SerializeField] private Vector2 endTouch; //vi tri cham cuoi cung
    [SerializeField] private TextMeshProUGUI brickText; // UI hiển thị số gạch
    [SerializeField] private int brickCount; //so luong gach duoi chan
    [SerializeField] private float _brickHeight = 0.1499993f;
    [SerializeField] private GameManager gameManager;
    
  
    public float _height;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    public int BrickCount => brickCount;
    private bool isFinishing = false;
    private Rigidbody rb;
    private Vector3 targetPosition; //diem den de dung lai
    private bool isMoving = false;

    private void Awake()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position; //ban đầu target = vị trí hiện tại
        
    }

    void Update()
    {
        if (!isMoving) // chỉ nhận input khi không di chuyển
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouch = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endTouch = Input.mousePosition;
                Vector2 swipe = endTouch - startTouch;

                if (swipe.magnitude > 50f)
                {
                    Vector3 dir = Vector3.zero;

                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                        dir = (swipe.x > 0) ? Vector3.right : Vector3.left;
                    else
                        dir = (swipe.y > 0) ? Vector3.forward : Vector3.back;

                    RaycastHit hit;
                    int layerMask = ~LayerMask.GetMask("PlatformBrick");

                    if (Physics.Raycast(transform.position, dir, out hit, 1000f, layerMask, QueryTriggerInteraction.Collide))
                    {
                        Debug.DrawRay(transform.position, dir * hit.distance, Color.red, 2f);

                        if (hit.collider.CompareTag("Intersection"))
                        {
                            targetPosition = hit.collider.transform.position;
                            isMoving = true;
                        }
                        else if (hit.collider.CompareTag("Wall"))
                        {
                            targetPosition = hit.point - dir * 0.5f;
                            isMoving = true;
                        }
                        else if (hit.collider.CompareTag("WallGate"))
                        {
                            RaycastHit nextHit;
                            if (Physics.Raycast(hit.point + dir * 0.1f, dir, out nextHit, 1000f, layerMask, QueryTriggerInteraction.Collide))
                            {
                                Debug.DrawRay(hit.point, dir * nextHit.distance, Color.green, 2f);

                                if (nextHit.collider.CompareTag("Wall"))
                                    targetPosition = nextHit.point - dir * 0.5f;
                                else if (nextHit.collider.CompareTag("Intersection"))
                                    targetPosition = nextHit.collider.transform.position;
                                else
                                    targetPosition = nextHit.point - dir * 0.5f;
                            }
                            else
                            {
                                targetPosition = hit.point + dir * 10f;
                            }

                            isMoving = true;
                        }
                        else if (hit.collider.CompareTag("Finish"))
                        {
                            //cham finish thi di toi tam finish
                            targetPosition = hit.collider.transform.position;
                            isMoving = true;
                        }
                        else
                        {
                            targetPosition = hit.point - dir * 0.5f;
                            isMoving = true;
                        }
                    }
                    else
                    {
                        isMoving = false;
                    }
                }
            }
        }
        else
        {
            //di chuyen ve target ppossition
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;

                //Chua dung neu van dang chay tren bridge
                if (!isFinishing)
                    isMoving = false;
            }
        }
    }

    
    void FixedUpdate()
    {
        
    }

    //Bat va cham voi gach  tren platform
    private void AddBrick()
    {
        //tang chieu cao cua stack
        _height += _brickHeight;
        
        GameObject newBrick = Instantiate(goPlayerBrickPrefab, tfPlayerBrick);

        
        newBrick.transform.localPosition = new Vector3(0, _height, 0);

        
        Collider col = newBrick.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = newBrick.GetComponent<Rigidbody>();
        if (rb) Destroy(rb);

        
        brickCount++;
        UpdateBrickUI();        
        tfPlayerModel.localPosition = new Vector3(0, _height - 0.2f, 0);
    }

    
    public void RemoveBrick(int amount = 1)
    {
        amount = Mathf.Min(amount, brickCount);

        for (int i = 0; i < amount; i++)
        {
            if (brickCount > 0)
            {
                //giam so luong gach
                brickCount--;
                UpdateBrickUI();
                //xoa brick cuoi cung
                int childIndex = tfPlayerBrick.childCount - 1;
                Transform lastBrick = tfPlayerBrick.GetChild(childIndex);
                Destroy(lastBrick.gameObject);
            }
        }

        //cap nhat lai chieu cao cho model
        _height = brickCount * _brickHeight;
        tfPlayerModel.localPosition = new Vector3(0, _height, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlatformBrick"))
        {
            PlatformBrick brick = other.GetComponent<PlatformBrick>();
            if (brick != null)
            {
                brick.DisableBrick();
                AddBrick();
            }
        }

        if (other.CompareTag("Intersection"))
        {
            targetPosition = other.transform.position;
            isMoving = true;
        }
       
        if (other.CompareTag("Finish") && !isFinishing)
        {
            //Debug.Log("xay cau");
            isFinishing = true;

            //di toi khi het gach
            StartCoroutine(RemoveBricksOverTime());
        }

        if (other.CompareTag("BridgeBrick"))
        {
            if (brickCount > 0)
            {
                BridgeBrick brick = other.GetComponent<BridgeBrick>();
                if (brick != null) brick.Show();

                RemoveBrick();
            }
            else
            {
                isMoving = false;
            }
        }
    }

    
    public float GetBrickHeight()
    {
        Renderer rend = goPlayerBrickPrefab.GetComponent<Renderer>();
        return rend.bounds.size.y;
    }

    //Xoa gach tu tu khi xay cau
    private IEnumerator RemoveBricksOverTime()
    {
        while (brickCount > 0)
        {
            RemoveBrick();  // Mỗi lần trừ 1 viên
            yield return new WaitForSeconds(0.05f);
        }

        // Brick đã hết
        Debug.Log("🎉 Brick đã về 0, nhân vật dừng lại!");

        isMoving = false;  // Dừng Player

        // Hiển thị màn hình Finish
        // Khi hết gạch thì load FinishScene luôn
        UnityEngine.SceneManagement.SceneManager.LoadScene("FinishScene");

    }

    //Hien thi so luong gach
    private void UpdateBrickUI()
    {
        if (brickText != null)
        {
            brickText.text = "Bricks: " + brickCount;
        }
    }
    
    //Xoa het gach khi qua tuong
    public void ClearAllBricks()
    {
        brickCount = 0;
        _height = 0f;

        // Xoá toàn bộ gạch con trong PlayerBrickHolder
        for (int i = tfPlayerBrick.childCount - 1; i >= 0; i--)
        {
            Destroy(tfPlayerBrick.GetChild(i).gameObject);
        }
        UpdateBrickUI();
        // Reset vị trí nhân vật về sát mặt đất
        tfPlayerModel.localPosition = Vector3.zero;
    }
    
    

}


