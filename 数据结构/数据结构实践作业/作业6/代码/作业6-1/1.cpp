#include <iostream>
#include <vector>
#include <cstring>

using namespace std;

class GraphMatrix {
public:
    GraphMatrix(int v, int e);  // 构造函数
    ~GraphMatrix();             // 析构函数
    void createGraph();         // 创建图
    void dfs(int n);            // 深度优先搜索
    void bfs(int n);            // 广度优先搜索
    void printGraph();          // 打印图

private:
    vector<vector<int>> edge;  // 邻接矩阵
    vector<string> vertex;     // 顶点集合
    vector<int> visited;       // 访问标记
    int v, e;                  // 顶点数和边数
};


GraphMatrix::GraphMatrix(int v, int e) {
    this->v = v;
    this->e = e;
    edge.resize(v, vector<int>(v, 0));  // 初始化邻接矩阵为0
    visited.resize(v, 0);                // 初始化访问数组
}


GraphMatrix::~GraphMatrix() {
}

void GraphMatrix::createGraph() {
    cout << "请输入顶点信息：\n";
    for (int i = 0; i < v; ++i) {
        string vertexName;
        cin >> vertexName;
        vertex.push_back(vertexName);
    }

    cout << "请输入边的信息（例如：0 1 表示顶点0和顶点1有一条边）：\n";
    for (int i = 0; i < e; ++i) {
        int m, n;
        cin >> m >> n;
        edge[m][n] = 1;
        edge[n][m] = 1;  
    }
}

void GraphMatrix::dfs(int n) {
    cout << vertex[n] << " ";
    visited[n] = 1; 
    for (int i = 0; i < v; ++i) {
        if (edge[n][i] == 1 && visited[i] == 0) {
            dfs(i);
        }
    }
}

void GraphMatrix::bfs(int n) {
    vector<int> queue;
    queue.push_back(n);
    visited[n] = 1;
    cout << vertex[n] << " ";

    while (!queue.empty()) {
        int u = queue[0];
        queue.erase(queue.begin());
        for (int i = 0; i < v; ++i) {
            if (edge[u][i] == 1 && visited[i] == 0) {
                visited[i] = 1;
                cout << vertex[i] << " ";
                queue.push_back(i);
            }
        }
    }
}

void GraphMatrix::printGraph() {
    for (int i = 0; i < v; ++i) {
        for (int j = 0; j < v; ++j) {
            cout << edge[i][j] << " ";
        }
        cout << endl;
    }
}

int main() {
    int v, e;
    cout << "请输入图的顶点数和边数：";
    cin >> v >> e;

    GraphMatrix graph(v, e);
    graph.createGraph();

    cout << "邻接矩阵表示的图：\n";
    graph.printGraph();

    cout << "\n深度优先搜索结果：\n";
    graph.dfs(0); 

    fill(graph.visited.begin(), graph.visited.end(), 0);

    cout << "\n广度优先搜索结果：\n";
    graph.bfs(0);  

    return 0;
}

