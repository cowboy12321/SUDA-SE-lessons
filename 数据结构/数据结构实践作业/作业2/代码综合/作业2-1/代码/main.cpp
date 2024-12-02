#include <iostream>
#include "list1.h"
#include "list2.h"
#include <vector>
using namespace std;

bool user_say_yes()
{
	char c;
	cout << "是否终止程序，如若终止请按Q/q：" << flush;
	cin >> c;
	return (c == 'q' || c == 'Q');
}


int main() {
    int n1,n2;
    cout << "请输入您想要实现的顺序表：" << endl;
	cout << "1.无序表 2.有序表" << endl;
	cin >> n1;
	if(n1 == 1)
	{
		list1 A;
		cout << "请选择想要实现的功能：" << endl;
		cout << "1.直接插入 2.按位置插入 3.替换 4.按值删除 5.按位置删除" << endl;
		do{
			cin >> n2;
			if(n2 == 1)
			{
				A.add1();
			}
			if(n2 == 2)
			{
				A.add2();
			}
			if(n2 == 3)
			{
				A.change();
			}
			if(n2 == 4)
			{
				A.del1();
			}
			if(n2 == 5)
			{
				A.del2();
			}
			A.print();
		}while(not user_say_yes());
		
	} 
	else
	{
		list2 B;
		cout << "请选择想要实现的功能：" << endl;
		cout << "1.顺序插入 2.按位置插入 3.替换 4.按值删除 5.按位置删除" << endl;
		do{
			cin >> n2;
			if(n2 == 1)
			{
				B.OrderedAdd();
			}
			if(n2 == 2)
			{
				B.LocatedAdd();
			}
			if(n2 == 3)
			{
				B.change();
			}
			if(n2 == 4)
			{
				B.del1();
			}
			if(n2 == 5)
			{
				B.del2();
			}
			B.print();
		}while(not user_say_yes());
	}
	return 0;
}
