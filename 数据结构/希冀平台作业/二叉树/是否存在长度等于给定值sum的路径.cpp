#include <iostream>
#include <vector>
#include <sstream>
using namespace std;

struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};

TreeNode* createTree(const vector<int>& nums, size_t& index) {
    if (index >= nums.size() || nums[index] == 0) {
        index++;
        return NULL; 
    }

    TreeNode* root = new TreeNode(nums[index]);
    index++;
    root->left = createTree(nums, index);
    root->right = createTree(nums, index);

    return root;
}

bool isSum(TreeNode* root, int sum) {
    if (root == NULL) {
        return false;
    }
    
    if (root->val == sum) {
        return true;
    }
    if(sum - root->val > 0)
    {
        return isSum(root->left, sum - root->val) || isSum(root->right, sum - root->val);
    }
    return false; 
}

int main() {
    vector<int> nums;
    string line;
    getline(cin, line);
    istringstream s(line);
    int n;
    while (s >> n) {
        nums.push_back(n);
    }
    int sum;
    cin >> sum;
    size_t index = 0;
    TreeNode* root = createTree(nums, index);
    if (isSum(root, sum)) {
        cout << "1";
    } else {
        cout << "0";
    }
    return 0;
}
