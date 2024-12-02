#include <iostream>
#include <vector>

using namespace std;

vector<int> func(const string& str) {
    vector<int> a;
    for (size_t i = 0; i < str.size(); i++) {
        if (isdigit(str[i])) {
            int num = 0;
            while (i < str.size() && isdigit(str[i])) {
                num = num * 10 + (str[i] - '0');
                i++;
            }
            a.push_back(num);
        }
    }
    return a;
}

int main() {
    string str;
    getline(cin, str);  
    vector<int> a = func(str);
    for (int i = 0; i < a.size(); i++) {
        cout << a[i] << " ";
    }
    cout << endl;
    return 0;
}
