#ifndef LIST1_H
#define LIST1_H
#include <iostream>
#include <vector>
using namespace std;
class list1 //����˳���� 
{
	public:
		void add1();//ֱ�Ӳ��� 
		void add2();//��λ�ò��� 
		void change();//�滻 
		void del1();//��ֵɾ��
		void del2();//��λ��ɾ�� 
		void print();
	private:
		vector<int> data;
};

#endif
