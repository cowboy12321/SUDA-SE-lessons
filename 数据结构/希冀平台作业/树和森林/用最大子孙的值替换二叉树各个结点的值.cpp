#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include <climits>
using namespace std;

// �������ڵ�ṹ��
struct TreeNode
{
    char val;
    TreeNode *left;
    TreeNode *right;
    TreeNode(char x) : val(x), left(NULL), right(NULL) {}
};

// ����������
TreeNode *createTree(vector<string> &nums, size_t &t)
{
    if (t >= nums.size() || nums[t] == "#")
    {
        t++;
        return NULL;
    }
    // ȡ�ַ����ĵ�һ���ַ���Ϊ�ڵ�ֵ
    char value = nums[t][0];
    TreeNode *root = new TreeNode(value);
    t++;
    root->left = createTree(nums, t);
    root->right = createTree(nums, t);
    return root;
}

// �滻�ڵ�ֵ�������������ֵ
char replace(TreeNode *root)
{
    if (root == NULL)
        return '\0';

    char l = replace(root->left);
    char r = replace(root->right);

    // �Ƚ��ַ���ASCII��ֵ��ȷ���ϴ�ֵ
    char m = l > r? l : r;

    if (m > root->val)
        root->val = m;

    return root->val > m? root->val : m;
}

// ���������ǰ�����
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
