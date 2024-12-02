#include "list2.h"
#include <iostream>
using namespace std;

void list2::OrderedAdd()
{
    // 顺序插入 
    int n;
    cout << "请输入需要插入的值：";
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
	//按位置插入
	int t,n;
	int a = 1;
	cout << "请输入需要插入的位置和值：";
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
		cout << "更改后不符合递增条件，请重新输入：" << endl;
	}
	else
	{
		data.insert(data.begin() + t - 1,n);
	}
} 

void list2::change() {
	//替换元素 
	int t,n;
	int a = 1;
	cout << "请输入需要改变的位置和值：";
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
		cout << "更改后不符合递增条件，请重新输入：" << endl;
	}
	else
	{
		data[t - 1] = n;
	}
}

void list2::del1(){
	//按值删除
	int n;
	cout << "请输入需要删除的值：";
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
	//按位置删除
	int t; 
	cout << "请输入需要删除的位置：";
	cin >> t; 
	data.erase(data.begin() + t - 1);
}

void list2::print(){
	//输出 
	for(int i = 0;i < data.size();i++)
	{
		cout << data[i] << " ";
	}
	cout << endl;
}
