#include <iostream>
#include <vector>
#include <cstring>

using namespace std;

class GraphMatrix {
public:
    GraphMatrix(int v, int e);  // ���캯��
    ~GraphMatrix();             // ��������
    void createGraph();         // ����ͼ
    void dfs(int n);            // �����������
    void bfs(int n);            // �����������
    void printGraph();          // ��ӡͼ

private:
    vector<vector<int>> edge;  // �ڽӾ���
    vector<string> vertex;     // ���㼯��
    vector<int> visited;       // ���ʱ��
    int v, e;                  // �������ͱ���
};


GraphMatrix::GraphMatrix(int v, int e) {
    this->v = v;
    this->e = e;
    edge.resize(v, vector<int>(v, 0));  // ��ʼ���ڽӾ���Ϊ0
    visited.resize(v, 0);                // ��ʼ����������
}


GraphMatrix::~GraphMatrix() {
}

void GraphMatrix::createGraph() {
    cout << "�����붥����Ϣ��\n";
    for (int i = 0; i < v; ++i) {
        string vertexName;
        cin >> vertexName;
        vertex.push_back(vertexName);
    }

    cout << "������ߵ���Ϣ�����磺0 1 ��ʾ����0�Ͷ���1��һ���ߣ���\n";
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
    cout << "������ͼ�Ķ������ͱ�����";
    cin >> v >> e;

    GraphMatrix graph(v, e);
    graph.createGraph();

    cout << "�ڽӾ����ʾ��ͼ��\n";
    graph.printGraph();

    cout << "\n����������������\n";
    graph.dfs(0); 

    fill(graph.visited.begin(), graph.visited.end(), 0);

    cout << "\n����������������\n";
    graph.bfs(0);  

    return 0;
}

