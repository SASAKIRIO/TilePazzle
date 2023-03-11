using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�N���v�^�u���I�u�W�F�N�g�̃f�[�^
/// </summary>
[Serializable]
public class TileData
{
    // �^�C����
    public string Name;

    // �^�C���A�C�R��
    public Sprite icon;
    
    // �^�C���̃I�u�W�F�N�g�i�v���n�u�j
    public GameObject gameObject;

    // �^�C����tips ������^
    public string tipsString;

    // �^�C����tips Texture�^
    public Texture tipsTexture;
}


/// <summary>
/// �X�N���v�^�u���I�u�W�F�N�g�̐���
/// </summary>
[CreateAssetMenu(fileName = "TileDataTable", menuName = "ScriptableObject/TileDataTable")]
public class TileDataTable : ScriptableObject
{
    public List<TileData> dataList = new List<TileData>();
}