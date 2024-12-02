#ifndef LIST1_H
#define LIST1_H
#include <iostream>
#include <vector>
using namespace std;
class list1 //无序顺序类 
{
	public:
		void add1();//直接插入 
		void add2();//按位置插入 
		void change();//替换 
		void del1();//按值删除
		void del2();//按位置删除 
		void print();
	private:
		vector<int> data;
};

#endif
