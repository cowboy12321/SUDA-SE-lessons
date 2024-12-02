#include "BinaryTree.h"
#include <iostream>
using namespace std;

void printMenu() {
    cout << "\n=== 二叉树操作菜单 ===\n";
    cout << "1. 创建空二叉树\n";
    cout << "2. 根据前序和中序序列建树\n";
    cout << "3. 判断是否为空树\n";
    cout << "4. 先序遍历\n";
    cout << "5. 中序遍历\n";
    cout << "6. 后序遍历\n";
    cout << "7. 层次遍历\n";
    cout << "8. 插入结点\n";
    cout << "9. 清空二叉树\n";
    cout << "10. 计算结点总数\n";
    cout << "11. 计算叶子结点数\n";
    cout << "12. 计算树的高度\n";
    cout << "13. 计算树的宽度\n";
    cout << "14. 判断是否为镜像树\n";
    cout << "0. 退出程序\n";
}

int main() {
    BinaryTree tree;
    BinaryTree otherTree; // 用于镜像判断
    int choice;
    string preOrder, inOrder;
    char value;
    bool isTreeCreated = false; // 标志变量

    printMenu();
    do {
    	cout << "请选择操作：";
        cin >> choice;

        if (choice != 1 && !isTreeCreated) {
            cout << "请先创建空二叉树（选择1）。\n";
            continue;
        }

        switch (choice) {
            case 1:
                tree.createTree();
                isTreeCreated = true;
                cout << "空二叉树已创建。\n";
                break;

            case 2:
                cout << "输入前序序列：";
                cin >> preOrder;
                cout << "输入中序序列：";
                cin >> inOrder;
                tree.root = tree.buildTree(preOrder, inOrder);
                cout << "二叉树已创建。\n";
                break;

            case 3:
                cout << (tree.isEmpty() ? "二叉树为空\n" : "二叉树不为空\n");
                break;

            case 4:
                cout << "先序遍历：";
                tree.preOrder(tree.root);
                cout << "\n";
                break;

            case 5:
                cout << "中序遍历：";
                tree.inOrder(tree.root);
                cout << "\n";
                break;

            case 6:
                cout << "后序遍历：";
                tree.postOrder(tree.root);
                cout << "\n";
                break;

            case 7:
                cout << "层次遍历：";
                tree.levelOrder();
                cout << "\n";
                break;

            case 8:
                cout << "输入要插入的结点值：";
                cin >> value;
                tree.insertNode(value);
                cout << "结点已插入。\n";
                break;

            case 9:
                tree.clear(tree.root);
                tree.root = NULL; // 确保清空后根节点为 nullptr
                cout << "二叉树已清空。\n";
                break;

            case 10:
                cout << "结点总数: " << tree.countNode(tree.root) << "\n";
                break;

            case 11:
                cout << "叶子结点数: " << tree.countLeave(tree.root) << "\n";
                break;

            case 12:
                cout << "二叉树高度: " << tree.height(tree.root) << "\n";
                break;

            case 13:
                cout << "二叉树宽度: " << tree.width() << "\n";
                break;

            case 14:
                cout << "输入用于镜像判断的树的前序序列：";
                cin >> preOrder;
                cout << "输入用于镜像判断的树的中序序列：";
                cin >> inOrder;
                otherTree.root = otherTree.buildTree(preOrder, inOrder);
                cout << (tree.isMirror(tree.root, otherTree.root) ? "两树是镜像。\n" : "两树不是镜像。\n");
                break;

            case 0:
                cout << "程序退出。\n";
                break;

            default:
                cout << "无效输入，请重新选择。\n";
        }
    } while (choice != 0);

    return 0;
}

