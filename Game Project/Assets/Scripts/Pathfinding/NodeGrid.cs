/**
 * Represent a grid of nodes we can search paths on.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *   
 * Author: Ronen Ness.
 * Since: 2016. 
 * 
 * Edited by Cody LaFlamme, 2018
*/
using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    /// <summary>
    /// A 2D grid of nodes we use to find path.
    /// The grid mark which tiles are walkable and which are not.
    /// </summary>
    public class NodeGrid
    {
        // nodes in grid
        public Node[,] nodes;

        // grid size
        int gridSizeX, gridSizeY;

        /// <summary>
        /// Create a new grid with tile prices.
        /// </summary>
        /// <param name="width">PathGrid width.</param>
        /// <param name="height">PathGrid height.</param>
        /// <param name="tiles_costs">A 2d array, matching width and height, of tile prices.
        ///     0.0f = Unwalkable tile.
        ///     1.0f = Normal tile.
        ///     > 1.0f = costy tile.
        ///     < 1.0f = cheap tile.
        /// </param>
        public NodeGrid(int width, int height, float[,] tiles_costs)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(tiles_costs[x, y], x, y);
                }
            }
        }

        /// <summary>
        /// Create a new grid without tile prices, eg with just walkable / unwalkable tiles.
        /// </summary>
        /// <param name="width">PathGrid width.</param>
        /// <param name="height">PathGrid height.</param>
        /// <param name="walkable_tiles">A 2d array, matching width and height, which tiles are walkable and which are not.</param>
        public NodeGrid(int width, int height, bool[,] walkable_tiles)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        /// <summary>
        /// Get all the walkable neighbors of a given tile in the grid.
        /// </summary>
        /// <param name="node">Node to get neighbors for.</param>
        /// <returns>List of node neighbors.</returns>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++){
                for (int y = -1; y <= 1; y++){
					if (x == 0 && y == 0)
						continue; //tile is looking at itself; cannot be a neighbor

					int checkX = node.gridX + x;
					int checkY = node.gridY + y;

					//ensure x and y are in bounds
					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
						//if we're looking diagonally, and at least one of the adjacent orthagonal tiles is not walkable
						if (x!=0 && y!=0 && !(nodes[checkX, node.gridY].walkable && nodes[node.gridX, checkY].walkable))
							continue;
						neighbours.Add(nodes[checkX, checkY]);
					}
                }
            }
            return neighbours;
        }
    }
}