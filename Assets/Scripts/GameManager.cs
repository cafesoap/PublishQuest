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


    // 新增：每关的GoalHappyScore数组
    public int[] goalHappyScores;

    // 新增：每关的LevelCommentNum数组
    public int[] levelCommentNums;

    public string[] LevelBossState;
    private int BossCount;

    private int randomCount;

    // 新增：当前关卡索引
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
        remainingIds.Clear(); // 清空列表
        // 创建一个包含所有可能ID的列表
        for (int i = 1; i <= count; i++)
        {
            remainingIds.Add(i);
        }
    }
    void RandPeople()
    {
        if (count > 0 )
        {

            // 如果remainingIds为空，重新初始化
            if (remainingIds.Count == 0)
            {
                InitializeRemainingIds();
            }
            // 随机选择一个未随机到的ID
            int randomIndex = Random.Range(0, remainingIds.Count);
            int randomId = remainingIds[randomIndex];

            // 从未随机到的ID列表中移除已经随机到的ID
            remainingIds.RemoveAt(randomIndex);
            // 随机选择一个ID（假设ID从1到count-1）
            

            // 根据ID获取表格数据（假设你有一个名为PlayerData的类来存储玩家数据）
            var people = PQ.PQDic[randomId];

            // 将数据应用到UI元素上
            myName.text = people.Name;
            myComment.text = people.Comment;

            randResultAllowHappy = people.AllowHappy;
         //   randResultAllowDe = people.AllowDe;
            randResultDisallowHappy = people.DisallowHappy;
          //  randResultDisallowDe = people.DisallowDe;


            DisplayInfluenceScore(people.AllowHappy, /*people.AllowDe,*/ AllowInfluenceScore);
            DisplayInfluenceScore(people.DisallowHappy, /*people.DisallowDe, */DisallowInfluenceScore);
            // 获取头像的Sprite
            Sprite headSprite = Resources.Load<Sprite>("Sprites/Head/" + people.HeadPic);
            if (headSprite != null)
            {
                myHeadPic.sprite = headSprite;
            }
            else
            {
                Debug.LogError("头像图片未找到：" + people.HeadPic);
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
            Debug.LogWarning("count小于等于0，无法更新玩家信息");
        }
        void DisplayInfluenceScore(int happyScore,/* int deScore,*/ TextMeshProUGUI textComponent)
        {
            string happyText = happyScore >= 0 ? "+" + happyScore.ToString() : happyScore.ToString();
          //  string deText = deScore >= 0 ? "+" + deScore.ToString() : deScore.ToString();

            textComponent.text = happyText + "\n" /*+ deText*/;
        }

        // 新增：每随机一次，增加随机次数
        randomCount++;

        // 新增：检查是否达到当前关卡的LevelCommentNum
        if (randomCount >= levelCommentNums[currentLevelIndex])
        {

            /*// 切换到下一关
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
        // 获取AllowHappy和AllowDe的值

        // 更新分数和InfluenceScore的文本
        UpdateScores(randResultAllowHappy/*, randResultAllowDe*/);
        
        

    }
    public void OnDisallowButtonClick()
    {
        // 获取DisallowHappy和DisallowDe的值
    

        // 更新分数和InfluenceScore的文本
        UpdateScores(randResultDisallowHappy/*, randResultDisallowDe*/);
    }
    void UpdateScores(int happyChange = 0, int deChange = 0)
    {
        // 更新分数
        int newHappyScore = int.Parse(nowHappyScore.text) + happyChange;
       // int newDeScore = int.Parse(nowDeScore.text) + deChange;

        nowHappyScore.text = newHappyScore.ToString();
        int offWorkNum = int.Parse(offWorkCounts.text);
        offWorkNum--;
        offWorkCounts.text = offWorkNum.ToString();
        //nowDeScore.text = newDeScore.ToString();

        // 更新InfluenceScore的文本
        DisplayInfluenceScore(newHappyScore, /*newDeScore,*/ AllowInfluenceScore);
        DisplayInfluenceScore(newHappyScore,/* newDeScore,*/ DisallowInfluenceScore);

        RandPeople();

    }


    // 显示InfluenceScore的文本
    void DisplayInfluenceScore(int happyScore, /*int deScore,*/ TextMeshProUGUI textComponent)
    {
        string happyText = happyScore >= 0 ? "+" + happyScore.ToString() : happyScore.ToString();
      //  string deText = deScore >= 0 ? "+" + deScore.ToString() : deScore.ToString();

        textComponent.text = happyText/* + "\n" + deText*/;
    }

    void SwitchToNextLevel()
    {
        // 重置随机次数
        randomCount = 0;

        // 切换到下一关
        currentLevelIndex++;

        // 检查是否超过关卡数量，如果是，结束游戏或执行其他操作
        if (currentLevelIndex >= goalHappyScores.Length)
        {
            Debug.Log("所有关卡已完成！");
            // 在这里添加游戏结束或其他操作
        }
        else
        {
            // 更新GoalHappyScore和LevelCommentNum
            GoalHappyScore.text = goalHappyScores[currentLevelIndex].ToString();
            DayNum.text = (currentLevelIndex+1).ToString();
            offWorkCounts.text = levelCommentNums[currentLevelIndex].ToString();
            Debug.Log("切换到第 " + (currentLevelIndex + 1) + " 关");
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
                //通关结局

                gameEnd.SetActive(true);
                    if (int.Parse(BossSore.text)>=5)
                    {
                        if (BossState.text == "乐")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("le1");
                        }
                        else if (BossState.text == "孝")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("xiao1");
                    }
                    }
                    else
                    {
                        if (BossState.text == "乐")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("le2");
                    }
                        else if (BossState.text == "孝")
                        {
                        gameEnd.GetComponent<GameEndManager>().PlayVideo("xiao2");
                    }
                }
                
               
            }
            
        }
        else
        {
            //每日失败
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
