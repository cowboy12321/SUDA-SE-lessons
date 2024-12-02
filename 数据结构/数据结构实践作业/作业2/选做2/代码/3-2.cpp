#include <iostream>
#include <string>
#include <algorithm>
using namespace std;

string addBigNumbers(string num1, string num2) {
    if (num1.length() < num2.length()) 
	{
        swap(num1, num2);
    }
    
    string result = "";
    int carry = 0;
    int diff = num1.length() - num2.length();
    
    for (int i = num2.length() - 1; i >= 0; i--) {
        int sum = (num1[i + diff] - '0') + (num2[i] - '0') + carry;
        carry = sum / 10;
        result.push_back(sum % 10 + '0');
    }

    for (int i = num1.length() - num2.length() - 1; i >= 0; i--) {
        int sum = (num1[i] - '0') + carry;
        carry = sum / 10;
        result.push_back(sum % 10 + '0');
    }

    if (carry) {
        result.push_back(carry + '0');
    }

    reverse(result.begin(), result.end()); 
    return result;
}

int main() {
    string num1;
    string num2;
    cout << "�������һ����������";
    cin >> num1;
    cout << "�������һ����������";
    cin >> num2;
    cout << "��: " << addBigNumbers(num1, num2) << endl;
    return 0;
}

