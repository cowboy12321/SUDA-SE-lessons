#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include <climits>
using namespace std;

// 二叉树节点结构体
struct TreeNode
{
    char val;
    TreeNode *left;
    TreeNode *right;
    TreeNode(char x) : val(x), left(NULL), right(NULL) {}
};

// 创建二叉树
TreeNode *createTree(vector<string> &nums, size_t &t)
{
    if (t >= nums.size() || nums[t] == "#")
    {
        t++;
        return NULL;
    }
    // 取字符串的第一个字符作为节点值
    char value = nums[t][0];
    TreeNode *root = new TreeNode(value);
    t++;
    root->left = createTree(nums, t);
    root->right = createTree(nums, t);
    return root;
}

// 替换节点值并返回子树最大值
char replace(TreeNode *root)
{
    if (root == NULL)
        return '\0';

    char l = replace(root->left);
    char r = replace(root->right);

    // 比较字符的ASCII码值来确定较大值
    char m = l > r? l : r;

    if (m > root->val)
        root->val = m;

    return root->val > m? root->val : m;
}

// 输出新树的前序遍历
void preOrder(TreeNode *root, vector<char> &ans)
{
    if (root == NULL)
        return;
    ans.push_back(root->val);
    preOrder(root->left, ans);
    preOrder(root->right, ans);
}

int main()
{
    vector<string> nums;
    string line;
    getline(cin, line);
    istringstream s(line);
    string n;
    while (s >> n)
    {
        nums.push_back(n);
    }

    size_t t = 0;
    TreeNode *root = createTree(nums, t);

    replace(root);

    vector<char> ans;
    preOrder(root, ans);
    for (size_t i = 0; i < ans.size(); i++)
    {
        cout << ans[i];
        if (i!= ans.size() - 1)
            cout << " ";
    }

    return 0;
}
