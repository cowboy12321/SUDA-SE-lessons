#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

struct TreeNode
{
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};

TreeNode* func1(const vector<int>& nums, int left, int right) {
    if (left > right) return NULL;

    int t = nums[left];  
    int cnt;
    int ans = left;  

    for (int i = left; i <= right; i++) {  
        if (nums[i] > t) {
            t = nums[i];
            ans = i;
        }
    }

    TreeNode* root = new TreeNode(t);
    root->left = func1(nums, left, ans - 1);
    root->right = func1(nums, ans + 1, right);

    return root;
}

void func2(TreeNode* root) {
    if (root == NULL) return;
    cout << root->val << " ";
    func2(root->left);
    func2(root->right);
}

int main()
{
    int n;
    cin >> n;
    vector<int> a(n);
    for (int i = 0; i < n; i++) {
        cin >> a[i];
    }
    TreeNode* root = func1(a, 0, n - 1);

    func2(root);

    return 0;
}
