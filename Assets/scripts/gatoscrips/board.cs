using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour {
    [Header("Input Settings")]
    [SerializeField] private LayerMask boxesLayerMask;
    [SerializeField] private float touchRadius;

    [Header("Mark Sprites")]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Header("Mark Colors")]
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorO;

    [Header("End Game Sprites")]
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite drawSprite;

    public UnityAction<Mark, Color> OnWinAction;

    public Mark[] marks;

    private Camera cam;
    private Mark currentMark;
    private bool canPlay;
    private LineRenderer lineRenderer;
    private int marksCount = 0;
    private GameObject endGameSpriteObject;

    private void Start() {
        InitializeComponents();
    }

    private void Update() {
        if (canPlay && currentMark == Mark.X && Input.GetMouseButtonUp(0)) {
            HandlePlayerInput();
        }

        if (canPlay && currentMark == Mark.O) {
            AIMove();
        }
    }

    private void InitializeComponents() {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        currentMark = Mark.X;

        marks = new Mark[9];

        canPlay = true;

        endGameSpriteObject = new GameObject("EndGameSprite");
        endGameSpriteObject.AddComponent<SpriteRenderer>();
        endGameSpriteObject.SetActive(false);
    }

    private void HandlePlayerInput() {
        Vector2 touchPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxesLayerMask);

        if (hit) 
            HitBox(hit.GetComponent<Box>());
    }

    private void HitBox(Box box) {
        if (!box.isMarked) {
            marks[box.index] = currentMark;

            box.SetAsMarked(GetSprite(), currentMark, GetColor());
            marksCount++;

            bool won = CheckIfWin();
            if (won) {
                HandleGameEnd(currentMark, GetColor(), winSprite, new Vector3(0.25f, 3.86f, 0));
                return;
            }

            if (marksCount == 9) {
                HandleGameEnd(Mark.None, Color.white, drawSprite, new Vector3(0f, 3.86f, 0));
                return;
            }

            SwitchPlayer();
        }
    }

    private void HandleGameEnd(Mark winningMark, Color winningColor, Sprite endSprite, Vector3 spritePosition) {
        if (OnWinAction != null)
            OnWinAction.Invoke(winningMark, winningColor);

        Debug.Log((winningMark == Mark.None) ? "Nobody Wins." : winningMark.ToString() + " Wins.");
        Vector3 customPosition = new Vector3(0.02f, 1.96f, 0);
        ShowEndGameSprite(endSprite, customPosition);
        canPlay = false;
    }

    private bool CheckIfWin() {
        return
        AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
    }

    private bool AreBoxesMatched(int i, int j, int k) {
        Mark m = currentMark;
        bool matched = (marks[i] == m && marks[j] == m && marks[k] == m);

        if (matched)
            DrawLine(i, k);

        return matched;
    }

    private void DrawLine(int i, int k) {
        lineRenderer.SetPosition(0, transform.GetChild(i).position);
        lineRenderer.SetPosition(1, transform.GetChild(k).position);
        Color color = GetColor();
        color.a = .3f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.enabled = true;
    }

    private void SwitchPlayer() {
        currentMark = (currentMark == Mark.X) ? Mark.O : Mark.X;

        if (currentMark == Mark.O && canPlay) {
            AIMove();
        }
    }

    private Color GetColor() {
        return (currentMark == Mark.X) ? colorX : colorO;
    }

    private Sprite GetSprite() {
        return (currentMark == Mark.X) ? spriteX : spriteO;
    }

    private void ShowEndGameSprite(Sprite sprite, Vector3 position) {
        SpriteRenderer renderer = endGameSpriteObject.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingLayerName = "juego";
        endGameSpriteObject.transform.position = position; 
        endGameSpriteObject.transform.localScale = new Vector3(2.38f, 1.38f, 1); 
        endGameSpriteObject.SetActive(true);
    }

    private void AIMove() {
        int index = FindWinningMove(Mark.O);
        if (index != -1) {
            HitBox(transform.GetChild(index).GetComponent<Box>());
            return;
        }

        // If AI can't win in the next move, try to block player's winning move
        index = FindWinningMove(Mark.X);
        if (index != -1) {
            HitBox(transform.GetChild(index).GetComponent<Box>());
            return;
        }

        // If no immediate winning moves for either side, make a random move
        do {
            index = Random.Range(0, 9);
        } while (marks[index] != Mark.None);

        HitBox(transform.GetChild(index).GetComponent<Box>());
    }
private int FindWinningMove(Mark mark) {
        for (int i = 0; i < 9; i += 3) {
            if (marks[i] == mark && marks[i + 1] == mark && marks[i + 2] == Mark.None) return i + 2;
            if (marks[i] == mark && marks[i + 2] == mark && marks[i + 1] == Mark.None) return i + 1;
            if (marks[i + 1] == mark && marks[i + 2] == mark && marks[i] == Mark.None) return i;
        }

        for (int i = 0; i < 3; i++) {
            if (marks[i] == mark && marks[i + 3] == mark && marks[i + 6] == Mark.None) return i + 6;
            if (marks[i] == mark && marks[i + 6] == mark && marks[i + 3] == Mark.None) return i + 3;
            if (marks[i + 3] == mark && marks[i + 6] == mark && marks[i] == Mark.None) return i;
        }

        if (marks[0] == mark && marks[4] == mark && marks[8] == Mark.None) return 8;
        if (marks[0] == mark && marks[8] == mark && marks[4] == Mark.None) return 4;
        if (marks[4] == mark && marks[8] == mark && marks[0] == Mark.None) return 0;

        if (marks[2] == mark && marks[4] == mark && marks[6] == Mark.None) return 6;
        if (marks[2] == mark && marks[6] == mark && marks[4] == Mark.None) return 4;
        if (marks[4] == mark && marks[6] == mark && marks[2] == Mark.None) return 2;

        return -1;
    }
}
