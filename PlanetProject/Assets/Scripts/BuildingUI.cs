using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {

	public Buildable _buildablePrefab;

	public Text _price;
	public Text _avaliable;
	public CanvasGroup _group;

	private GameManager _gameManager;


	private int lastAvaliable = -100;

	void Start ()
	{
		if(_price != null)
			_price.text = _buildablePrefab.Cost.ToString ();
		_gameManager = FindObjectOfType<GameManager> ();
	}

	void Update ()
	{
		var _canPurchase = _gameManager.Ressources >= _buildablePrefab.Cost;
		_group.interactable = _canPurchase;

		var avaliableCount = (int)_gameManager.Ressources / (int)_buildablePrefab.Cost;
		if (avaliableCount != lastAvaliable) {
			lastAvaliable = avaliableCount;
			_avaliable.text = avaliableCount.ToString () + "x";
		}
	}
}
