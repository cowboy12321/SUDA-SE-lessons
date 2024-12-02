#include <iostream>
#include <vector>
#include <queue>
#include <unordered_map>
#include <climits>

using namespace std;

class SocialNetwork {
public:
    map<string, int> name_to_id;
    vector<string> id_to_name;  // ����ӳ��
    vector<vector<int>> adj;    // �洢��idΪi��������ʶ���˵ı��
    
    int num; // �罻�����е�����

    SocialNetwork() : num(0) {}

    void add(const string& name) {
        if (name_to_id.find(name) == name_to_id.end()) {
            name_to_id[name] = num_people;
            id_to_name.push_back(name);
            adj.push_back({});
            num_people++;
        }
    }
    
    void addConnection(const string& person1, const string& person2) {
        int id1 = name_to_id[person1];
        int id2 = name_to_id[person2];
        adj[id1].push_back(id2);
        adj[id2].push_back(id1);  // ����ͼ
    }

    // �ҵ�������֮������·��
    vector<string> findShortestPath(const string& start, const string& end) {
        int start_id = name_to_id[start];
        int end_id = name_to_id[end];
        
        // �����������ͬ
        if (start_id == end_id) {
            return {start};
        }

        vector<int> dist(num_people, INT_MAX);
        vector<int> prev(num_people, -1);
        queue<int> q;
        
        dist[start_id] = 0;
        q.push(start_id);
        
        while (!q.empty()) {
            int current = q.front();
            q.pop();
            
            // �����������ڵĽڵ�
            for (int neighbor : adj[current]) {
                if (dist[neighbor] == INT_MAX) {  // �����û�����ʹ�
                    dist[neighbor] = dist[current] + 1;
                    prev[neighbor] = current;
                    q.push(neighbor);
                    
                    // һ���ҵ�Ŀ�ֹ꣬ͣ����
                    if (neighbor == end_id) {
                        vector<string> path;
                        for (int at = end_id; at != -1; at = prev[at]) {
                            path.push_back(id_to_name[at]);
                        }
                        reverse(path.begin(), path.end());
                        return path;
                    }
                }
            }
        }
        
        // ���û���ҵ�·��
        return {};
    }
    
    // ��ӡ�����˵���Ϣ
    void printNetwork() {
        for (int i = 0; i < num_people; ++i) {
            cout << id_to_name[i] << " -> ";
            for (int neighbor : adj[i]) {
                cout << id_to_name[neighbor] << " ";
            }
            cout << endl;
        }
    }
};

int main() {
    SocialNetwork sn;
    
    // �����
    sn.addPerson("Alice");
    sn.addPerson("Bob");
    sn.addPerson("Charlie");
    sn.addPerson("David");
    sn.addPerson("Eve");
    
    // ��ӹ�ϵ
    sn.addConnection("Alice", "Bob");
    sn.addConnection("Bob", "Charlie");
    sn.addConnection("Charlie", "David");
    sn.addConnection("David", "Eve");
    
    // ��ӡ�罻����
    cout << "�罻����ͼ��" << endl;
    sn.printNetwork();
    
    // ����������֮������·��
    cout << "��ѯ Alice �� Eve ֮������·����" << endl;
    vector<string> path = sn.findShortestPath("Alice", "Eve");
    
    if (path.empty()) {
        cout << "û���ҵ�·����" << endl;
    } else {
        cout << "���·���ǣ�";
        for (const string& name : path) {
            cout << name << " ";
        }
        cout << endl;
    }

    return 0;
}

