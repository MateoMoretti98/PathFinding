using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Stuff")]
    public Dictionary<Node, float> neighbours = new Dictionary<Node, float>();
	public Node previousNode;
	public LayerMask obstacles;
	public float G = float.MaxValue;
	public float H;
	public float F { get { return G + H; } }
	public float radius;

	bool isBlocked;

	[ExecuteInEditMode]
	private void Awake()
	{
		List<Node> allNodes = new List<Node>(FindObjectsOfType<Node>());

		foreach (Node node in allNodes)
		{
			float distanceToNode = Vector3.Distance(this.transform.position, node.transform.position);
			if (distanceToNode <= radius)
			{
				if(!Physics.Raycast(this.transform.position, Vector3.Normalize(node.transform.position - this.transform.position), Mathf.Clamp(distanceToNode, 0, radius), obstacles))
				{
					neighbours.Add(node, distanceToNode);
				}
			}
		}
	}

	public void Reset()
	{
		G = Mathf.Infinity;
		previousNode = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;

		foreach (var neighbour in neighbours)
		{
			Gizmos.DrawLine(transform.position, neighbour.Key.transform.position);
		}
	}
}
