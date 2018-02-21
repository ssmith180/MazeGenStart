using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RandomMazeGenerator : MonoBehaviour {

    #region Public/Inspector properties

    public GameObject horizontalWall;
    public GameObject verticalWall;

    #endregion

    #region Enums

    private enum ConnectionState
    {
        Unknown,
        Closed,
        Open
    }

    #endregion

    #region Private member variables

    private ConnectionState[] horizontalConnections;
    private ConnectionState[] verticalConnections;

    private int width = 10;
    private int height = 10;

    private List<int> availableToBeProcessed = new List<int>();
    private List<int> processed = new List<int>();
    private int[] parents;

    #endregion

    #region Unity events

    // Use this for initialization
    void Start()
    {

        availableToBeProcessed.Add(0);

        horizontalConnections = new ConnectionState[(width - 1) * height];
        for (int i = 0; i < horizontalConnections.Length; i++) horizontalConnections[i] = ConnectionState.Unknown;
        verticalConnections = new ConnectionState[width * (height - 1)];
        for (int i = 0; i < verticalConnections.Length; i++) verticalConnections[i] = ConnectionState.Unknown;

        parents = new int[width * height];
        for (int i = 0; i < parents.Length; i++) parents[i] = -1;

        while (availableToBeProcessed.Count > 0)
        {
            ProcessRandomTile();
        }

        //DebugOutput();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y < height - 1)
                {
                    // Draw a vertical block if it's there
                    if (verticalConnections[y * width + x] == ConnectionState.Closed) GameObject.Instantiate(verticalWall, new Vector3(x, 0.5f, y), Quaternion.identity);
                }

                if (x < width - 1)
                {
                    // Draw a horizontal block if it's there
                    if (horizontalConnections[y * (width - 1) + x] == ConnectionState.Closed) GameObject.Instantiate(horizontalWall, new Vector3(x, 0.5f, y), Quaternion.identity);
                }

            }
        }

        for (int x = 0; x < 10; x++)
        {
            GameObject.Instantiate(verticalWall, new Vector3(x, 0.5f, -1), Quaternion.identity);
            GameObject.Instantiate(verticalWall, new Vector3(x, 0.5f, height - 1), Quaternion.identity);
        }

        for (int y = 0; y < 10; y++)
        {
            GameObject.Instantiate(horizontalWall, new Vector3(-1, 0.5f, y), Quaternion.identity);
            GameObject.Instantiate(horizontalWall, new Vector3(width - 1, 0.5f, y), Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Debug

    private void DebugOutput()
    {
        int hChecks = 0;
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y < height - 1)
                {
                    // Draw a vertical block if it's there
                    if (verticalConnections[y * width + x] == ConnectionState.Closed) sb.Append("_"); else sb.Append(" ");
                }
                else
                {
                    sb.Append(" ");
                }

                if (x < width - 1)
                {
                    hChecks++;
                    // Draw a horizontal block if it's there
                    if (horizontalConnections[y * (width - 1) + x] == ConnectionState.Closed) sb.Append("|"); else sb.Append(" ");
                }

            }
            sb.AppendLine();
        }
        print(sb.ToString());

        sb = new StringBuilder();
        foreach (ConnectionState state in horizontalConnections) sb.AppendLine(state.ToString());
        print(sb.ToString());
        print(hChecks);
    }

    #endregion

    #region Process iteration

    private void ProcessConnection(int tileIndex, int candidateTileIndex, int connectionIndex, ConnectionState[] connectionList)
    {
        if (processed.Contains(candidateTileIndex))
        {
            if (parents[tileIndex] != candidateTileIndex)
            {
                // Not possible; block
                connectionList[connectionIndex] = ConnectionState.Closed;
            }
        }
        else
        {
            connectionList[connectionIndex] = ConnectionState.Open;
            parents[candidateTileIndex] = tileIndex;
            if (!availableToBeProcessed.Contains(candidateTileIndex)) availableToBeProcessed.Add(candidateTileIndex);
        }
    }

    private void ProcessRandomTile()
    {
        int tileIndex = availableToBeProcessed[Random.Range(0, availableToBeProcessed.Count)];
        availableToBeProcessed.Remove(tileIndex);
        processed.Add(tileIndex);

        // Now: All connections are available by default, unless they're next to a wall or a processed tile.
        // Any unprocessed tile on the far side of a connection is available to be processed.

        int x = tileIndex % width, y = tileIndex / width;

        if (x > 0)
        {
            int leftIndex = y * width + x - 1;
            int connectionIndex = y * (width - 1) + x - 1;
            ProcessConnection(tileIndex, leftIndex, connectionIndex, horizontalConnections);
        }

        if (x < width - 1)
        {
            int rightIndex = y * width + x + 1;
            int connectionIndex = y * (width - 1) + x;
            ProcessConnection(tileIndex, rightIndex, connectionIndex, horizontalConnections);
        }

        if (y > 0)
        {
            int upIndex = (y - 1) * width + x;
            int connectionIndex = (y - 1) * width + x;
            ProcessConnection(tileIndex, upIndex, connectionIndex, verticalConnections);
        }

        if (y < height - 1)
        {
            int downIndex = (y + 1) * width + x;
            int connectionIndex = y * width + x;
            ProcessConnection(tileIndex, downIndex, connectionIndex, verticalConnections);
        }

    }

    #endregion

}
