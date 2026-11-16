using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject achiementPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float displayTIme = 3.0f;

    private int tatalCollected;
    private bool firstItemUnlocked;
    private bool collectorUnlocked;
    private bool healUnlocked;
    private bool dropUnlocked;
    private bool QuestUnlocked;

    private Coroutine popupRoutine;

    void Start()
    {
        if (achiementPanel != null) achiementPanel.SetActive(false);
    }

    public void ItemCollected(int amount)
    {
        tatalCollected += amount;

        if (firstItemUnlocked == false )
        {
            firstItemUnlocked = true;
            Unlock("첫 수집", "처음으로 아이템을 획득 했습니다");
        }

        if (collectorUnlocked == false && tatalCollected >= 5)
        {
            collectorUnlocked = true;
            Unlock("초보 수집가", "아이템을 총 5개 획득했습니다");
        }
    }

    public void ItemUsed(string itemName)
    {
        if (healUnlocked) return;

        if (itemName == "물약" || itemName == "회복약" || itemName == "구급키트")
        {
            healUnlocked = true;
            Unlock("건강이 최고", "회복 아이템을 처음 사용했습니다");
        }
    }

    public void ItemDropped()
    {
        if (dropUnlocked) return;

        dropUnlocked = true;

        Unlock("과감한 정리", "아이템을 처음 버렸습니다");
    }

    public void QuestCompleted()
    {
        if (QuestUnlocked) return;

        QuestUnlocked = true;

        Unlock("퀘스트 해결사", "퀘스트를 처음 완료 하였습니다");
    }

    private void Unlock(string title, string description)
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            achiementPanel.SetActive(false);
        }

        popupRoutine = StartCoroutine(ShowPopup(title, description));
    }




    private IEnumerator ShowPopup(string title, string description)
    {
        if (achiementPanel == null || titleText == null || descriptionText == null)
        {
            Debug.LogWarning("AchiementManager의 UI가 연결 되어있지 않습니다.");
            yield break;
        }

        achiementPanel.SetActive(true);
        titleText.text = "업적 달성! \n" + title;
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTIme);

        achiementPanel.SetActive(false);
        popupRoutine = null;
    }

}
