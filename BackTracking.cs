// Bruteforce solution
using System;
using System.Collections.Generic;

public class Solution {
    int ROWS, COLS;
    int[][] directions = new int[][] {
        new int[] {1, 0}, new int[] {-1, 0}, new int[] {0, 1}, new int[] {0, -1}
    };

    public char[][] GangaYamuna(int[][] heights) {
        ROWS = heights.Length;
        COLS = heights[0].Length;

        // Initialize result matrix
        char[][] result = new char[ROWS][];
        for (int i = 0; i < ROWS; i++) {
            result[i] = new char[COLS];
        }

        // Check reachability for each cell
        for (int r = 0; r < ROWS; r++) {
            for (int c = 0; c < COLS; c++) {
                bool ganga = false, yamuna = false;
                Dfs(heights, r, c, int.MaxValue, ref ganga, ref yamuna);

                if (ganga && yamuna) {
                    result[r][c] = 'M'; // Merge point
                } else if (ganga) {
                    result[r][c] = 'G'; // Ganga only
                } else if (yamuna) {
                    result[r][c] = 'Y'; // Yamuna only
                }
            }
        }

        return result;
    }

    private void Dfs(int[][] heights, int r, int c, int prevHeight, ref bool ganga, ref bool yamuna) {
        // Termination conditions
        if (r < 0 || c < 0) {
            ganga = true; // Reached Ganga boundary
            return;
        }
        if (r >= ROWS || c >= COLS) {
            yamuna = true; // Reached Yamuna boundary
            return;
        }
        if (heights[r][c] > prevHeight) {
            return; // Elevation constraint
        }

        // Temporarily mark the cell as visited
        int tmp = heights[r][c];
        heights[r][c] = int.MaxValue;

        // Explore all four directions
        foreach (var dir in directions) {
            Dfs(heights, r + dir[0], c + dir[1], tmp, ref ganga, ref yamuna);
            if (ganga && yamuna) {
                break; // Stop further exploration if both rivers are reachable
            }
        }

        // Restore the cell's value
        heights[r][c] = tmp;
    }

    public static void Main(string[] args) {
        // Input heights matrix
        int[][] heights = new int[][] {
            new int[] {1, 2, 2, 3, 5},
            new int[] {3, 2, 3, 4, 4},
            new int[] {2, 4, 5, 3, 1},
            new int[] {6, 7, 1, 4, 5},
            new int[] {5, 1, 1, 2, 4}
        };

        Solution solution = new Solution();
        char[][] result = solution.GangaYamuna(heights);

        // Output the result matrix
        Console.WriteLine("Result Matrix:");
        for (int r = 0; r < result.Length; r++) {
            Console.WriteLine(string.Join(" ", result[r]));
        }
    }
}