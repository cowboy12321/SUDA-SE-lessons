#include "list2.h"
#include <iostream>
using namespace std;

void list2::OrderedAdd()
{
    // ˳����� 
    int n;
    cout << "��������Ҫ�����ֵ��";
    cin >> n;
    if (data.empty())  
    {
        data.push_back(n);  
    }
    else
    {
        bool inserted = false;  
        for (size_t i = 0; i < data.size(); i++)
        {
            if (data[i] >= n)
            {
                data.insert(data.begin() + i, n);  
                inserted = true;  
                break;
            }
        }
        if (!inserted)  
        {
            data.push_back(n); 
        }
    }
}


void list2::LocatedAdd()
{
	//��λ�ò���
	int t,n;
	int a = 1;
	cout << "��������Ҫ�����λ�ú�ֵ��";
	cin >> t >> n;
	if(t == 1 && data[0] < n)
	{
		a = 0;
	}
	if(t > 1 && t < data.size())
	{
		if(data[t - 2] > n || data[t] < n)
		{
			a = 0;
		}
	}
	if(t == data.size() && data[t - 1] > n)
	{
		a = 0;
	}
	if(a == 0)
	{
		cout << "���ĺ󲻷��ϵ������������������룺" << endl;
	}
	else
	{
		data.insert(data.begin() + t - 1,n);
	}
} 

void list2::change() {
	//�滻Ԫ�� 
	int t,n;
	int a = 1;
	cout << "��������Ҫ�ı��λ�ú�ֵ��";
	cin >> t >> n;
    if(t == 1 && data[0] < n)
	{
		a = 0;
	}
	if(t > 1 && t < data.size())
	{
		if(data[t - 2] > n || data[t] < n)
		{
			a = 0;
		}
	}
	if(t == data.size() && data[t - 1] > n)
	{
		a = 0;
	}
	if(a == 0)
	{
		cout << "���ĺ󲻷��ϵ������������������룺" << endl;
	}
	else
	{
		data[t - 1] = n;
	}
}

void list2::del1(){
	//��ֵɾ��
	int n;
	cout << "��������Ҫɾ����ֵ��";
	cin >> n;
	for(int i = 0;i < data.size();i++)
	{
		if(data[i] == n)
		{
			data.erase(data.begin() + i);
		}
	}
}

void list2::del2(){
	//��λ��ɾ��
	int t; 
	cout << "��������Ҫɾ����λ�ã�";
	cin >> t; 
	data.erase(data.begin() + t - 1);
}

void list2::print(){
	//��� 
	for(int i = 0;i < data.size();i++)
	{
		cout << data[i] << " ";
	}
	cout << endl;
}
