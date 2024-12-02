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
    	void dfs_path_len_n(int u, vector<int>path, bool visited[]); 
    	int edge[MaxSize][MaxSize];
    	int vertexNum, edgeNum;
};



Graph::Graph(int n, int e) {
    vertexNum = n;
    edgeNum = e;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            edge[i][j] = 0;
        }
    }

    pair<int, int> data[] = { {0,1},{1,2},{0,5},{2,5},{2,3},{5,7},{5,6},{6,7},{5,4},{6,4},{4,8},{3,8} };

    for (int i = 0; i < e; i++) {
        int u = data[i].first, v = data[i].second;
        edge[u][v] = edge[v][u] = 1;
    }
}

void Graph::path_len_n(int u) {

}

void Graph::dfs_path_len_n(int u, vector<int>path, bool visited[]) {

}



int main() {


    int start;

    cin >> start;

    Graph g(9, 12);

    g.path_len_n(start);

}
