#include <iostream>
#include <vector>
using namespace std;

const int MaxSize = 100;

class Graph
{
public:
    Graph(int n, int e);
    void path_len_n(int u);

private:
    void dfs_path_len_n(int u, vector<int> path);
    int edge[MaxSize][MaxSize];
    int vertexNum, edgeNum;
    bool visited[MaxSize]; 
};

Graph::Graph(int n, int e)
{
    vertexNum = n;
    edgeNum = e;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            edge[i][j] = 0;
        }
    }
    pair<int, int> data[] = { {0,1},{1,2},{0,5},{2,5},{2,3},{5,7},{5,6},{6,7},{5,4},{6,4},{4,8},{3,8} };
    for (int i = 0; i < e; i++)
    {
        int u = data[i].first, v = data[i].second;
        edge[u][v] = edge[v][u] = 1;
    }
    for (int i = 0; i < MaxSize; i++)
    {
        visited[i] = false;
    }
}

void Graph::path_len_n(int u)
{
    vector<int> path;
    dfs_path_len_n(u, path);
}

void Graph::dfs_path_len_n(int u, vector<int> path)
{
    path.push_back(u);
    visited[u] = true;
    if (path.size() == vertexNum)
    {
    	int t = 8;
    	cout << "[";
        for (auto p : path)
        {
            cout << p;
            if(t--)
        	{
        		cout << ", ";
			}
        }
        cout <<"]";
        cout << endl;
        visited[u] = false;
        return;
    }

    for (int i = 0; i < vertexNum; i++)
    {
        if (i == u)
        {
            continue;
        }
        if (visited[i] == false && edge[u][i] == 1)
        {
            bool a = false;
            for (auto p: path)
            {
                if (p == i)
                {
                    a = true;
                    break;
                }
            }
            if (!a)
            {
                dfs_path_len_n(i, path);
            }
        }
    }
    visited[u] = false;
}

int main()
{
    int start;
    cin >> start;
    Graph g(9, 12);
    g.path_len_n(start);

    return 0;
}
