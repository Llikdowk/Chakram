﻿
using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using UnityEngine;
using RSG;

public class BackgroundColorManager : MonoBehaviour {
	public Color[] ColorTuple1;
	public Color[] ColorTuple2;
	public Color[] ColorTuple3;
	public Color[] ColorTuple4;

	private readonly Color[][] colors = new Color[4][];
	public int CurrentPair = 0;
	public float DurationSeconds = 2.0f;

	private readonly C5.IList<SpriteRenderer> targetBlocks = new C5.ArrayList<SpriteRenderer>(16);
	private readonly PromiseTimer promiseTimer = new PromiseTimer();
	private readonly Dictionary<int, C5.ArrayList<SpriteRenderer>> map = new Dictionary<int, C5.ArrayList<SpriteRenderer>>();

	private static BackgroundColorManager instance;

	public void Awake() {
		if (instance == null) {
			instance = this;
			colors[0] = ColorTuple1;
			colors[1] = ColorTuple2;
			colors[2] = ColorTuple3;
			colors[3] = ColorTuple4;
		}
		else {
			Destroy(gameObject);
		}
	}

	public static void Register(GameObject block) {
		var childRenderers = block.GetComponentsInChildren<SpriteRenderer>();
		foreach(var renderer in childRenderers) {
			instance.targetBlocks.Add(renderer);
		}
		instance.map.Add(block.GetInstanceID(), 
			(ArrayList<SpriteRenderer>) instance.targetBlocks.View(
				instance.targetBlocks.Count-childRenderers.Length, 
				childRenderers.Length
				));
	}

	public static void Unregister(GameObject block) {
		instance.map[block.GetInstanceID()].Clear();
		instance.map.Remove(block.GetInstanceID());
	}

	private IPromise LerpColor(Color endColor, float durationSeconds) {
		var startColor = targetBlocks.First.material.color;
		return promiseTimer.WaitUntil(timeData => {
			float t = timeData.elapsedTime / durationSeconds;
			foreach (var r in targetBlocks) {
				r.material.color = Color.Lerp(startColor, endColor, t);
			}
			return t >= 1.0f;
		});
	}

	private IPromise ColorCycle() {
		var colorSequence = colors[CurrentPair]
			.Select(c => (Func<IPromise>)(() => LerpColor(c, DurationSeconds)));
		return Promise.Sequence(colorSequence).Then(()=>ColorCycle());
	}

	public void Start() {
		ColorCycle().Catch(Debug.LogError);
	}
	
	public void Update() {
		promiseTimer.Update(Time.deltaTime);
	}

}
