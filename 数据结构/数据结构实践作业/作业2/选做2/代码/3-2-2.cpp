#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
using namespace std;

string multiplyBigNumbers(string num1, string num2) {
    if (num1 == "0" || num2 == "0") return "0"; 
    
    int len1 = num1.length();
    int len2 = num2.length();
    vector<int> result(len1 + len2, 0); 

    for (int i = len1 - 1; i >= 0; i--) {
        for (int j = len2 - 1; j >= 0; j--) {
            int mul = (num1[i] - '0') * (num2[j] - '0');
            int sum = mul + result[i + j + 1];
            result[i + j + 1] = sum % 10;
            result[i + j] += sum / 10;
        }
    }

    string res = "";
    for (int i = 0; i < result.size(); i++) {
        if (!(res.empty() && result[i] == 0)) { 
            res.push_back(result[i] + '0');
        }
    }
    
    return res.empty() ? "0" : res; 
}

int main() {
    string num1;
    string num2; 
    cout << "请输入第一个大整数：";
    cin >> num1;
    cout << "请输入第一个大整数：";
    cin >> num2;
    cout << "积: " << multiplyBigNumbers(num1, num2) << endl;
    return 0;
}

