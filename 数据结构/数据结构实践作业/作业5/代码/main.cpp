#include "BinaryTree.h"
#include <iostream>
using namespace std;

void printMenu() {
    cout << "\n=== �����������˵� ===\n";
    cout << "1. �����ն�����\n";
    cout << "2. ����ǰ����������н���\n";
    cout << "3. �ж��Ƿ�Ϊ����\n";
    cout << "4. �������\n";
    cout << "5. �������\n";
    cout << "6. �������\n";
    cout << "7. ��α���\n";
    cout << "8. ������\n";
    cout << "9. ��ն�����\n";
    cout << "10. ����������\n";
    cout << "11. ����Ҷ�ӽ����\n";
    cout << "12. �������ĸ߶�\n";
    cout << "13. �������Ŀ��\n";
    cout << "14. �ж��Ƿ�Ϊ������\n";
    cout << "0. �˳�����\n";
}

int main() {
    BinaryTree tree;
    BinaryTree otherTree; // ���ھ����ж�
    int choice;
    string preOrder, inOrder;
    char value;
    bool isTreeCreated = false; // ��־����

    printMenu();
    do {
    	cout << "��ѡ�������";
        cin >> choice;

        if (choice != 1 && !isTreeCreated) {
            cout << "���ȴ����ն�������ѡ��1����\n";
            continue;
        }

        switch (choice) {
            case 1:
                tree.createTree();
                isTreeCreated = true;
                cout << "�ն������Ѵ�����\n";
                break;

            case 2:
                cout << "����ǰ�����У�";
                cin >> preOrder;
                cout << "�����������У�";
                cin >> inOrder;
                tree.root = tree.buildTree(preOrder, inOrder);
                cout << "�������Ѵ�����\n";
                break;

            case 3:
                cout << (tree.isEmpty() ? "������Ϊ��\n" : "��������Ϊ��\n");
                break;

            case 4:
                cout << "���������";
                tree.preOrder(tree.root);
                cout << "\n";
                break;

            case 5:
                cout << "���������";
                tree.inOrder(tree.root);
                cout << "\n";
                break;

            case 6:
                cout << "���������";
                tree.postOrder(tree.root);
                cout << "\n";
                break;

            case 7:
                cout << "��α�����";
                tree.levelOrder();
                cout << "\n";
                break;

            case 8:
                cout << "����Ҫ����Ľ��ֵ��";
                cin >> value;
                tree.insertNode(value);
                cout << "����Ѳ��롣\n";
                break;

            case 9:
                tree.clear(tree.root);
                tree.root = NULL; // ȷ����պ���ڵ�Ϊ nullptr
                cout << "����������ա�\n";
                break;

            case 10:
                cout << "�������: " << tree.countNode(tree.root) << "\n";
                break;

            case 11:
                cout << "Ҷ�ӽ����: " << tree.countLeave(tree.root) << "\n";
                break;

            case 12:
                cout << "�������߶�: " << tree.height(tree.root) << "\n";
                break;

            case 13:
                cout << "���������: " << tree.width() << "\n";
                break;

            case 14:
                cout << "�������ھ����жϵ�����ǰ�����У�";
                cin >> preOrder;
                cout << "�������ھ����жϵ������������У�";
                cin >> inOrder;
                otherTree.root = otherTree.buildTree(preOrder, inOrder);
                cout << (tree.isMirror(tree.root, otherTree.root) ? "�����Ǿ���\n" : "�������Ǿ���\n");
                break;

            case 0:
                cout << "�����˳���\n";
                break;

            default:
                cout << "��Ч���룬������ѡ��\n";
        }
    } while (choice != 0);

    return 0;
}

