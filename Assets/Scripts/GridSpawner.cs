using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask; //layer that raycasts will check for other grid pieces, currently should be "ground"
    [SerializeField] private int inputGridSize = 15; //size of the grid, works best with odd numbers
    private int gridSize = 15; //size of the grid, works best with odd numbers
    private int tileWidth = 60; //size of each grid tile
    private GameObject[,] grid; //where entire grid of gameobjects is stored
    [FormerlySerializedAs("Seed")] [SerializeField] private int seed;
    [InspectorButton("RandomizeGrid", ButtonWidth = 100)] [SerializeField] private bool randomizeGrid;
    [InspectorButton("DeleteGrid", ButtonWidth = 50)] [SerializeField] private bool delete;


    private class Piece
    {
        //class for storing a pieces gameobject and rotation
        public GameObject piece { get; set; }
        public Quaternion rotation { get; set; }
    }

    private Dictionary<string, Piece> pieces = new Dictionary<string, Piece>(); //keeps track of all 16 version of pieces

    private void Start()
    {
        gridSize = inputGridSize + 2; //just to add a border around the grid

        RandomizeGrid();
    }

    private void DeleteGrid()
    {
        if (grid != null)
        {
            for (var row = 0; row < gridSize; row++)
            {
                for (var col = 0; col < gridSize; col++)
                {
                    DestroyImmediate(grid[row, col]);
                }
            }
        }
    }

    private void RandomizeGrid() // this can be called elsewhere now
    {
        FillDictionary();

        DeleteGrid();

        gridSize = inputGridSize + 2; //increase gridsize changes, update here
        grid = new GameObject[gridSize, gridSize];

        Random.InitState(seed);

        GameObject temp;
        int x;
        int y;

        //these for-loops are for filling the border of the grid
        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                //converts row and col numbers into usable x and y coordinates
                x = tileWidth * (col - gridSize / 2);
                y = tileWidth * (row - gridSize / 2);

                if (row == 0 || col == 0 || row == gridSize - 1 ||
                    col == gridSize - 1) //coordinates of the border of the grid
                {
                    if (row == gridSize / 2 && col == 0)
                    {
                        temp = Instantiate(pieces["deadend-right"].piece, new Vector3(x, 0, y),
                            pieces["deadend-right"].rotation, this.transform.Find("Border").transform); //left-most piece
                        temp.GetComponentInParent<RoadPiece>().value = 0;
                    }
                    else if (row == gridSize / 2 && col == gridSize - 1)
                    {
                        temp = Instantiate(pieces["deadend-left"].piece, new Vector3(x, 0, y),
                            pieces["deadend-left"].rotation, this.transform.Find("Border").transform); //right-most piece
                        temp.GetComponentInParent<RoadPiece>().value = 0;
                    }
                    else if (col == gridSize / 2 && row == 0)
                    {
                        temp = Instantiate(pieces["deadend-up"].piece, new Vector3(x, 0, y), pieces["deadend-up"].rotation,
                            this.transform.Find("Border").transform); //bottom-most piece
                        temp.GetComponentInParent<RoadPiece>().value = 0;
                    }
                    else if (col == gridSize / 2 && row == gridSize - 1)
                    {
                        temp = Instantiate(pieces["deadend-down"].piece, new Vector3(x, 0, y),
                            pieces["deadend-down"].rotation, this.transform.Find("Border").transform); //top-most piece
                        temp.GetComponentInParent<RoadPiece>().value = 0;
                    }
                    else
                        temp = Instantiate(pieces["border"].piece, new Vector3(x, 0, y), pieces["border"].rotation,
                            this.transform.Find("Border").transform);

                    grid[row, col] = temp;
                }
            }
        }

        bool filled = false;

        while (!filled) //will run until every spot is filled
        {
            //picks random coordinate
            int tempRow = Random.Range(1, gridSize - 1);
            int tempCol = Random.Range(1, gridSize - 1);

            if (grid[tempRow, tempCol] == null) //makes sure random coordinate is not already occupied
                GridGenerator(tempRow, tempCol);

            //check if entire grid is filled
            filled = true;
            for (var row = 1; row < gridSize - 1; row++)
            {
                for (var col = 1; col < gridSize - 1; col++)
                {
                    if (grid[row, col] == null)
                        filled = false;
                }
            }
        }
    }

    private void GridGenerator(int row, int col)
    {
        while (true) // No need to do recursive calls
        //Juniper waz here 😎🤘
        {
            if (grid[row, col] != null) return;

            //converts row and col numbers into usable x and y coordinates
            int x = tileWidth * (row - gridSize / 2);
            int y = tileWidth * (col - gridSize / 2);

            string tempPiece = PieceChecker(x, y);
            GameObject temp = Instantiate(pieces[tempPiece].piece, new Vector3(x, 0, y), pieces[tempPiece].rotation, this.transform);
            grid[row, col] = temp;

            int rand = Random.Range(1, 4);
            switch (rand) // i refactored this for u :) -juniper 
            {
                case 1:
                    row += 1;
                    continue;
                case 2:
                    col += 1;
                    continue;
                case 3:
                    row -= 1;
                    continue;
                case 4:
                    col -= 1;
                    continue;
            }

            break;
        }
    }
    
    private string PieceChecker(int x, int y)
    {
        //list of all possible pieces
        List<string> possiblePieces = new List<string> { "deadend-down", "deadend-left", "deadend-up", "deadend-right", "straight-vertical", "straight-horizontal", "L-down", "L-left", "L-up", "L-right", "T-down", "T-left", "T-up", "T-right", "cross" };

        int height = 15;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.back), out hit, 40, layerMask)) //checks back direction and removes possible pieces depending on if there is a wall or open road in that direction
        {
            //Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.back) * hit.distance, Color.green, Mathf.Infinity);
            if (hit.transform.tag == "Wall")
            {
                //Debug.Log("Hitting a Back Wall");
                possiblePieces.Remove("deadend-down");
                possiblePieces.Remove("straight-vertical");
                possiblePieces.Remove("L-down");
                possiblePieces.Remove("L-left");
                possiblePieces.Remove("T-down");
                possiblePieces.Remove("T-left");
                possiblePieces.Remove("T-right");
                possiblePieces.Remove("cross");
            }
            if (hit.transform.tag == "Road")
            {
                //Debug.Log("Hitting a Back Road");
                possiblePieces.Remove("deadend-left");
                possiblePieces.Remove("deadend-up");
                possiblePieces.Remove("deadend-right");
                possiblePieces.Remove("straight-horizontal");
                possiblePieces.Remove("L-up");
                possiblePieces.Remove("L-right");
                possiblePieces.Remove("T-up");
            }
        }
        //else
            //Debug.Log("Hitting nothing back");

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.left), out hit, 40, layerMask)) //checks left direction and removes possible pieces depending on if there is a wall or open road in that direction
        {
            //Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.left) * hit.distance, Color.red, Mathf.Infinity);
            if (hit.transform.tag == "Wall")
            {
                //Debug.Log("Hitting a Left Wall");
                possiblePieces.Remove("deadend-left");
                possiblePieces.Remove("straight-horizontal");
                possiblePieces.Remove("L-left");
                possiblePieces.Remove("L-up");
                possiblePieces.Remove("T-down");
                possiblePieces.Remove("T-left");
                possiblePieces.Remove("T-up");
                possiblePieces.Remove("cross");
            }
            if (hit.transform.tag == "Road")
            {
                //Debug.Log("Hitting a Left Road");
                possiblePieces.Remove("deadend-down");
                possiblePieces.Remove("deadend-up");
                possiblePieces.Remove("deadend-right");
                possiblePieces.Remove("straight-vertical");
                possiblePieces.Remove("L-down");
                possiblePieces.Remove("L-right");
                possiblePieces.Remove("T-right");
            }  
        }
        //else
            //Debug.Log("Hitting nothing left");

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.right), out hit, 40, layerMask)) //checks right direction and removes possible pieces depending on if there is a wall or open road in that direction
        {
            //Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.right) * hit.distance, Color.blue, Mathf.Infinity);
            if (hit.transform.tag == "Wall")
            {
                //Debug.Log("Hitting a Right Wall");
                possiblePieces.Remove("deadend-right");
                possiblePieces.Remove("straight-horizontal");
                possiblePieces.Remove("L-down");
                possiblePieces.Remove("L-right");
                possiblePieces.Remove("T-down");
                possiblePieces.Remove("T-up");
                possiblePieces.Remove("T-right");
                possiblePieces.Remove("cross");
            }  
            if (hit.transform.tag == "Road")
            {
                //Debug.Log("Hitting a Right Road");
                possiblePieces.Remove("deadend-down");
                possiblePieces.Remove("deadend-left");
                possiblePieces.Remove("deadend-up");
                possiblePieces.Remove("straight-vertical");
                possiblePieces.Remove("L-left");
                possiblePieces.Remove("L-up");
                possiblePieces.Remove("T-left");
            }  
        }
        //else
            //Debug.Log("Hitting nothing right");

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.forward), out hit, 40, layerMask)) //checks forward direction and removes possible pieces depending on if there is a wall or open road in that direction
        {
            //Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, Mathf.Infinity);
            if (hit.transform.tag == "Wall")
            {
                //Debug.Log("Hitting a Forward Wall");
                possiblePieces.Remove("deadend-up");
                possiblePieces.Remove("straight-vertical");
                possiblePieces.Remove("L-up");
                possiblePieces.Remove("L-right");
                possiblePieces.Remove("T-left");
                possiblePieces.Remove("T-up");
                possiblePieces.Remove("T-right");
                possiblePieces.Remove("cross");
            }
            if (hit.transform.tag == "Road")
            {
                //Debug.Log("Hitting a Forward Road");
                possiblePieces.Remove("deadend-down");
                possiblePieces.Remove("deadend-left");
                possiblePieces.Remove("deadend-right");
                possiblePieces.Remove("straight-horizontal");
                possiblePieces.Remove("L-down");
                possiblePieces.Remove("L-left");
                possiblePieces.Remove("T-down");
            }    
        }
        //else
            //Debug.Log("Hitting nothing forward");
        
        if (possiblePieces.Count > 1)//unless the only thing left in the list is a deadend piece, remove the deadend pieces so they dont spawn
        {
            possiblePieces.Remove("deadend-down");
            possiblePieces.Remove("deadend-left");
            possiblePieces.Remove("deadend-up");
            possiblePieces.Remove("deadend-right");
        }
        if (possiblePieces.Count > 1)//unless the only thing left in the list is a L piece, remove the L pieces so they dont spawn
        {
            possiblePieces.Remove("L-down");
            possiblePieces.Remove("L-left");
            possiblePieces.Remove("L-up");
            possiblePieces.Remove("L-right");
        }

        if (possiblePieces.Count <= 0)//if there are no possible pieces, spawn border piece
            return "border";
        else
            return possiblePieces[Random.Range(0, possiblePieces.Count - 1)];//randomly choose from possible pieces and return that piece
    }

    private void FillDictionary() //fills the dictionary "pieces" with all 16 possible piece types/rotations
    {
        pieces = new Dictionary<string, Piece>(); // clean out old dict

        Piece temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[0];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("border", temp); //border piece

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[1];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("deadend-down", temp); //deadend piece with road side facing down

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[1];
        temp.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("deadend-left", temp); //deadend piece with road side facing left

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[1];
        temp.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("deadend-up", temp); //deadend piece with road side facing up

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[1];
        temp.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("deadend-right", temp); //deadend piece with road side facing right

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[2];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("straight-vertical", temp); //straight piece with road sides going vertical

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[2];
        temp.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("straight-horizontal", temp); //straight piece with road sides going horizontal

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[3];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("L-down", temp); //L piece with the bottom of the L facing down

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[3];
        temp.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("L-left", temp); //L piece with the bottom of the L facing left

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[3];
        temp.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("L-up", temp); //L piece with the bottom of the L facing up

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[3];
        temp.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("L-right", temp); //L piece with the bottom of the L facing right

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[4];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("T-down", temp); //T piece with the bottom of the T facing down

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[4];
        temp.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("T-left", temp); //T piece with the bottom of the T facing left

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[4];
        temp.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("T-up", temp); //T piece with the bottom of the T facing up

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[4];
        temp.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("T-right", temp); //T piece with the bottom of the T facing right

        temp = new Piece();
        temp.piece = RPSingleton.Access.rp.gcm_RoadPieces[5];
        temp.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("cross", temp); //cross piece

    }


}
