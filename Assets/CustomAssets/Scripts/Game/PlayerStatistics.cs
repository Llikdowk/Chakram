﻿
using UnityEngine;

namespace Game {
	public class PlayerStatistics : IPersistant {

		public int HighScore {
			get { return highScore; }
			set {
				highScore = value;
				if (highScore >= 10) {
					nextMedal = 1;
				}
				if (highScore >= 20) {
					nextMedal = 2;
				}
				if (highScore >= 50) {
					nextMedal = 3;
				}
				if (highScore >= 100) {
					nextMedal = 4;
				}

				Debug.Log("Current MEdal = " + nextMedal + " value= " + medals[nextMedal]);
			}
		}
		private int highScore = -1;
		private readonly string[] medals = new string[5];
		private int nextMedal = 0;
		public int Crashes = 0;

		public static PlayerStatistics Instance {
			get {
				if (instance == null) instance = new PlayerStatistics();
				return instance;
			}
		}
		private static PlayerStatistics instance;

		private PlayerStatistics() {
			medals[0] = "Next medal: Bronze medal, at 10";
			medals[1] = "Next medal: Silver medal, at 20";
			medals[2] = "Next medal: Gold medal, at 50";
			medals[3] = "Next medal: Platinum medal, at 100";
			medals[4] = "You've reached the max medal! but have no doubt that someone in the world is better than you. Always.";
			Load();
			if (HighScore != -1) {
				UIManager.Instance.SetMaxScore(HighScore);
			}
		}

		public void Load() {
			nextMedal = PlayerPrefs.GetInt("NextMedal");
			HighScore = PlayerPrefs.GetInt("HighScore");
			Crashes = PlayerPrefs.GetInt("Crashes");
		}

		public void Save() {
			PlayerPrefs.SetInt("NextMedal", nextMedal);
			PlayerPrefs.SetInt("HighScore", HighScore);
			PlayerPrefs.SetInt("Crashes", Crashes);
		}

		public void Clear() {
			PlayerPrefs.SetInt("NextMedal", 0);
			PlayerPrefs.SetInt("HighScore", -1);
			PlayerPrefs.SetInt("Crashes", 0);
		}

	}
}
