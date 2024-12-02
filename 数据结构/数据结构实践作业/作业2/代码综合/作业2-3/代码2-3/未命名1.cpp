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
        return NULL; // ��ʼ������Ϊ��
    }

    // ����ƣ�����������
    CardNode* AddCard(CardNode* head, int value) {
        CardNode* newNode = new CardNode{value, NULL};
        if (!head || head->value > value) {
            newNode->next = head;
            return newNode;
        }
        CardNode* current = head;
        while (current->next && current->next->value <= value) { // �����ظ��Ʋ���
            current = current->next;
        }
        newNode->next = current->next;
        current->next = newNode;
        return head;
    }

    // ��ӡ�����е�ʣ����
    void PrintRemainingCards(CardNode* head) {
        if (!head) {
            cout << "����" << endl;
            return;
        }
        CardNode* current = head;
        while (current) {
            cout << current->value << " ";
            current = current->next;
        }
        cout << endl;
    }

    // �ͷ������ڴ�
    void FreeList(CardNode* head) {
        while (head) {
            CardNode* temp = head;
            head = head->next;
            delete temp;
        }
    }

    // ͳ�������е�����
    int CountCards(CardNode* head) {
        int count = 0;
        CardNode* current = head;
        while (current) {
            count++;
            current = current->next;
        }
        return count;
    }

    // ɾ��ָ��������ĳ����ֵ
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
                delete temp;  // ɾ����ǰ��
                remaining--;  // ������1��ȷ��ֻɾ��ʣ��ָ����������
            } else {
                prev = current;
                current = current->next;
            }
        }
    }

    // ��ÿ�����а�˳��ɾ��ָ��������ĳ����ֵ
    void RemoveCardFromAllSets(vector<CardNode*>& sets, int value, int count) {
        for (int i = 0; i < sets.size() && count > 0; i++) {
            RemoveCard(sets[i], value, count);
        }
    }

    // ɾ��������
    void RemoveCompleteSet(vector<CardNode*>& spades, vector<CardNode*>& hearts, vector<CardNode*>& clubs, vector<CardNode*>& diamonds) {
        for (int i = 1; i <= 13; i++) {
            int count = 1;  // ֻɾ��1��
            RemoveCardFromAllSets(spades, i, count);  // ɾ������1��13�ĸ�1����
            RemoveCardFromAllSets(hearts, i, count);  // ɾ������1��13�ĸ�1����
            RemoveCardFromAllSets(clubs, i, count);   // ɾ��÷��1��13�ĸ�1����
            RemoveCardFromAllSets(diamonds, i, count);// ɾ������1��13�ĸ�1����
        }
    }
};

int main() {
    ifstream inputFile("poker.txt");
    if (!inputFile) {
        cerr << "�޷����ļ���" << endl;
        return 1;
    }

    string line;
    SingleLink link;

    // ��ȡ�Ƶĸ���
    getline(inputFile, line);
    int n = line[0] - '0'; // ����
    cout << "��" << n << "����" << endl;

    vector<CardNode*> spades(n, NULL);
    vector<CardNode*> hearts(n, NULL);
    vector<CardNode*> clubs(n, NULL);
    vector<CardNode*> diamonds(n, NULL);

    // ��ȡÿ���Ƶ�������
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < 4; j++) {
            getline(inputFile, line);
            istringstream iss(line);
            int count;
            iss >> count; // ��ȡ��ǰ��ɫ��������
            for (int k = 0; k < count; k++) {
                int cardValue;
                iss >> cardValue; // ��ȡ��ֵ
                switch (j) {
                    case 0: spades[i] = link.AddCard(spades[i], cardValue); break; // ����
                    case 1: hearts[i] = link.AddCard(hearts[i], cardValue); break; // ����
                    case 2: clubs[i] = link.AddCard(clubs[i], cardValue); break; // ÷��
                    case 3: diamonds[i] = link.AddCard(diamonds[i], cardValue); break; // ����
                }
            }
        }
    }

    // ͳ��ÿ�ֻ�ɫ��������
    vector<int> colorCounts(4, 0);
    for (int i = 0; i < n; i++) {
        colorCounts[0] += link.CountCards(spades[i]);   // ����
        colorCounts[1] += link.CountCards(hearts[i]);   // ����
        colorCounts[2] += link.CountCards(clubs[i]);    // ÷��
        colorCounts[3] += link.CountCards(diamonds[i]); // ����
    }

    // ������ƴ�յ������˿�������
    int totalCompleteSets = *min_element(colorCounts.begin(), colorCounts.end()) / 13;
    cout << "��ƴ�յ��˿�������Ϊ" << totalCompleteSets << "��" << endl;

    // �Ƴ�ƴ�ճ��������˿�������
    for (int set = 0; set < totalCompleteSets; set++) {
        link.RemoveCompleteSet(spades, hearts, clubs, diamonds);
    }

    // ���ÿ������ʣ�����
    for (int i = 0; i < n; i++) {
        cout << "��" << (i + 1) << "������ʣ��������£�" << endl;
        cout << "���ң�";
        link.PrintRemainingCards(spades[i]);
        cout << "���ң�";
        link.PrintRemainingCards(hearts[i]);
        cout << "÷����";
        link.PrintRemainingCards(clubs[i]);
        cout << "���飺";
        link.PrintRemainingCards(diamonds[i]);
    }

    // �ͷ��ڴ�
    for (int i = 0; i < n; i++) {
        link.FreeList(spades[i]);
        link.FreeList(hearts[i]);
        link.FreeList(clubs[i]);
        link.FreeList(diamonds[i]);
    }

    inputFile.close();
    return 0;
}

