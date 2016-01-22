using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class SpriteMapChunk : MonoBehaviour
	{
		static Material sharedMaterial = Resources.Load<Material> ("DefaultSpriteMaterial");
		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		Vector3[] vertices;
		Vector2[] uvs;
		int[] indices;

		int width;
		int height;
		Mesh chunkMesh;

		public void Setup (int width, int height)
		{
			this.width = width;
			this.height = height;
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
			meshRenderer.sharedMaterial = sharedMaterial;
			meshFilter = gameObject.AddComponent<MeshFilter> ();
			int quadsCount = width * height;
			int trianglesCount = quadsCount * 2;
			int verticesCount = quadsCount * 4;
			int indicesCount = trianglesCount * 3;
			int uvsCount = verticesCount;

			vertices = new Vector3[verticesCount];
			uvs = new Vector2[uvsCount];
			indices = new int[indicesCount];

			for (int i = 0; i < quadsCount; i++)
			{
				int vertexStart = i * 4;
				int indexStart = i * 6;
				int xOffset = i % width;
				int yOffset = i / width;
				vertices [vertexStart + 0] = new Vector3 (xOffset, yOffset + 1);
				vertices [vertexStart + 1] = new Vector3 (xOffset + 1, yOffset);
				vertices [vertexStart + 2] = new Vector3 (xOffset + 1, yOffset + 1);
				vertices [vertexStart + 3] = new Vector3 (xOffset, yOffset);

				indices [indexStart + 0] = vertexStart + 3;
				indices [indexStart + 1] = vertexStart + 0;
				indices [indexStart + 2] = vertexStart + 1;

				indices [indexStart + 3] = vertexStart + 2;
				indices [indexStart + 4] = vertexStart + 1;
				indices [indexStart + 5] = vertexStart + 0;

				uvs [vertexStart + 0] = Vector3.zero;
				uvs [vertexStart + 1] = Vector3.zero;
				uvs [vertexStart + 2] = Vector3.zero;
				uvs [vertexStart + 3] = Vector3.zero;
				/*uvs [vertexStart + 0] = new Vector3 (0f, 0f);
                uvs [vertexStart + 1] = new Vector3 (1f, 0f);
                uvs [vertexStart + 2] = new Vector3 (1f, 1f);
                uvs [vertexStart + 3] = new Vector3 (0f, 1f);*/

			}
			chunkMesh = new Mesh ();
			chunkMesh.MarkDynamic ();
			chunkMesh.vertices = vertices;
			chunkMesh.uv = uvs;
			chunkMesh.triangles = indices;
			//chunkMesh.UploadMeshData (false);
			meshFilter.mesh = chunkMesh;


		}

		public void SetTileSprite (int x, int y, Sprite sprite)
		{
			meshRenderer.sharedMaterial.mainTexture = sprite.texture;
			int uvIndex = (x + y * width) * 4;
			for (int i = 0; i < 4; i++)
				uvs [uvIndex + i] = sprite.uv [i];
			chunkMesh.uv = uvs;
		}

	}
}


