#include <iostream>
#include "SingleLink.h"
#include "IterSingleLink.h"
#include "DoubleLink.h"
#include <vector>
#include <string>
#include <sstream>
using namespace std;

bool user_say_yes()
{
	char c;
	cout << "是否继续操作（继续请直接回车，终止则输入Q/q）:";
	cin >> c;
	return (c == 'q' || c == 'Q');
}

int main() 
{
	int n1,n2,n3;
	int a,b,c;
	string input;
	int num1;
	vector<int> arr; 
	cout << "请选择您想实现的链表：" << endl;
	cout << "1.单链表 " << "2.单向循环链表 " << "3.双向链表（非循环）" << endl;
	cin >> n1;
	if(n1 == 1)
	{
		SingleLink A;
		cout << "请选择您的输入类型：" << endl;
		cout << "1.数组 " << "2.一行数据，中间用空格隔开" << endl;
		cin >> n2;
		cin.ignore();
		if(n2 == 1)
		{
			getline(cin, input);  // 读取整行输入
    		input.erase(input.begin());     // 删除开头的 '['
    		input.erase(input.end() - 1);   // 删除结尾的 ']'
	
    		stringstream ss(input);
    		string token;
	
    		while (getline(ss, token, ',')) 
			{
        		int num = 0;
        		for (size_t i = 0; i < token.size(); i++) 
				{
    				char c = token[i];  
    				if (isdigit(c)) 
					{  
        				num = num * 10 + (c - '0'); 
    				}
				}
				arr.push_back(num);
        	}
			
			for(int i = 0;i < arr.size();i++)
			{
				A.insert(arr[i]);
			}
		}
		else
		{
			while(cin >> num1)
			{
				A.insert(num1);
				char ch;
				if(getchar() == '\n'){
					break;
				}
			}
		}
		cout << "输入结果：" << endl;
		A.print();
		cout << "请输入您的操作(第一个数字为操作，第二/三个数字为值,-1终止)：" << endl;
		cout << "1.插入 2.删除 3.查找值（输入位置） 4.查找位置（输入值）" << endl;
		do{
			cin >> n2;
			if(n2 == 1)
			{
				A.add();
			}
			if(n2 == 2)
			{
				A.remove();
			}
			if(n2 == 3)
			{
				A.get();
			}
			if(n2 == 4)
			{
				A.search();
			}
		}while(not user_say_yes());
		cout << "最终结果：" << endl;
		A.print();
		A.destroy();
	}
	else if(n1 == 2)
	{
		IterSingleLink A;
		cout << "请选择您的输入类型：" << endl;
		cout << "1.数组 " << "2.一行数据，中间用空格隔开" << endl;
		cin >> n2;
		cin.ignore();
		if(n2 == 1)
		{
			getline(cin, input);  
    		input.erase(input.begin());     
    		input.erase(input.end() - 1);   
    		stringstream ss(input);
    		string token;
	
    		while (getline(ss, token, ',')) 
			{
        		int num = 0;
        		for (size_t i = 0; i < token.size(); i++) 
				{
    				char c = token[i];  
    				if (isdigit(c)) 
					{  
        				num = num * 10 + (c - '0'); 
    				}
				}
				arr.push_back(num);
        	}
			
			for(int i = 0;i < arr.size();i++)
			{
				A.insert(arr[i]);
			}
		}
		else
		{
			while(cin >> num1)
			{
				A.insert(num1);
				char ch;
				if(getchar() == '\n'){
					break;
				}
			}
		}
		cout << "输入结果：" << endl;
		A.print();
		cout << "请输入您的操作(第一个数字为操作，第二/三个数字为值,-1终止)：" << endl;
		cout << "1.插入 2.删除 3.查找值（输入位置） 4.查找位置（输入值）" << endl;
		do{
			cin >> n2;
			if(n2 == 1)
			{
				A.add();
			}
			if(n2 == 2)
			{
				A.del();
			}
			if(n2 == 3)
			{
				A.get();
			}
			if(n2 == 4)
			{
				A.search();
			}
		}while(not user_say_yes());
		cout << "最终结果：" << endl;
		A.print();
		A.destroy();
	}
	else if(n1 == 3)
	{
		DoubleLink A;
		cout << "请选择您的输入类型：" << endl;
		cout << "1.数组 " << "2.一行数据，中间用空格隔开" << endl;
		cin >> n2;
		cin.ignore();
		if(n2 == 1)
		{
			getline(cin, input);  
    		input.erase(input.begin());    
    		input.erase(input.end() - 1);  
    		stringstream ss(input);
    		string token;
	
    		while (getline(ss, token, ',')) 
			{
        		int num = 0;
        		for (size_t i = 0; i < token.size(); i++) 
				{
    				char c = token[i];  
    				if (isdigit(c)) 
					{  
        				num = num * 10 + (c - '0'); 
    				}
				}
				arr.push_back(num);
        	}
			
			for(int i = 0;i < arr.size();i++)
			{
				A.create(arr[i]);
			}
		}
		else
		{
			while(cin >> num1)
			{
				A.create(num1);
				char ch;
				if(getchar() == '\n'){
					break;
				}
			}
		}
		cout << "输入结果：" << endl;
		A.print();
		cout << "请输入您的操作(第一个数字为操作，第二/三个数字为值,-1终止)：" << endl;
		cout << "1.插入 2.删除 3.查找值（输入位置） 4.查找位置（输入值）" << endl;
		do{
			cin >> n2;
			if(n2 == 1)
			{
				A.add();
			}
			if(n2 == 2)
			{
				A.remove();
			}
			if(n2 == 3)
			{
				A.get();
			}
			if(n2 == 4)
			{
				A.search();
			}
		}while(not user_say_yes());
		cout << "最终结果：" << endl;
		A.print();
		A.destroy();
	}
	return 0;
}
