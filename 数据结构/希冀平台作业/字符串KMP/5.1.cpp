#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <cctype>
#include <vector>
#include <algorithm>

using namespace std;

// 将字符转换为小写
char func1(char ch) {
    return static_cast<char>(tolower(static_cast<unsigned char>(ch)));
}

// 判断字符是否在给定的字符集合中（可选择是否排除）
bool func2(char ch, const string& a, bool b) {
    ch = func1(ch);
    if (b) {
        return a.find(ch) == string::npos;
    }
    return a.find(ch)!= string::npos;
}

// 表示模式中的一个token
struct Token {
    enum Type { CHAR, SET, STAR } type;
    char ch;
    string setChs;
    bool excld;
};

// 解析模式字符串为token数组
int func3(const string& a, vector<Token>& t) {
    int c = 0;
    int d = 0;
    int e = 0;
    int f = 0;
    while (c < a.size()) {
        if (a[c] == '*') {
            if (e >= 1) {
                return -1;
            }
            t.push_back({ Token::STAR });
            c++;
            e++;
        } else if (a[c] == '[') {
            if (f >= 1) {
                return -1;
            }
            t.push_back({ Token::SET, '\0', "", false });
            c++;
            if (a[c] == '^') {
                t.back().excld = true;
                c++;
            }
            while (c < a.size() && a[c]!= ']') {
                t.back().setChs += a[c++];
            }
            if (c < a.size() && a[c] == ']') {
                c++;
            } else {
                return -1;
            }
            if (t.back().setChs.empty() || (t.back().excld && t.back().setChs.size() == 1 && t.back().setChs[0] == '^')) {
                return -1;
            }
            d++;
            f++;
        } else {
            t.push_back({ Token::CHAR, a[c], "", false });
            c++;
            d++;
        }
    }
    return d;
}

// 在文本中从指定位置开始匹配模式
int func4(const string& a, int b, int c, const vector<Token>& t, int d, int e) {
    if (e == d) {
        return 0;
    }
    if (c > b) {
        return -1;
    }
    if (t[e].type == Token::STAR) {
        int g = -1;
        for (int i = c; i <= b; i++) {
            int h = func4(a, b, i, t, d, e + 1);
            if (h!= -1) {
                int i2 = (i - c) + h;
                if (g == -1 || i2 < g) {
                    g = i2;
                }
            }
        }
        return g;
    } else if (t[e].type == Token::CHAR) {
        if (c >= b) {
            return -1;
        }
        if (func1(a[c]) == func1(t[e].ch)) {
            int h = func4(a, b, c + 1, t, d, e + 1);
            if (h!= -1) {
                return 1 + h;
            }
        }
        return -1;
    } else if (t[e].type == Token::SET) {
        if (c >= b) {
            return -1;
        }
        if (func2(a[c], t[e].setChs, t[e].excld)) {
            int h = func4(a, b, c + 1, t, d, e + 1);
            if (h!= -1) {
                return 1 + h;
            }
        }
        return -1;
    }
    return -1;
}

// 在一行文本中查找所有匹配模式的子串
void func5(const string& a, const vector<Token>& t, int d, vector<string>& ans) {
    int b = a.size();
    ans.clear();

    for (int c = 0; c < b; c++) {
        int e = func4(a, b, c, t, d, 0);
        if (e!= -1) {
            ans.push_back(a.substr(c, e));
        }
    }
}

int main() {
    ifstream inFile("string.in");
    ofstream outFile("string.out");

    string a;
    getline(cin, a);

    vector<Token> t;
    int d = func3(a, t);

    int f = 0;
    string a2;
    while (getline(inFile, a2)) {
        f++;
        vector<string> ans;
        func5(a2, t, d, ans);

        if (ans.size() > 0) {
            outFile << f << ":";
            for (int i = 0; i < ans.size(); i++) {
                if (i > 0) {
                    outFile << ",";
                }
                outFile << ans[i];
            }
            outFile << "\n";
        }
    }

    inFile.close();
    outFile.close();

    return 0;
}
