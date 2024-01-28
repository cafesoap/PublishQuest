using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI nowHappyScore;
  //  public TextMeshProUGUI nowDeScore;
    public TextMeshProUGUI GoalHappyScore;
    public int nowHappyScoreDefault;
    public int nowDeScoreDefault;
  //  public int GoalHappyScoreDefault;

    public TextMeshProUGUI myName;
    public TextMeshProUGUI myComment;
    public Image myHeadPic;

    public PQ PQ;
    public TextMeshProUGUI AllowInfluenceScore;
    public TextMeshProUGUI DisallowInfluenceScore;

    private int count;

    private int randResultAllowHappy;
   // private int randResultAllowDe;
    private int randResultDisallowHappy;
   // private int randResultDisallowDe;

    private List<int> remainingIds = new List<int>();
    // Start is called before the first frame update


    // ������ÿ�ص�GoalHappyScore����
    public int[] goalHappyScores;

    // ������ÿ�ص�LevelCommentNum����
    public int[] levelCommentNums;

    public string[] LevelBossState;
    private int BossCount;

    private int randomCount;

    // ��������ǰ�ؿ�����
    private int currentLevelIndex = 0;


    public TextMeshProUGUI DayNum;
    public TextMeshProUGUI BossState;
    public TextMeshProUGUI BossSore;

    private string LevelBoss;

    public GameObject commentState;
    public TextMeshProUGUI commentStateText;

    public TextMeshProUGUI offWorkCounts;


    public GameObject DailyReport;
    public GameObject Win;
    public GameObject Loss;
    public TextMeshProUGUI nowHappyScore_Report;
    //  public TextMeshProUGUI nowDeScore;
    public TextMeshProUGUI GoalHappyScore_Report;

    private bool dailyResult;

    public GameObject fire;

    public GameObject gameEnd;
    void Start()
    {
        BossCount = LevelBossState.Length;
        nowHappyScore.text= nowHappyScoreDefault.ToString();
      //  nowDeScore.text = nowDeScoreDefault.ToString();
        GoalHappyScore.text = goalHappyScores[0].ToString();
        commentState.SetActive(false);
        PQ.Init();
        count = PQ.PQDic.Count;
        randomCount = -1;
        InitializeRemainingIds();
        RandBoss();
        RandPeople();
        offWorkCounts.text = levelCommentNums[0].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandPeople();
        }
    }
    void RandBoss()
    {
        if (BossCount > 0)
        {
            int randomBoss = Random.Range(0, BossCount);
            LevelBoss = LevelBossState[randomBoss];

            BossState.text = LevelBoss;

        }
    }

    void InitializeRemainingIds()
    {
        remainingIds.Clear(); // ����б�
        // ����һ���������п���ID���б�
        for (int i = 1; i <= count; i++)
        {
            remainingIds.Add(i);
        }
    }
    void RandPeople()
    {
        if (count > 0 )
        {

            // ���remainingIdsΪ�գ����³�ʼ��
            if (remainingIds.Count == 0)
            {
                InitializeRemainingIds();
            }
            // ���ѡ��һ��δ�������ID
            int randomIndex = Random.Range(0, remainingIds.Count);
            int randomId = remainingIds[randomIndex];

            // ��δ�������ID�б����Ƴ��Ѿ��������ID
            remainingIds.RemoveAt(randomIndex);
            // ���ѡ��һ��ID������ID��1��count-1��
            

            // ����ID��ȡ������ݣ���������һ����ΪPlayerData�������洢������ݣ�
            var people = PQ.PQDic[randomId];

            // ������Ӧ�õ�UIԪ����
            myName.text = people.Name;
            myComment.text = people.Comment;

            randResultAllowHappy = people.AllowHappy;
         //   randResultAllowDe = people.AllowDe;
            randResultDisallowHappy = people.DisallowHappy;
          //  randResultDisallowDe = people.DisallowDe;


            DisplayInfluenceScore(people.AllowHappy, /*people.AllowDe,*/ AllowInfluenceScore);
            DisplayInfluenceScore(people.DisallowHappy, /*people.DisallowDe, */DisallowInfluenceScore);
            // ��ȡͷ���Sprite
            Sprite headSprite = Resources.Load<Sprite>("Sprites/Head/" + people.HeadPic);
            if (headSprite != null)
            {
                myHeadPic.sprite = headSprite;
            }
            else
            {
                Debug.LogError("ͷ��ͼƬδ�ҵ���" + people.HeadPic);
            }

            if (people.Tag !="")
            {
                commentState.SetActive(true);
                commentStateText.text = people.Tag;
            }
            else
            {
                commentState.SetActive(false);
                commentStateText.text = "";
            }
        }
        else
        {
            Debug.LogWarning("countС�ڵ���0���޷����������Ϣ");
        }
        void DisplayInfluenceScore(int happyScore,/* int deScore,*/ TextMeshProUGUI textComponent)
        {
            string happyText = happyScore >= 0 ? "+" + happyScore.ToString() : happyScore.ToString();
          //  string deText = deScore >= 0 ? "+" + deScore.ToString() : deScore.ToString();

            textComponent.text = happyText + "\n" /*+ deText*/;
        }

        // ������ÿ���һ�Σ������������
        randomCount++;

        // ����������Ƿ�ﵽ��ǰ�ؿ���LevelCommentNum
        if (randomCount >= levelCommentNums[currentLevelIndex])
        {

            /*// �л�����һ��
            SwitchToNextLevel();*/


            ShowDailyReport();
        }

    }
    public void OnAllowButtonClick()
    {
        if (commentStateText.text == BossState.text)
        {
            int bossScoreValue = int.Parse(BossSore.text);
            bossScoreValue++;
            BossSore.text = bossScoreValue.ToString();

        }
        // ��ȡAllowHappy��AllowDe��ֵ

        // ���·�����InfluenceScore���ı�
        UpdateScores(randResultAllowHappy/*, randResultAllowDe*/);
        
        

    }
    public void OnDisallowButtonClick()
    {
        // ��ȡDisallowHappy��DisallowDe��ֵ
    

        // ���·�����InfluenceScore���ı�
        UpdateScores(randResultDisallowHappy/*, randResultDisallowDe*/);
    }
    void UpdateScores(int happyChange = 0, int deChange = 0)
    {
        // ���·���
        int newHappyScore = int.Parse(nowHappyScore.text) + happyChange;
       // int newDeScore = int.Parse(nowDeScore.text) + deChange;

        nowHappyScore.text = newHappyScore.ToString();
        int offWorkNum = int.Parse(offWorkCounts.text);
        offWorkNum--;
        offWorkCounts.text = offWorkNum.ToString();
        //nowDeScore.text = newDeScore.ToString();

        // ����InfluenceScore���ı�
        DisplayInfluenceScore(newHappyScore, /*newDeScore,*/ AllowInfluenceScore);
        DisplayInfluenceScore(newHappyScore,/* newDeScore,*/ DisallowInfluenceScore);

        RandPeople();

    }


    // ��ʾInfluenceScore���ı�
    void DisplayInfluenceScore(int happyScore, /*int deScore,*/ TextMeshProUGUI textComponent)
    {
        string happyText = happyScore >= 0 ? "+" + happyScore.ToString() : happyScore.ToString();
      //  string deText = deScore >= 0 ? "+" + deScore.ToString() : deScore.ToString();

        textComponent.text = happyText/* + "\n" + deText*/;
    }

    void SwitchToNextLevel()
    {
        // �����������
        randomCount = 0;

        // �л�����һ��
        currentLevelIndex++;

        // ����Ƿ񳬹��ؿ�����������ǣ�������Ϸ��ִ����������
        if (currentLevelIndex >= goalHappyScores.Length)
        {
            Debug.Log("���йؿ�����ɣ�");
            // �����������Ϸ��������������
        }
        else
        {
            // ����GoalHappyScore��LevelCommentNum
            GoalHappyScore.text = goalHappyScores[currentLevelIndex].ToString();
            DayNum.text = (currentLevelIndex+1).ToString();
            offWorkCounts.text = levelCommentNums[currentLevelIndex].ToString();
            Debug.Log("�л����� " + (currentLevelIndex + 1) + " ��");
        }
        

    }
    void ShowDailyReport()
    {
        DailyReport.SetActive(true);

        nowHappyScore_Report.text = nowHappyScore.text;
        //  public TextMeshProUGUI nowDeScore;
        GoalHappyScore_Report.text = GoalHappyScore.text;
        if (int.Parse(nowHappyScore_Report.text) < int.Parse(GoalHappyScore_Report.text))
        {
            dailyResult = false;
            Loss.SetActive(true);
            Win.SetActive(false);
        }
        else
        {
            dailyResult = true;
            Win.SetActive(true);
            Loss.SetActive(false);
        }
    }
    public void OnclickGetBtn()
    {
        if (dailyResult)
        {
            if (currentLevelIndex < goalHappyScores.Length-1)
            {
                SwitchToNextLevel();
            }
            else
            {
                //ͨ�ؽ��

                gameEnd.SetActive(true);
                    if (int.Parse(BossSore.text)>=5)
                    {
                        if (BossState.text == "��")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("le1");
                        }
                        else if (BossState.text == "Т")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("xiao1");
                    }
                    }
                    else
                    {
                        if (BossState.text == "��")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("le2");
                    }
                        else if (BossState.text == "Т")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("xiao2");
                    }
                }
                
               
            }
            
        }
        else
        {
            //ÿ��ʧ��
            fire.SetActive(true);
        }

        DailyReport.SetActive(false);
    }
    public void ResultGameBtn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
