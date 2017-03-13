﻿
using CustomDebug;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game {
	public class Line : MonoBehaviour {
		private static GameObject lastLine;
		private static Material material;

		public LineRenderer Renderer;
		public EdgeCollider2D Collider;
		public bool MoveWithScene;
		public readonly Vector3[] Vertices = new Vector3[2];
		private readonly Vector2[] vertices2D = new Vector2[2];
		private Vector3 lastPosition;

		public void Awake() {
			if (!material) {
				const string path = "Materials/mat_lineReleased";
				material = Resources.Load<Material>(path);
				if (!material) {
					DebugMsg.ResourceNotFound(Debug.LogError, path);
				}
			}
		}

		public void Create(LineRenderer other) {
			Destroy(lastLine);
			lastLine = gameObject;
			gameObject.layer = LayerMaskManager.Get(Layer.Line);
			Collider = gameObject.AddComponent<EdgeCollider2D>();
			Renderer = gameObject.AddComponent<LineRenderer>();

			other.GetPositions(Vertices);
			Renderer.SetPositions(Vertices);
			Renderer.receiveShadows = false;
			Renderer.shadowCastingMode = ShadowCastingMode.Off;
			Renderer.widthMultiplier = 0.50f;
			Renderer.material = material;

			vertices2D[0] = Vertices[0];
			vertices2D[1] = Vertices[1];
			Collider.points = vertices2D;
		}

		public void Update() {
			if (MoveWithScene) {
				Vector3 deltaPosition = transform.position - lastPosition;
				lastPosition = transform.position;

				Renderer.GetPositions(Vertices);
				Vertices[0] += deltaPosition;
				Vertices[1] += deltaPosition;
				Renderer.SetPositions(Vertices);

			}
			
		}

	}
}
