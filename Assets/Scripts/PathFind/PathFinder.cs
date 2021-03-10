using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
	public List<Node> allNodes = new List<Node>();
	List<Node> closedNodes = new List<Node>();
	List<Node> openNodes = new List<Node>();

	private void Awake()
	{
		allNodes = new List<Node>(FindObjectsOfType<Node>());
	}

	public Stack<Node> GetPath(Node initialNode, Node finalNode)
	{
		foreach (var item in openNodes)
			item.Reset();
		foreach (var item in closedNodes)
			item.Reset();

		ResetAllForNewPath();
		Stack<Node> resultPath = new Stack<Node>();

		openNodes.Add(initialNode);

		initialNode.G = 0;
		initialNode.previousNode = null;

		while (openNodes.Count > 0)
		{
			Node current = LookForLowerF();


			if (current == finalNode)
			{
			
				while (current != null)
				{
					resultPath.Push(current);
					current = current.previousNode;
				}

				return resultPath;
			}

			foreach (var item in current.neighbours)
			{
				var neighNode = item.Key;
				var neighDist = item.Value;

				if (closedNodes.Contains(neighNode))
				{
					continue;
				}

				if (!openNodes.Contains(neighNode))
				{
			
					neighNode.H = Vector3.Distance(neighNode.transform.position, finalNode.transform.position);

					openNodes.Add(neighNode);
				}

		
				if (current.G + neighDist < neighNode.G)
				{
		
					neighNode.G = current.G + neighDist;
					neighNode.previousNode = current;
				}

			}

	
			closedNodes.Add(current);
	
			openNodes.Remove(current);
		}

		return null;
	}

	public Node LookForLowerF()
	{
		Node nextNode = openNodes[0];

		foreach (var node in openNodes)
		{
			if (node.F < nextNode.F)
			{
				nextNode = node;
			}
		}
		return nextNode;
	}

	private void ResetAllForNewPath()
	{
		openNodes.Clear();
		closedNodes.Clear();
	}
}
