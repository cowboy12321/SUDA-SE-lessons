#include "list1.h"
#include <iostream>
#include <vector>
using namespace std;

void list1::add1()
{	//ֱ�Ӳ��� 
	int n;
	cout << "��������Ҫ�����ֵ��";
	cin >> n;
	data.push_back(n);
}

void list1::add2() {
	// ��λ�ò���
	int t,n;
	cout << "��������Ҫ�����λ�ú�ֵ��";
	cin >> t >> n; 
    if (t >= 0 && t <= data.size()) {
        data.insert(data.begin() + t,n); 
    }
    else{
    	cout << "����ʧ��: ����������Χ��" << endl;
	}
}

void list1::change() {
	//�滻Ԫ�� 
	int a,b;
	cout << "��������Ҫ�ı��λ�ú�ֵ��";
	cin >> a >> b;
    data[a - 1] = b;
}
void list1::del1(){
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

void list1::del2(){
	//��λ��ɾ��
	int t; 
	cout << "��������Ҫɾ����λ�ã�";
	cin >> t; 
	data.erase(data.begin() + t);
}
void list1::print(){
	//��� 
	for(int i = 0;i < data.size();i++)
	{
		cout << data[i] << " ";
	}
	cout << endl;
}

