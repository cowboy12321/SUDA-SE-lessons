#include <iostream>
#include "list1.h"
#include "list2.h"
#include <vector>
using namespace std;

bool user_say_yes()
{
	char c;
	cout << "�Ƿ���ֹ����������ֹ�밴Q/q��" << flush;
	cin >> c;
	return (c == 'q' || c == 'Q');
}


int main() {
    int n1,n2;
    cout << "����������Ҫʵ�ֵ�˳���" << endl;
	cout << "1.����� 2.�����" << endl;
	cin >> n1;
	if(n1 == 1)
	{
		list1 A;
		cout << "��ѡ����Ҫʵ�ֵĹ��ܣ�" << endl;
		cout << "1.ֱ�Ӳ��� 2.��λ�ò��� 3.�滻 4.��ֵɾ�� 5.��λ��ɾ��" << endl;
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
		cout << "��ѡ����Ҫʵ�ֵĹ��ܣ�" << endl;
		cout << "1.˳����� 2.��λ�ò��� 3.�滻 4.��ֵɾ�� 5.��λ��ɾ��" << endl;
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
