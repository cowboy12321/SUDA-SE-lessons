#include <iostream>
#include <string>

using namespace std;

void func(const string& tree, size_t t) {
    if (t >= tree.length() || tree[t] == ' ') {
        return;
    }
    cout << tree[t];
    func(tree, 2 * t + 1);
    func(tree, 2 * t + 2);
}

int main() {
    string tree;
    getline(cin, tree);
    func(tree, 0);
    return 0;
}
