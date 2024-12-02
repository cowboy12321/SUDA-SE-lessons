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
    GraphList(int v, int e);      // ���캯��
    ~GraphList();                 // ��������
    void createGraph();           // ����ͼ
    void dfs(int n);              // �����������
    void bfs(int n);              // �����������
    void printGraph();            // ��ӡͼ

private:
    vector<Node*> adjList;       // �ڽӱ�
    vector<string> vertex;       // ���㼯��
    vector<int> visited;         // ���ʱ��
    int v, e;                    // �������ͱ���
};

GraphList::GraphList(int v, int e) {
    this->v = v;
    this->e = e;
    adjList.resize(v, NULL);  // ��ʼ���ڽӱ�
    visited.resize(v, 0);     // ��ʼ����������
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
    cout << "������ͼ�Ķ������ͱ�����";
    cin >> v >> e;

    GraphList graph(v, e);
    graph.createGraph();

    cout << "�ڽӱ��ʾ��ͼ��\n";
    graph.printGraph();

    cout << "\n����������������\n";
    graph.dfs(0);  

    fill(graph.visited.begin(), graph.visited.end(), 0);

    cout << "\n����������������\n";
    graph.bfs(0); 

    return 0;
}

