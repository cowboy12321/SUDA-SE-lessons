#include <iostream>
#include <vector>
#include <queue>
#include <unordered_map>
#include <climits>

using namespace std;

class SocialNetwork {
public:
    map<string, int> name_to_id;
    vector<string> id_to_name;  // 反向映射
    vector<vector<int>> adj;    // 存储与id为i的人相认识的人的编号
    
    int num; // 社交网络中的人数

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
        adj[id2].push_back(id1);  // 无向图
    }

    // 找到两个人之间的最短路径
    vector<string> findShortestPath(const string& start, const string& end) {
        int start_id = name_to_id[start];
        int end_id = name_to_id[end];
        
        // 如果两个人相同
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
            
            // 遍历所有相邻的节点
            for (int neighbor : adj[current]) {
                if (dist[neighbor] == INT_MAX) {  // 如果还没被访问过
                    dist[neighbor] = dist[current] + 1;
                    prev[neighbor] = current;
                    q.push(neighbor);
                    
                    // 一旦找到目标，停止搜索
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
        
        // 如果没有找到路径
        return {};
    }
    
    // 打印所有人的信息
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
    
    // 添加人
    sn.addPerson("Alice");
    sn.addPerson("Bob");
    sn.addPerson("Charlie");
    sn.addPerson("David");
    sn.addPerson("Eve");
    
    // 添加关系
    sn.addConnection("Alice", "Bob");
    sn.addConnection("Bob", "Charlie");
    sn.addConnection("Charlie", "David");
    sn.addConnection("David", "Eve");
    
    // 打印社交网络
    cout << "社交网络图：" << endl;
    sn.printNetwork();
    
    // 查找两个人之间的最短路径
    cout << "查询 Alice 和 Eve 之间的最短路径：" << endl;
    vector<string> path = sn.findShortestPath("Alice", "Eve");
    
    if (path.empty()) {
        cout << "没有找到路径。" << endl;
    } else {
        cout << "最短路径是：";
        for (const string& name : path) {
            cout << name << " ";
        }
        cout << endl;
    }

    return 0;
}

