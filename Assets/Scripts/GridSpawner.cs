using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject[] roadPiece;
    [SerializeField] private int gridSize = 7; //size of the grid
    private int tileWidth = 60; //size of each grid tile
    private GameObject[,] grid;

    private Dictionary<string, GameObject> pieces = new Dictionary<string, GameObject>(); //keeps track of all 17 version of pieces

    private void Start()
    {
        FillDictionary();

        gridSize += 2; //just to add a border around the grid
        grid = new GameObject[gridSize, gridSize];

        GridGeneration();


    }

    private void GridGeneration()
    {
        GameObject temp;
        int x;
        int y;

        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                //converts row and col numbers into usable x and y coordinates
                x = tileWidth * (col - gridSize / 2);
                y = tileWidth * (row - gridSize / 2);

                if (row == 0 || col == 0 || row == gridSize - 1 || col == gridSize - 1) //coordinates of the border of the grid
                {
                    if (row == gridSize / 2 && col == 0)
                        temp = Instantiate(pieces["deadend-right"], new Vector3(x, 0, y), pieces["deadend-right"].transform.rotation); //left-most piece
                    else if (row == gridSize / 2 && col == gridSize - 1)
                        temp = Instantiate(pieces["deadend-left"], new Vector3(x, 0, y), pieces["deadend-left"].transform.rotation); //right-most piece
                    else if (col == gridSize / 2 && row == 0)
                        temp = Instantiate(pieces["deadend-up"], new Vector3(x, 0, y), pieces["deadend-up"].transform.rotation); //bottom-most piece
                    else if (col == gridSize / 2 && row == gridSize - 1)
                        temp = Instantiate(pieces["deadend-down"], new Vector3(x, 0, y), pieces["deadend-down"].transform.rotation); //top-most piece
                    else
                        temp = Instantiate(roadPiece[1], new Vector3(x, 0, y), roadPiece[1].transform.rotation);


                    temp.transform.SetParent(this.transform.Find("Border").transform);
                    grid[row, col] = temp;
                }
                else if (row == gridSize / 2 || col == gridSize / 2) //makes a cross in the grid, filling it with 4-way pieces
                {
                    temp = Instantiate(pieces["cross"], new Vector3(x, 0, y), pieces["cross"].transform.rotation);
                    temp.transform.SetParent(this.transform);
                    grid[row, col] = temp;
                }
                else //randomly fills grid, delete later
                {
                    //var randomKey = pieces.Keys.ElementAt((int)Random.Range(2, pieces.Keys.Count));

                    //temp = Instantiate(pieces[randomKey], new Vector3(x, 0, y), pieces[randomKey].transform.rotation);
                    //temp.transform.SetParent(this.transform);
                    //grid[row, col] = temp;
                }
            }
        }

        bool filled = false;

        while (!filled)
        {
            //bottom left quadrant
            int tempRow = Random.Range(1, gridSize / 2);
            int tempCol = Random.Range(1, gridSize / 2);

            if (grid[tempRow, tempCol] == null)
                GridGenerator(tempRow, tempCol);

            //bottom right quadrant
            tempRow = Random.Range(gridSize / 2 + 1, gridSize - 1);
            tempCol = Random.Range(1, gridSize / 2);

            if (grid[tempRow, tempCol] == null)
                GridGenerator(tempRow, tempCol);

            //top left quadrant
            tempRow = Random.Range(1, gridSize / 2);
            tempCol = Random.Range(gridSize / 2 + 1, gridSize - 1);

            if (grid[tempRow, tempCol] == null)
                GridGenerator(tempRow, tempCol);

            //top right quadrant
            tempRow = Random.Range(gridSize / 2 + 1, gridSize - 1);
            tempCol = Random.Range(gridSize / 2 + 1, gridSize - 1);

            if (grid[tempRow, tempCol] == null)
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


        //for (var row = 1; row < gridSize - 1; row++)
        //{
        //    for (var col = 1; col < gridSize - 1; col++)
        //    {
        //        x = tileWidth * (col - gridSize / 2);
        //        y = tileWidth * (row - gridSize / 2);

        //        foreach (BoxCollider box in grid[row, col].transform.Find("Colliders").GetComponentsInChildren<BoxCollider>())
        //            box.enabled = false;
        //        //Debug.Log(PieceChecker(x, y));
        //        foreach (BoxCollider box in grid[row, col].transform.Find("Colliders").GetComponentsInChildren<BoxCollider>())
        //            box.enabled = true;
        //    }
        //}





    }

    private void GridGenerator(int row, int col)
    {
        if (grid[row, col] != null)
            return;
            
        //converts row and col numbers into usable x and y coordinates
        int x = tileWidth * (row - gridSize / 2);
        int y = tileWidth * (col - gridSize / 2);

        string tempPiece = PieceChecker(x, y);
        GameObject temp = Instantiate(pieces[tempPiece], new Vector3(x, 0, y), pieces[tempPiece].transform.rotation);
        temp.transform.SetParent(this.transform);
        grid[row, col] = temp;

        int rand = Random.Range(1, 4);
        if (rand == 1)
            GridGenerator(row + 1, col);
        if (rand == 2)
            GridGenerator(row, col + 1);
        if (rand == 3)
            GridGenerator(row - 1, col);
        if (rand == 4)
            GridGenerator(row, col - 1);
    }

    private string PieceChecker(int x, int y)
    {
        int height = 15;
        List<string> possiblePieces = new List<string> { "deadend-down", "deadend-left", "deadend-up", "deadend-right", "straight-vertical", "straight-horizontal", "L-down", "L-left", "L-up", "L-right", "T-down", "T-left", "T-up", "T-right", "cross" };

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.back), out hit, 40, layerMask))
        {
            Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.back) * hit.distance, Color.green, Mathf.Infinity);
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

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.left), out hit, 40, layerMask))
        {
            Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.left) * hit.distance, Color.red, Mathf.Infinity);
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

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.right), out hit, 40, layerMask))
        {
            Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.right) * hit.distance, Color.blue, Mathf.Infinity);
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

        if (Physics.Raycast(new Vector3(x, height, y), transform.TransformDirection(Vector3.forward), out hit, 40, layerMask))
        {
            Debug.DrawRay(new Vector3(x, height, y), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, Mathf.Infinity);
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
        if (possiblePieces.Count > 1)
        {
            possiblePieces.Remove("deadend-down");
            possiblePieces.Remove("deadend-left");
            possiblePieces.Remove("deadend-up");
            possiblePieces.Remove("deadend-right");
        }

        if (possiblePieces.Count <= 0)
            return "border";
        else
            return possiblePieces[Random.Range(0, possiblePieces.Count - 1)];
    }

    private void FillDictionary() //fills the dictionary "pieces" with all 17 possible piece types/rotations
    {
        GameObject temp = Instantiate(roadPiece[0]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("empty", temp); //empty piece
        Destroy(temp);  

        temp = Instantiate(roadPiece[1]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("border", temp); //border piece
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("deadend-down", temp); //deadend piece with road side facing down
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("deadend-left", temp); //deadend piece with road side facing left
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("deadend-up", temp); //deadend piece with road side facing up
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("deadend-right", temp); //deadend piece with road side facing right
        Destroy(temp);

        temp = Instantiate(roadPiece[3]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("straight-vertical", temp); //straight piece with road sides going vertical
        Destroy(temp);

        temp = Instantiate(roadPiece[3]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("straight-horizontal", temp); //straight piece with road sides going horizontal
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("L-down", temp); //L piece with the bottom of the L facing down
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("L-left", temp); //L piece with the bottom of the L facing left
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("L-up", temp); //L piece with the bottom of the L facing up
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("L-right", temp); //L piece with the bottom of the L facing right
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("T-down", temp); //T piece with the bottom of the T facing down
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("T-left", temp); //T piece with the bottom of the T facing left
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("T-up", temp); //T piece with the bottom of the T facing up
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("T-right", temp); //T piece with the bottom of the T facing right
        Destroy(temp);

        temp = Instantiate(roadPiece[6]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("cross", temp); //cross piece
        Destroy(temp);

    }


}
