# Triveni Sangam Water Flow Problem


## Problem Description

### Overview
Triveni Sangam is represented as a **grid of elevations**, where water flows from higher to lower or equal elevations. The task is to determine the direction of water flow for each cell in the grid.

### Rivers
1. **Ganga River**: Represented by the **top and left edges** of the grid.
2. **Yamuna River**: Represented by the **bottom and right edges** of the grid.

### Conditions for Water Flow
1. Water flows **from a cell to its neighboring cells** (up, down, left, right) if the neighboring cell's elevation is **less than or equal** to the current cell's elevation.
2. A cell can flow to:
   - **Ganga River (G)**: If water can reach any cell on the **top or left edge**.
   - **Yamuna River (Y)**: If water can reach any cell on the **bottom or right edge**.
   - **Both Rivers (M)**: If water can flow to both the Ganga and Yamuna rivers.

### Input
- A (ROWS * COLS) grid, where each cell contains an integer representing its elevation.

### Output
- A matrix of the same size where:
  - **`G`** indicates water flows to the **Ganga River** only.
  - **`Y`** indicates water flows to the **Yamuna River** only.
  - **`M`** indicates water flows to **both rivers**.

### Example
**Input Grid**:
```
1 2 2 3 5
3 2 3 4 4
2 4 5 3 1
6 7 1 4 5
5 1 1 2 4
```
**Output Matrix**:
```
G G G G M
G G G M M
G G M Y Y
M M Y Y Y
M Y Y Y Y
```
---

## Approach 1(Brute Force): Backtracking 

The file is `BackTracking.cs`

This approach uses **backtracking** to explore all possible paths from each cell to determine if it can flow to the **Ganga River** (top/left boundary) or the **Yamuna River** (bottom/right boundary). The algorithm independently checks every cell in the grid.

## Steps to Solve

### 1. Initialization
- Define the grid dimensions (\(ROWS\) and \(COLS\)).
- Initialize a result matrix to store:
  - `'G'`: Water flows only to the **Ganga River**.
  - `'Y'`: Water flows only to the **Yamuna River**.
  - `'M'`: Water flows to both rivers.

### 2. Perform DFS
- For each cell in the grid:
  - Initialize two flags (`ganga` and `yamuna`) to track if the cell can flow to the respective rivers.
  - Perform a recursive **DFS** to explore all possible paths.
  - After DFS, classify the cell based on the flags into `'G'`, `'Y'`, or `'M'`.

Here’s how DFS determines reachability:
```csharp
private void Dfs(int[][] heights, int r, int c, int prevHeight, ref bool ganga, ref bool yamuna) {
    if (r < 0 || c < 0) {
        ganga = true; // Reached Ganga boundary
        return;
    }
    if (r >= ROWS || c >= COLS) {
        yamuna = true; // Reached Yamuna boundary
        return;
    }
    if (heights[r][c] > prevHeight) return; // Elevation constraint

    int tmp = heights[r][c];
    heights[r][c] = int.MaxValue; // Mark as visited

    foreach (var dir in directions) {
        Dfs(heights, r + dir[0], c + dir[1], tmp, ref ganga, ref yamuna);
        if (ganga && yamuna) break; // Early exit if both rivers are reachable
    }

    heights[r][c] = tmp; // Restore the cell value
}
```

#### Time Complexity:
- O(m × n × 4^(m × n))
#### Space Complexity:
- O(m × n)
---

## Approach 2(Efficient Solution): Edge-Based Flow with DFS

The file is `dfs.cs`

The efficient solution leverages **Edge-Based Flow** and **Depth-First Search (DFS)** to determine the reachability of each cell to the Ganga and Yamuna rivers. The approach minimizes redundant computations by starting DFS from the **edges** of the grid and propagating water inward, ensuring each cell is processed only once for each river.

## Key Concept: Edge-Based Flow

1. **Ganga River**:
   - Water naturally flows to the Ganga River from:
     - **Top Row** (\( r = 0 \)): All cells in the top row are directly connected to the Ganga River.
     - **Left Column** (\( c = 0 \)): All cells in the left column are directly connected to the Ganga River.

2. **Yamuna River**:
   - Water naturally flows to the Yamuna River from:
     - **Bottom Row** (\( r = ROWS - 1 \)): All cells in the bottom row are directly connected to the Yamuna River.
     - **Right Column** (\( c = COLS - 1 \)): All cells in the right column are directly connected to the Yamuna River.

## Step-by-Step Explanation

### Step 1: Initialize Reachability Matrices
   - Create two ( ROWS * COLS) boolean matrices:
  1. `ganga`: Tracks whether a cell can flow to the Ganga River.
  2. `yamuna`: Tracks whether a cell can flow to the Yamuna River.

```csharp
int ROWS = heights.Length, COLS = heights[0].Length;
bool[,] ganga = new bool[ROWS, COLS];
bool[,] yamuna = new bool[ROWS, COLS];
```
### Step 2: Perform DFS for River Reachability

#### Ganga River DFS
1. **Top Row** (\( r = 0 \), all columns):
   - Start DFS from each cell in the top row to mark cells reachable by the Ganga River.
```csharp
   for (int c = 0; c < COLS; c++) {
       Dfs(0, c, ganga, heights); // Top edge (Ganga)
   }
```
2. **Left Column** (c=0, all rows):
   - Start DFS from each cell in the left column to mark cells reachable by the Ganga River.
```csharp
  for (int r = 0; r < ROWS; r++) {
    Dfs(r, 0, ganga, heights); // Left edge (Ganga)
}
```
#### Yamuna River DFS

1. **Bottom Row** (\( r = ROWS - 1 \), all columns):
   - Perform DFS for all cells in the bottom row to mark them as reachable by the Yamuna River.
   - These cells are the natural entry points for the Yamuna River as they are part of its boundary.
   - Code snippet:
```csharp
     for (int c = 0; c < COLS; c++) {
         Dfs(ROWS - 1, c, yamuna, heights); // Bottom edge (Yamuna)
     }
```
2. **Right Column** (\( c = COLS - 1 \), all rows):
   - Perform DFS for all cells in the right column to mark them as reachable by the Yamuna River.
   - These cells are part of the natural boundary for the Yamuna River, ensuring direct water flow.
   - Code snippet:
 ```csharp
     for (int r = 0; r < ROWS; r++) {
         Dfs(r, COLS - 1, yamuna, heights); // Right edge (Yamuna)
     }
 ```
### Step 3: Define DFS Propagation

The **DFS function** is responsible for marking cells as reachable and propagating reachability to their valid neighbors. It ensures that water flows only downhill or remains level and avoids revisiting already processed cells.

#### Conditions for DFS Propagation:
For each cell \((r, c)\), the function propagates its reachability to its neighboring cells \((nr, nc)\) if:

1. **Within Grid Bounds**:
   - The neighboring cell \((nr, nc)\) must be within the valid boundaries of the grid.
   ```csharp
   if (nr >= 0 && nr < ROWS && nc >= 0 && nc < COLS)
   ```
2. **Not Visited**:
   - The neighboring cell must not have been marked as reachable for the current river in this DFS.
   ```csharp
   && !river[nr, nc]
   ```
3. **Valid Elevation Condition**:
   - The elevation of the neighboring cell must be greater than or equal to the current cell's elevation to allow water flow.
  ```csharp
&& heights[nr][nc] >= heights[r][c]
  ```
Full DFS Function:
```csharp
private void Dfs(int r, int c, bool[,] river, int[][] heights) {
    river[r, c] = true; // Mark cell as reachable
    foreach (var dir in directions) {
        int nr = r + dir[0], nc = c + dir[1];
        if (nr >= 0 && nr < heights.Length && nc >= 0 && 
            nc < heights[0].Length && !river[nr, nc] && 
            heights[nr][nc] >= heights[r][c]) {
            Dfs(nr, nc, river, heights);
        }
    }
}
```
#### Time Complexity:
- O(m × n)
#### Space Complexity:
- O(m × n)

---
### Key Differences between Backtracking(bruteforce) and Depth First Search(efficient)

| **Aspect**            | **Brute Force**                      | **Efficient**                          |
|------------------------|---------------------------------------|-----------------------------------------|
| **Path Exploration**   | Exhaustive, explores all possibilities | Systematic, edge-based propagation      |
| **Redundancy**         | Revisits cells multiple times         | Avoids revisits with reachability matrices |
| **Reusability**        | No reuse of intermediate results      | Reuses results through matrices         |
| **Time Complexity**    | 0(4^(m*n))              | O(m × n)                     |
| **Space Complexity**   | High due to recursion and visited arrays | \( O(m \times n) \)                     |
| **Scalability**        | Poor, inefficient for large grids     | Highly scalable, efficient              |


  


