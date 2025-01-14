using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�׽�Ʈ��")]
    [SerializeField] private float moveSpeed;
    private Animator myAnimator;
    public Animator[] childAnimators;
    private float inputX;
    private float inputY;
    private bool isMove;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        // �θ� �ִϸ��̼� �ʱ⼼���� �ڽ� Animator�� ����ȭ
        SyncWithParentAnimator(myAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if (inputX != 0 || inputY != 0) isMove = true;
        else isMove = false;

        
        OnMove();
    }

    private void OnMove()
    {
        myAnimator.SetBool("isMove", isMove);
        myAnimator.SetFloat("inputX", inputX);
        myAnimator.SetFloat("inputY", inputY);

        SyncWithParentAnimator(myAnimator); //�ڽľִϸ��̼ǿ��� �Ķ���� ����

        transform.Translate(new Vector2(inputX, inputY) * Time.deltaTime * moveSpeed);
    }

    void SyncWithParentAnimator(Animator parentAnimator)
    {
        foreach (Animator childAnimator in childAnimators)
        {
            childAnimator.SetBool("isMove", parentAnimator.GetBool("isMove"));
            childAnimator.SetFloat("inputX", parentAnimator.GetFloat("inputX"));
            childAnimator.SetFloat("inputY", parentAnimator.GetFloat("inputY"));
        }
    }

}
