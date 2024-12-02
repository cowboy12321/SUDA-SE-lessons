#include "BinaryTree.h"
#include <queue>
#include <iostream>
#include <stack>
#include <string>
#include <algorithm>
using namespace std;

void BinaryTree::createTree() {
    root = NULL;
}

bool BinaryTree::isEmpty() {
    return root == NULL;
}

TreeNode* BinaryTree::buildTree(string& preorder, string& inorder) {
    if (preorder.empty() || inorder.empty()) return NULL;
    char rootKey = preorder[0];
    auto rootIndex = inorder.find(rootKey);
    if (rootIndex == string::npos) return NULL;

    TreeNode* newNode = new TreeNode(rootKey);
    string leftIn = inorder.substr(0, rootIndex);
    string rightIn = inorder.substr(rootIndex + 1);
    preorder = preorder.substr(1);

    newNode->left = buildTree(preorder, leftIn);
    newNode->right = buildTree(preorder, rightIn);
    return newNode;
}

void BinaryTree::preOrder(TreeNode* node) {
    if (node) {
        cout << node->val - '0' << " ";
        preOrder(node->left);
        preOrder(node->right);
    }
}

void BinaryTree::inOrder(TreeNode* node) {
    if (node) {
        inOrder(node->left);
        cout << node->val - '0' << " ";
        inOrder(node->right);
    }
}

void BinaryTree::postOrder(TreeNode* node) {
    if (node) {
        postOrder(node->left);
        postOrder(node->right);
        cout << node->val - '0' << " ";
    }
}

void BinaryTree::levelOrder() {
    if (root == NULL) return;
    queue<TreeNode*> q;
    q.push(root);
    while (!q.empty()) {
        TreeNode* node = q.front();
        q.pop();
        cout << node->val - '0' << " ";
        if (node->left) q.push(node->left);
        if (node->right) q.push(node->right);
    }
}

void BinaryTree::preOrder1() {
    if (root == NULL) return;
    stack<TreeNode*> s;
    TreeNode* node = root;
    while (node || !s.empty()) {
        while (node) {
            cout << node->val - '0' << " ";
            s.push(node);
            node = node->left;
        }
        if (!s.empty()) {
            node = s.top();
            s.pop();
            node = node->right;
        }
    }
}

void BinaryTree::inOrder1() {
    if (root == NULL) return;
    stack<TreeNode*> s;
    TreeNode* node = root;
    while (node || !s.empty()) {
        while (node) {
            s.push(node);
            node = node->left;
        }
        if (!s.empty()) {
            node = s.top();
            s.pop();
            cout << node->val - '0' << " ";
            node = node->right;
        }
    }
}

void BinaryTree::postOrder1() {
    if (root == NULL) return;
    stack<TreeNode*> s;
    TreeNode* node = root;
    TreeNode* pre_node = NULL;
    while (node || !s.empty()) {
        while (node) {
            s.push(node);
            node = node->left;
        }
        node = s.top();
        s.pop();
        if (node->right == NULL || node->right == pre_node) {
            cout << node->val - '0' << " ";
            pre_node = node;
            node = NULL;
        } else {
            s.push(node);
            node = node->right;
        }
    }
}

int BinaryTree::countNode(TreeNode* node) {
    if (node == NULL) return 0;
    return 1 + countNode(node->left) + countNode(node->right);
}

int BinaryTree::countLeave(TreeNode* node) {
    if (node == NULL) return 0;
    if (node->left == NULL && node->right == NULL) return 1;
    return countLeave(node->left) + countLeave(node->right);
}

int BinaryTree::height(TreeNode* node) {
    if (node == NULL) return 0;
    int l = height(node->left);
    int r = height(node->right);
    return max(l, r) + 1;
}

void BinaryTree::clear(TreeNode* node) {
    if (node == NULL) return;
    clear(node->left);
    clear(node->right);
    delete node;
}

int BinaryTree::width() {
    if (root == NULL) return 0;
    queue<TreeNode*> q;
    q.push(root);
    int maxWidth = 0;
    while (!q.empty()) {
        int size = q.size();
        maxWidth = max(maxWidth, size);
        for (int i = 0; i < size; i++) {
            TreeNode* node = q.front();
            q.pop();
            if (node->left) q.push(node->left);
            if (node->right) q.push(node->right);
        }
    }
    return maxWidth;
}

void BinaryTree::insertNode(char value) {
    TreeNode* newNode = new TreeNode(value);
    if (!root) {
        root = newNode;
        return;
    }
    queue<TreeNode*> q;
    q.push(root);
    while (!q.empty()) {
        TreeNode* node = q.front();
        q.pop();
        if (!node->left) {
            node->left = newNode;
            return;
        } else q.push(node->left);

        if (!node->right) {
            node->right = newNode;
            return;
        } else q.push(node->right);
    }
}

bool BinaryTree::isMirror(TreeNode* node1, TreeNode* node2) {
    if (node1 == NULL && node2 == NULL) return true;
    if (node1 == NULL || node2 == NULL) return false;
    return (node1->val == node2->val) &&
           isMirror(node1->left, node2->right) &&
           isMirror(node1->right, node2->left);
}

TreeNode* BinaryTree::copyTree(TreeNode* node) {
    if (node == NULL) return NULL;
    TreeNode* newNode = new TreeNode(node->val);
    newNode->left = copyTree(node->left);
    newNode->right = copyTree(node->right);
    return newNode;
}

BinaryTree::BinaryTree(const BinaryTree& other) {
    root = copyTree(other.root);
}

BinaryTree& BinaryTree::operator=(const BinaryTree& other) {
    if (this == &other) return *this;
    clear(root);
    root = copyTree(other.root);
    return *this;
}

BinaryTree::~BinaryTree() {
    clear(root);
}

