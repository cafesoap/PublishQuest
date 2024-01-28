using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PQClass
{
	public int ID;
	public string Name;
	public string Comment;
	public string HeadPic;
	public int AllowHappy;
	//public int AllowDe;
	public int DisallowHappy;
	//public int DisallowDe;
	public string Tag;
}
[ExcelAsset]
public class PQ : ScriptableObject
{
	//public List<EntityType> PQ; // Replace 'EntityType' to an actual type that is serializable.
	public List<PQClass> PQExcel;
	public Dictionary<int, PQClass> PQDic => pQDic;
	public Dictionary<int, PQClass> pQDic;


	public void Init()
    {
		pQDic = new Dictionary<int, PQClass>();
		foreach (var People in PQExcel)
        {
			pQDic.Add(People.ID, People);

		}


	}
}
