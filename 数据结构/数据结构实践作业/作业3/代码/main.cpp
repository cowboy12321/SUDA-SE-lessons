#include <iostream>
#include <stack>
#include <string>
#include <sstream>
#include <cctype>
#include <algorithm>
using namespace std;

int judge(char op) 
{
    if (op == '+' || op == '-') return 1;
    if (op == '*' || op == '/') return 2;
    return 0;
}

double apply(double a, double b, char op) 
{
    switch (op) {
        case '+': return a + b;
        case '-': return a - b;
        case '*': return a * b;
        case '/': return a / b;
    }
    return 0;
}

double func1(string tokens) // 中缀表达式求值
{
    stack<double> values; 
    stack<char> ops;
    
    for (int i = 0; i < tokens.length(); i++) {
        if (tokens[i] == ' ') continue;
        
        if (isdigit(tokens[i]) || tokens[i] == '.') {
            double v = 0;
            bool k = false;
            double l = 1;
            while (i < tokens.length() && (isdigit(tokens[i]) || tokens[i] == '.')) {
                if (tokens[i] == '.') 
				{
                    k = true;
                } 
				else 
				{
                    if (k) 
					{
                        l /= 10;
                        v += (tokens[i] - '0') * l;
                    } 
					else 
					{
                        v = (v * 10) + (tokens[i] - '0');
                    }
                }
                i++;
            }
            values.push(v);
            i--;  
        }
        else if (tokens[i] == '(') 
		{
            ops.push(tokens[i]);
        }
        else if (tokens[i] == ')') 
		{
            while (!ops.empty() && ops.top() != '(') 
			{
                double v2 = values.top(); values.pop();
                double v1 = values.top(); values.pop();
                char op = ops.top(); ops.pop();
                values.push(apply(v1, v2, op));
            }
            ops.pop();
        }
        else 
		{
            while (!ops.empty() && judge(ops.top()) >= judge(tokens[i]))
			 {
                double v2 = values.top(); values.pop();
                double v1 = values.top(); values.pop();
                char op = ops.top(); ops.pop();
                values.push(apply(v1, v2, op));
            }
            ops.push(tokens[i]);
        }
    }

    while (!ops.empty()) 
	{
        double v2 = values.top(); values.pop();
        double v1 = values.top(); values.pop();
        char op = ops.top(); ops.pop();
        values.push(apply(v1, v2, op));
    }
    return values.top();
}

void func2(string input) // 后缀表达式求值
{
    stack<double> s; 
    stringstream ss(input);
    string token;
    
    while (ss >> token) 
	{
        if (token == "#") 
		{
            break;
        } 
		else if (isdigit(token[0]) || token[0] == '.') 
		{
            s.push(stod(token)); 
        } 
		else 
		{
            double op2 = s.top(); s.pop();
            double op1 = s.top(); s.pop();
            double result = apply(op1, op2, token[0]);
            s.push(result);
        }
    }
    cout << "后缀表达式的结果: " << s.top() << endl;
}

void prefix(string input) {
    stack<double> s;
    stringstream ss(input);
    string token;
    vector<string> tokens;
    
    while (ss >> token) {
        tokens.push_back(token);  
    }

    reverse(tokens.begin(), tokens.end());

    for (const string& t : tokens) {
        if (isdigit(t[0]) || (t[0] == '-' && t.size() > 1)) {  
            s.push(stod(t));  
        } else {  
            double op1 = s.top(); s.pop();
            double op2 = s.top(); s.pop();
            double result = apply(op1, op2, t[0]);  
            s.push(result);  
        }
    }

    cout << "前缀表达式的结果: " << s.top() << endl;
}
int main()
{
    int n1;
    cout << "请选择想要实现的表达式：" << endl;
    cout << "1.中缀表达式  2.后缀表达式  3.前缀表达式" << endl;
    cin >> n1;

    if (n1 == 1) 
	{
        string input;
        cout << "请输入中缀表达式（空格分隔，结束符 #）: ";
        cin.ignore();  
        getline(cin, input);  
        input.pop_back();  
        double result = func1(input); 
        cout << "中缀表达式的结果是: " << result << endl;
    } 
	else if (n1 == 2) 
	{
        string input;
        cout << "请输入后缀表达式（空格分隔，结束符 #）: ";
        cin.ignore();
        getline(cin, input);
        func2(input);
    } 
    else if(n1 == 3)
    {
    	string input;
    	cout << "请输入前缀表达式（空格分隔）: ";
    	cin.ignore();
		getline(cin,input);
		prefix(input); 
	}
	else  
	{
        cout << "无效的选择。" << endl;
    }
    return 0;
}

