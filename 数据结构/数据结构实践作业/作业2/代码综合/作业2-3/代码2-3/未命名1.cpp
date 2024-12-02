#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <algorithm>

using namespace std;

struct CardNode {
    int value;
    CardNode* next;
};

class SingleLink {
public:

    CardNode* Init() {
        return NULL; // 初始化链表为空
    }

    // 添加牌，按升序排列
    CardNode* AddCard(CardNode* head, int value) {
        CardNode* newNode = new CardNode{value, NULL};
        if (!head || head->value > value) {
            newNode->next = head;
            return newNode;
        }
        CardNode* current = head;
        while (current->next && current->next->value <= value) { // 允许重复牌插入
            current = current->next;
        }
        newNode->next = current->next;
        current->next = newNode;
        return head;
    }

    // 打印链表中的剩余牌
    void PrintRemainingCards(CardNode* head) {
        if (!head) {
            cout << "无牌" << endl;
            return;
        }
        CardNode* current = head;
        while (current) {
            cout << current->value << " ";
            current = current->next;
        }
        cout << endl;
    }

    // 释放链表内存
    void FreeList(CardNode* head) {
        while (head) {
            CardNode* temp = head;
            head = head->next;
            delete temp;
        }
    }

    // 统计链表中的牌数
    int CountCards(CardNode* head) {
        int count = 0;
        CardNode* current = head;
        while (current) {
            count++;
            current = current->next;
        }
        return count;
    }

    // 删除指定数量的某个牌值
    void RemoveCard(CardNode*& head, int value, int& remaining) {
        CardNode* current = head;
        CardNode* prev = NULL;
        while (current && remaining > 0) {
            if (current->value == value) {
                if (prev) {
                    prev->next = current->next;
                } else {
                    head = current->next;
                }
                CardNode* temp = current;
                current = current->next;
                delete temp;  // 删除当前牌
                remaining--;  // 计数减1，确保只删除剩余指定数量的牌
            } else {
                prev = current;
                current = current->next;
            }
        }
    }

    // 从每副牌中按顺序删除指定数量的某个牌值
    void RemoveCardFromAllSets(vector<CardNode*>& sets, int value, int count) {
        for (int i = 0; i < sets.size() && count > 0; i++) {
            RemoveCard(sets[i], value, count);
        }
    }

    // 删除整副牌
    void RemoveCompleteSet(vector<CardNode*>& spades, vector<CardNode*>& hearts, vector<CardNode*>& clubs, vector<CardNode*>& diamonds) {
        for (int i = 1; i <= 13; i++) {
            int count = 1;  // 只删除1张
            RemoveCardFromAllSets(spades, i, count);  // 删除黑桃1到13的各1张牌
            RemoveCardFromAllSets(hearts, i, count);  // 删除红桃1到13的各1张牌
            RemoveCardFromAllSets(clubs, i, count);   // 删除梅花1到13的各1张牌
            RemoveCardFromAllSets(diamonds, i, count);// 删除方块1到13的各1张牌
        }
    }
};

int main() {
    ifstream inputFile("poker.txt");
    if (!inputFile) {
        cerr << "无法打开文件！" << endl;
        return 1;
    }

    string line;
    SingleLink link;

    // 读取牌的副数
    getline(inputFile, line);
    int n = line[0] - '0'; // 副数
    cout << "有" << n << "副牌" << endl;

    vector<CardNode*> spades(n, NULL);
    vector<CardNode*> hearts(n, NULL);
    vector<CardNode*> clubs(n, NULL);
    vector<CardNode*> diamonds(n, NULL);

    // 读取每副牌的牌数据
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < 4; j++) {
            getline(inputFile, line);
            istringstream iss(line);
            int count;
            iss >> count; // 读取当前花色的牌数量
            for (int k = 0; k < count; k++) {
                int cardValue;
                iss >> cardValue; // 读取牌值
                switch (j) {
                    case 0: spades[i] = link.AddCard(spades[i], cardValue); break; // 黑桃
                    case 1: hearts[i] = link.AddCard(hearts[i], cardValue); break; // 红桃
                    case 2: clubs[i] = link.AddCard(clubs[i], cardValue); break; // 梅花
                    case 3: diamonds[i] = link.AddCard(diamonds[i], cardValue); break; // 方块
                }
            }
        }
    }

    // 统计每种花色的总牌数
    vector<int> colorCounts(4, 0);
    for (int i = 0; i < n; i++) {
        colorCounts[0] += link.CountCards(spades[i]);   // 黑桃
        colorCounts[1] += link.CountCards(hearts[i]);   // 红桃
        colorCounts[2] += link.CountCards(clubs[i]);    // 梅花
        colorCounts[3] += link.CountCards(diamonds[i]); // 方块
    }

    // 计算能拼凑的完整扑克牌套数
    int totalCompleteSets = *min_element(colorCounts.begin(), colorCounts.end()) / 13;
    cout << "能拼凑的扑克牌套数为" << totalCompleteSets << "套" << endl;

    // 移除拼凑出的完整扑克牌套数
    for (int set = 0; set < totalCompleteSets; set++) {
        link.RemoveCompleteSet(spades, hearts, clubs, diamonds);
    }

    // 输出每副牌中剩余的牌
    for (int i = 0; i < n; i++) {
        cout << "第" << (i + 1) << "副牌中剩余的牌如下：" << endl;
        cout << "黑桃：";
        link.PrintRemainingCards(spades[i]);
        cout << "红桃：";
        link.PrintRemainingCards(hearts[i]);
        cout << "梅花：";
        link.PrintRemainingCards(clubs[i]);
        cout << "方块：";
        link.PrintRemainingCards(diamonds[i]);
    }

    // 释放内存
    for (int i = 0; i < n; i++) {
        link.FreeList(spades[i]);
        link.FreeList(hearts[i]);
        link.FreeList(clubs[i]);
        link.FreeList(diamonds[i]);
    }

    inputFile.close();
    return 0;
}

