#ifndef LIST2_H
#define LIST2_H
#include <iostream>
#include <vector>
#include "list1.h"
using namespace std;
class list2 : public list1 //����˳���� 
{
	public:
		void OrderedAdd(); //˳����� 
		void LocatedAdd();//��λ�ò��� 
		void change();//�滻Ԫ�� 
		void del1();//��ֵɾ��
		void del2();//��λ��ɾ��
		void print();
	private:
		vector<int> data;
};

#endif
