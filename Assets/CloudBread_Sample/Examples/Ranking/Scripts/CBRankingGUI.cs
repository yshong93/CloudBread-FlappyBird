using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CloudBread;

public class CBRankingGUI : MonoBehaviour {

	[Serializable]
	public struct RankingRow {
		[SerializeField]
		public string element;

		[SerializeField]
		public string score;

		[SerializeField]
		public string value;

		[SerializeField]
		public string key;
	}

	public struct RankData {
		public int rank;
		public string name;
		public string score;
	}

	private const int RANK_NUM = 10;

	public RankingRow[] RankingList;

	public GameObject myNameTextGameObj;
	public GameObject myScoreTextGameObj;
	public GameObject myRankTextGameObj;

	private GameObject [] RankingBoardRow_Gameobject = new GameObject [RANK_NUM];


	// Use this for initialization
	void Start () {
		initMyRank ();

//		CloudBread cb = new CloudBread ();
//		cb.CBComSelMember(

		RankingBoardRow_Gameobject [0] = GameObject.Find ("RankingBoardRow");
		RankingBoardRow_Gameobject [0].transform.FindChild ("RnakingNumText").GetComponent<Text> ().text = "1";

		GameObject parentObject = GameObject.Find("PanelRankingBoard") as GameObject;

		for (int i = 1; i < RANK_NUM; i++) {
			RankingBoardRow_Gameobject [i] = Instantiate (RankingBoardRow_Gameobject [0]) as GameObject;
			RankingBoardRow_Gameobject[i].transform.SetParent(parentObject.transform, false);
			RankingBoardRow_Gameobject [i].transform.FindChild ("RnakingNumText").GetComponent<Text> ().text = (i + 1).ToString ();
		}

//		RankingBoardRow_Gameobject [7].transform.FindChild ("NameText").GetComponent<Text> ().text = "HiHIhI";

		gridLayoutGroup = parentObject.GetComponent<GridLayoutGroup> ();
		rect = parentObject.GetComponent<RectTransform> ();

		gridLayoutGroup.cellSize = new Vector2 (rect.rect.width, rect.rect.height/11);
		cellCount = GetComponentsInChildren<RectTransform> ().Length;

		CloudBread cb = new CloudBread ();
		cb.CBTopRanker (callback_TopRanker);

	}

	Dictionary<string, string>[] RankingResultDicArr;

	private void callback_TopRanker(string id, WWW www){
//		RankingList = JsonFx.Json.JsonReader.Deserialize<RankingRow[]> (www.text);

		print("[callback_TopRanker] : " + www.text);
		RankingResultDicArr = JsonFx.Json.JsonReader.Deserialize<Dictionary<string,string>[]> (www.text);

		/*
		foreach (var Rank in RankingResultDicArr) {
		CloudBread cb = new CloudBread ();
			cb.CBComSelMember2 (Rank["element"], callback_CBComSelMember);
		}
		*/

		int j = 0;
		foreach (var Rank in RankingResultDicArr) {
			

			setRankBoardGameObjWithRankData (j, new RankData {
				name = Rank["element"],
				score = Rank["score"],
				rank = j+1
			});
			j++;

//			rankDataList.Add (new RankData {
//				name = Rank["element"],
//				score = Rank["score"],
//				rank = j
//			});
		}

	}

	int i = 0;

	ArrayList rankDataList = new ArrayList();

	private void callback_CBComSelMember(string id, WWW www){
		print ("[callback_CBComSelMember] : " + www.text);
		var dicArr = JsonFx.Json.JsonReader.Deserialize<Dictionary<string,string>[]> (www.text);


		if (dicArr.Length != 0) {
			rankDataList.Add (new RankData {
				name = dicArr [0] ["name1"],
				score = RankingResultDicArr [i] ["score"],
				rank = i + 1
			});
			setRankBoardGameObjWithRankData (i, new RankData {
				name = dicArr [0] ["name1"],
				score = RankingResultDicArr [i] ["score"],
				rank = i + 1
			});
			i++;
		}

//			if (RankingResultDicArr.Length > rankDataList.Count) {
//				CloudBread cb = new CloudBread ();
//				cb.CBComSelMember2 (RankingResultDicArr [i] ["element"], callback_CBComSelMember);
//			} else {
//				setRankBoardGameObjWithRankDataList (rankDataList);
//			}

				
			
		

//		} 
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void initMyRank(){
		myNameTextGameObj.GetComponent<Text> ().text = "홍윤석";
		myRankTextGameObj.GetComponent<Text> ().text = "1";
		myScoreTextGameObj.GetComponent<Text> ().text = "10000";
	}

	private void setRankBoardGameObjWithRankData(int num, RankData rankData){
		GameObject rankBoardGameObj = RankingBoardRow_Gameobject [num];
		rankBoardGameObj.transform.FindChild ("RnakingNumText").GetComponent<Text> ().text = (rankData.rank).ToString ();
		rankBoardGameObj.transform.FindChild ("NameText").GetComponent<Text> ().text = rankData.name;
		rankBoardGameObj.transform.FindChild ("ScoreText").GetComponent<Text> ().text = rankData.score;
	}

	private void setRankBoardGameObjWithRankDataList(ArrayList rankList){
		int i = 0;
		foreach (RankData item in rankList) {
//			i++;
			setRankBoardGameObjWithRankData (i, item);
			i++;
		}
	}

	int  getRankBoardGameObjNum(string memberID){
		
		int i = 0;
		foreach (var item in RankingList) {
			if(String.Compare(memberID, item.key) !=0)
				return i;

			i++;
		}
		return -1;
	}

	GridLayoutGroup gridLayoutGroup;
	RectTransform rect;
	public float height;
	public int cellCount = 2;
		
	void OnRectTransformDimensionsChange ()
	{
		if (gridLayoutGroup != null && rect != null)
		if ((rect.rect.height + (gridLayoutGroup.padding.horizontal * 2)) * cellCount < rect.rect.width)
			gridLayoutGroup.cellSize = new Vector2 (rect.rect.height, rect.rect.height);
	}


}
