using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {

	public Buildable _buildablePrefab;

	public Text _price;
	public CanvasGroup _group;

	private GameManager _gameManager;


	void Start ()
	{
		_price.text = _buildablePrefab.Cost.ToString ();
		_gameManager = FindObjectOfType<GameManager> ();
	}

	void Update ()
	{
		var _canPurchase = _gameManager.Ressources >= _buildablePrefab.Cost;
		_group.interactable = _canPurchase;
	}
}
