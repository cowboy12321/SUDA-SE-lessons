#include <iostream>
#include <vector>
#include <queue>

using namespace std;

struct Node {
    int val;
    Node* next;
};

class GraphList {
public:
    GraphList(int v, int e);      // 构造函数
    ~GraphList();                 // 析构函数
    void createGraph();           // 创建图
    void dfs(int n);              // 深度优先搜索
    void bfs(int n);              // 广度优先搜索
    void printGraph();            // 打印图

private:
    vector<Node*> adjList;       // 邻接表
    vector<string> vertex;       // 顶点集合
    vector<int> visited;         // 访问标记
    int v, e;                    // 顶点数和边数
};

GraphList::GraphList(int v, int e) {
    this->v = v;
    this->e = e;
    adjList.resize(v, NULL);  // 初始化邻接表
    visited.resize(v, 0);     // 初始化访问数组
}

GraphList::~GraphList() {
    for (int i = 0; i < v; ++i) {
        Node* p = adjList[i];
        while (p != NULL) {
            Node* temp = p;
            p = p->next;
            delete temp;
        }
    }
}

void GraphList::createGraph() {
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
        Node* node1 = new Node{n, adjList[m]};
        adjList[m] = node1;
        
        Node* node2 = new Node{m, adjList[n]};
        adjList[n] = node2;  
    }
}

void GraphList::dfs(int n) {
    cout << vertex[n] << " ";
    visited[n] = 1;
    Node* p = adjList[n];
    while (p != NULL) {
        if (visited[p->val] == 0) {
            dfs(p->val);
        }
        p = p->next;
    }
}

void GraphList::bfs(int n) {
    queue<int> q;
    q.push(n);
    visited[n] = 1;
    cout << vertex[n] << " ";

    while (!q.empty()) {
        int u = q.front();
        q.pop();
        Node* p = adjList[u];
        while (p != NULL) {
            if (visited[p->val] == 0) {
                visited[p->val] = 1;
                cout << vertex[p->val] << " ";
                q.push(p->val);
            }
            p = p->next;
        }
    }
}

void GraphList::printGraph() {
    for (int i = 0; i < v; ++i) {
        cout << vertex[i] << ": ";
        Node* p = adjList[i];
        while (p != NULL) {
            cout << p->val << " ";
            p = p->next;
        }
        cout << endl;
    }
}

int main() {
    int v, e;
    cout << "请输入图的顶点数和边数：";
    cin >> v >> e;

    GraphList graph(v, e);
    graph.createGraph();

    cout << "邻接表表示的图：\n";
    graph.printGraph();

    cout << "\n深度优先搜索结果：\n";
    graph.dfs(0);  

    fill(graph.visited.begin(), graph.visited.end(), 0);

    cout << "\n广度优先搜索结果：\n";
    graph.bfs(0); 

    return 0;
}

