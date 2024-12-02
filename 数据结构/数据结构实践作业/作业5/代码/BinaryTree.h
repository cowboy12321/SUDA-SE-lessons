#ifndef BINARYTREE_H
#define BINARYTREE_H

#include <string>
using namespace std;

struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode(char x) : val(x), left(NULL), right(NULL) {}
};

class BinaryTree {
public:
    BinaryTree() : root(NULL) {}  // 构造函数初始化 root
    ~BinaryTree();                // 析构函数

    void createTree();
    TreeNode* buildTree(string&, string&);
    bool isEmpty();

    void preOrder(TreeNode* node);
    void inOrder(TreeNode* node);
    void postOrder(TreeNode* node);
    void levelOrder();

    void preOrder1();
    void inOrder1();
    void postOrder1();

    int countNode(TreeNode* node);
    int countLeave(TreeNode* node);
    int height(TreeNode* node);
    void clear(TreeNode* node);
    int width();

    void insertNode(char value);
    bool isMirror(TreeNode* node1, TreeNode* node2);

    BinaryTree(const BinaryTree& other);        // 拷贝构造函数
    BinaryTree& operator=(const BinaryTree& other);  // 赋值运算符重载

    TreeNode* root;

private:
    TreeNode* copyTree(TreeNode* node);  // 辅助函数声明
};

#endif

