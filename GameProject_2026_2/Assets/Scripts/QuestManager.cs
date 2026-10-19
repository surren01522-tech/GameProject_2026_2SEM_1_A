using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;

    private Quest currentQuest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questText.text = "진행 중인 퀘스트가 없습니다";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TalkToNPC(Quest npcQuest)
    {
        if(npcQuest.state == QuestState.Completed)
        {
            Debug.Log("이미 완료한 퀘스트입니다");
            return;
        }

        if(currentQuest == null || currentQuest.state == QuestState.Completed)
        {
            StartQuest(npcQuest);
        }

        if (currentQuest == null || currentQuest.state == QuestState.Completed) 
        {
            Debug.Log("이미 다른 퀘스트를 진행중입니다");
            return;
        }

        if (currentQuest.state == QuestState.InProgress)
        {
            Debug.Log("아직 아이템이 부족합니다");
        }
        else if(currentQuest.state == QuestState.ReadyToComplet)
        {
            CompleteQuest();
        }
    }

    private void UpdateQuestText()
    {
        if (currentQuest.state == QuestState.InProgress)
        {
            questText.text =
                currentQuest.questName + "\n" +
                currentQuest.targetItemName + " " +
                currentQuest.currentAmount + "/" +
                currentQuest.targetAmount;
        }
        else if(currentQuest.state == QuestState.ReadyToComplet)
        {
            questText.text = "수집 완료! \nNPC에게 돌아가세요";
        }
        else if(currentQuest.state == QuestState.Completed)
        {
            questText.text = currentQuest.questName + "\n퀘스트 완료";
        }
    }

    void StartQuest(Quest newQuest)
    {
        currentQuest = newQuest;
        currentQuest.currentAmount = 0;
        currentQuest.state = QuestState.InProgress;

        Debug.Log(currentQuest.questName + "퀘스트 시작!");
        UpdateQuestText();
    }

    public void AddQuestItem(string itemName, int amount)
    {
        if (currentQuest == null) return;
        if (currentQuest.state != QuestState.InProgress) return;
        if (currentQuest.targetItemName != itemName) return;

        currentQuest.currentAmount += amount;

        if (currentQuest.currentAmount >= currentQuest.targetAmount)
        {
            currentQuest.currentAmount = currentQuest.targetAmount;
            currentQuest.state = QuestState.ReadyToComplet;
        }

        UpdateQuestText();
    }

    public void CompleteQuest()
    {
        currentQuest.state = QuestState.Completed;
        Debug.Log(currentQuest.questName + "퀘스트 완료!");
        UpdateQuestText();
    }

}
