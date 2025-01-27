// Efficient Solution
using System;
using System.Collections.Generic;

public class Solution {
    private int[][] directions = new int[][] { 
        new int[] { 1, 0 }, new int[] { -1, 0 }, 
        new int[] { 0, 1 }, new int[] { 0, -1 } 
    };

    public char[][] GangaYamuna(int[][] heights) {
        int ROWS = heights.Length, COLS = heights[0].Length;
        bool[,] ganga = new bool[ROWS, COLS];
        bool[,] yamuna = new bool[ROWS, COLS];

        // Perform DFS for Ganga and Yamuna edges
        for (int c = 0; c < COLS; c++) {
            Dfs(0, c, ganga, heights);        // Top edge (Ganga)
            Dfs(ROWS - 1, c, yamuna, heights); // Bottom edge (Yamuna)
        }
        for (int r = 0; r < ROWS; r++) {
            Dfs(r, 0, ganga, heights);        // Left edge (Ganga)
            Dfs(r, COLS - 1, yamuna, heights); // Right edge (Yamuna)
        }

        // Generate result matrix
        char[][] result = new char[ROWS][];
        for (int r = 0; r < ROWS; r++) {
            result[r] = new char[COLS];
            for (int c = 0; c < COLS; c++) {
                if (ganga[r, c] && yamuna[r, c]) {
                    result[r][c] = 'M'; // Merge point
                } else if (ganga[r, c]) {
                    result[r][c] = 'G'; // Ganga only
                } else if (yamuna[r, c]) {
                    result[r][c] = 'Y'; // Yamuna only
                }
            }
        }

        return result;
    }

    private void Dfs(int r, int c, bool[,] river, int[][] heights) {
        river[r, c] = true;
        foreach (var dir in directions) {
            int nr = r + dir[0], nc = c + dir[1];
            if (nr >= 0 && nr < heights.Length && nc >= 0 && 
                nc < heights[0].Length && !river[nr, nc] && 
                heights[nr][nc] >= heights[r][c]) {
                Dfs(nr, nc, river, heights);
            }
        }
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

        // Print the result matrix
        Console.WriteLine("Result Matrix:");
        for (int r = 0; r < result.Length; r++) {
            Console.WriteLine(string.Join(" ", result[r]));
        }
    }
}