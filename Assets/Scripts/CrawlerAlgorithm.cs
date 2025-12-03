using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAlgorithm : MazeLogic
{
    public override void GenerateMaps()
    {
        bool done = false;
        int x = width / 2;
        int z = depth / 2;

        while (!done) {
            map[x, z] = 0;

            Debug.Log("Setting map[" + x + ", " + z + "] = " + map[x, z]);

            // Random Crawling Avoid Diagonal Movement
            // if (Random.Range(0, 100) < 50)
            //     x += Random.Range(-1, 2);
            // else
            //     z += Random.Range(-1, 2);
            // Update x or z randomly but keep within inner bounds (1 to width-2 and 1 to depth-2)
            if (Random.Range(0, 100) < 50)
                x = Mathf.Clamp(x + Random.Range(-1, 2), 1, width - 2);
            else
                z = Mathf.Clamp(z + Random.Range(-1, 2), 1, depth - 2);

            // Continue until the crawler hits the edge
            // if (x < 1 || x >= width || z < 1 || z >= depth)
            //     done = true;
            // else
            //     done = false;
            done = (x <= 1 || x >= width - 2 || z <= 1 || z >= depth - 2);
        }
    }
}
