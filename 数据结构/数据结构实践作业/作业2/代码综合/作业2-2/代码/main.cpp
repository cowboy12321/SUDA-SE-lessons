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
	cout << "�Ƿ����������������ֱ�ӻس�����ֹ������Q/q��:";
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
	cout << "��ѡ������ʵ�ֵ�����" << endl;
	cout << "1.������ " << "2.����ѭ������ " << "3.˫��������ѭ����" << endl;
	cin >> n1;
	if(n1 == 1)
	{
		SingleLink A;
		cout << "��ѡ�������������ͣ�" << endl;
		cout << "1.���� " << "2.һ�����ݣ��м��ÿո����" << endl;
		cin >> n2;
		cin.ignore();
		if(n2 == 1)
		{
			getline(cin, input);  // ��ȡ��������
    		input.erase(input.begin());     // ɾ����ͷ�� '['
    		input.erase(input.end() - 1);   // ɾ����β�� ']'
	
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
		cout << "��������" << endl;
		A.print();
		cout << "���������Ĳ���(��һ������Ϊ�������ڶ�/��������Ϊֵ,-1��ֹ)��" << endl;
		cout << "1.���� 2.ɾ�� 3.����ֵ������λ�ã� 4.����λ�ã�����ֵ��" << endl;
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
		cout << "���ս����" << endl;
		A.print();
		A.destroy();
	}
	else if(n1 == 2)
	{
		IterSingleLink A;
		cout << "��ѡ�������������ͣ�" << endl;
		cout << "1.���� " << "2.һ�����ݣ��м��ÿո����" << endl;
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
		cout << "��������" << endl;
		A.print();
		cout << "���������Ĳ���(��һ������Ϊ�������ڶ�/��������Ϊֵ,-1��ֹ)��" << endl;
		cout << "1.���� 2.ɾ�� 3.����ֵ������λ�ã� 4.����λ�ã�����ֵ��" << endl;
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
		cout << "���ս����" << endl;
		A.print();
		A.destroy();
	}
	else if(n1 == 3)
	{
		DoubleLink A;
		cout << "��ѡ�������������ͣ�" << endl;
		cout << "1.���� " << "2.һ�����ݣ��м��ÿո����" << endl;
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
		cout << "��������" << endl;
		A.print();
		cout << "���������Ĳ���(��һ������Ϊ�������ڶ�/��������Ϊֵ,-1��ֹ)��" << endl;
		cout << "1.���� 2.ɾ�� 3.����ֵ������λ�ã� 4.����λ�ã�����ֵ��" << endl;
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
		cout << "���ս����" << endl;
		A.print();
		A.destroy();
	}
	return 0;
}
