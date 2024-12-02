#include "list1.h"
#include <iostream>
#include <vector>
using namespace std;

void list1::add1()
{	//直接插入 
	int n;
	cout << "请输入需要插入的值：";
	cin >> n;
	data.push_back(n);
}

void list1::add2() {
	// 按位置插入
	int t,n;
	cout << "请输入需要插入的位置和值：";
	cin >> t >> n; 
    if (t >= 0 && t <= data.size()) {
        data.insert(data.begin() + t,n); 
    }
    else{
    	cout << "插入失败: 索引超出范围！" << endl;
	}
}

void list1::change() {
	//替换元素 
	int a,b;
	cout << "请输入需要改变的位置和值：";
	cin >> a >> b;
    data[a - 1] = b;
}
void list1::del1(){
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

void list1::del2(){
	//按位置删除
	int t; 
	cout << "请输入需要删除的位置：";
	cin >> t; 
	data.erase(data.begin() + t);
}
void list1::print(){
	//输出 
	for(int i = 0;i < data.size();i++)
	{
		cout << data[i] << " ";
	}
	cout << endl;
}

