#ifndef LIST2_H
#define LIST2_H
#include <iostream>
#include <vector>
#include "list1.h"
using namespace std;
class list2 : public list1 //有序顺序类 
{
	public:
		void OrderedAdd(); //顺序插入 
		void LocatedAdd();//按位置插入 
		void change();//替换元素 
		void del1();//按值删除
		void del2();//按位置删除
		void print();
	private:
		vector<int> data;
};

#endif
