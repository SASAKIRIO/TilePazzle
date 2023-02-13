using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Tile : MonoBehaviour
{
    [Tooltip("�^�C���̃^�C�v")]
    public enum Tile_Type
    {
        /// <summary>
        /// �Ȃɂ��^�C�����Ȃ�
        /// </summary>
        None,
        /// <summary>
        /// ���ʂȂ�
        /// </summary>
        Pink,
        /// <summary>
        /// ���ʂȂ��A(�p�Y����퓬)
        /// </summary>
        Green,
        /// <summary>
        /// ��O�̃^�C���ɖ߂����
        /// </summary>
        Yellow,
        /// <summary>
        /// �I�����W�̍��肪��
        /// </summary>
        Orange,
        /// <summary>
        /// �Ό��ł��ׂ�B�������̍��肪��
        /// </summary>
        Purple,
        /// <summary>
        /// ���F�Ɨא� -> �ЂƂO�ɖ߂�
        /// �I�����W�̍��肪���Ă��� -> ��O�ɖ߂�
        /// </summary>
        Blue,
        /// <summary>
        /// �ʍs�֎~
        /// </summary>
        Red,
    }

    public enum Tile_Feature
    {
        None,
        Bounce,
        Slip,
    }


    [Header("�^�C���̎��")]
    [SerializeField] public Tile_Type _tileType = default;

    
}
