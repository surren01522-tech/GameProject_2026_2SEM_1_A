using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerPickup : MonoBehaviour
{
    [Header("직접 연결")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [Header("아이템 탐색")]
    [SerializeField] private float detectionRadius = 2f;
    [Header("줍기 모션")]
    [SerializeField] private float pickupDuraion = 1.2f;
    [SerializeField] private float pickupMoment = 0.55f;

    private PickupItem targetItem;
    private bool isPickingUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickingUp) return;

        //player 주변에서 가장 가까운 아이템을 찾는다.
        targetItem = FindClosestItem();

        if (targetItem == null) return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard.eKey.wasPressedThisFrame)
        {
            StartCoroutine(PickupAnimation());
        }
    }

    private PickupItem FindClosestItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, ~0, QueryTriggerInteraction.Collide);
        PickupItem closestItem = null;
        float closesDisance = Mathf.Infinity;

        foreach(Collider itemCollider in colliders)
        {
            PickupItem item = itemCollider.GetComponentInParent<PickupItem>();

            if (item == null) continue;

            float distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance < closesDisance)
            {
                closesDisance = distance;
                closestItem = item;
            }
        }
        return closestItem;
    }

    private IEnumerator PickupAnimation()
    {
        isPickingUp = true;

        //현재 찾은 아이템을 저장한다.
        PickupItem itemToCollect = targetItem;

        //아이템 방향으로 캐릭터를 돌린다.
        Vector3 itemDirection = itemToCollect.transform.position - transform.position;
        itemDirection.y = 0;

        if (itemDirection.sqrMagnitude > 0.0001f )
        {
            transform.rotation = Quaternion.LookRotation(itemDirection);
        }

        //줍기 상태로 변경하여 이동을 막는다.
        playerController.ChangeState(PlayerState.Pickup);
        //전환선 없이 Pickup01 상태로 이동한다.
        animator.CrossFade("PickUp01", 0.1f);
        //손이 아이템이 닿는 시점까지 기다린다.
        yield return new WaitForSeconds(pickupMoment);

        //아이템을 실제로 흭득한다.
        if (itemToCollect != null)
        {
            itemToCollect.Collect();
        }

        //남은 애니메이션 시간을 기다린다.
        yield return new WaitForSeconds(pickupDuraion - pickupMoment);

        //
        animator.CrossFade("Blend Tree", 0.1f);

        //
        playerController.ChangeState(PlayerState.Normal);

        targetItem = null;
        isPickingUp = false;
    }
}
