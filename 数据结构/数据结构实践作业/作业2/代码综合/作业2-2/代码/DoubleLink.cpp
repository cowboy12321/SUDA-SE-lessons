#include "DoubleLink.h"
#include <iostream>
using namespace std;

DoubleLink::DoubleLink()
{
	head = NULL;
}

void DoubleLink::create(int n) {
    Node* newNode = new Node;
    newNode->data = n;
    newNode->next = NULL;
    if (!head) { // �������Ϊ��
        newNode->prior = NULL;
        head = newNode;
    } else {
        Node* p = head;
        while (p->next) { // �ҵ������β��
            p = p->next;
        }
        p->next = newNode;
        newNode->prior = p;
    }
}

void DoubleLink::add()
{
    int n, x;
    cout << "��������Ҫ�����λ�ú�ֵ��";
    cin >> n >> x;

    Node* p = head;
    Node* q = new Node;
    q->data = x;

    int t = 1; 
    while (p && t < n) {
        p = p->next;
        t++;
    }

    if (!p || t != n) {
        cout << "������Χ" << endl;
        delete q; 
        return;
    }

    q->prior = p;
    q->next = p->next;
    if (p->next) {
        p->next->prior = q;
    }
    p->next = q;
}


void DoubleLink::remove()
{
    int n;
    cout << "��������Ҫɾ����ֵ��";
    cin >> n;
    if (!head) {
        cout << "����Ϊ�գ��޷�ɾ��" << endl;
        return;
    }
    Node* p = head;
    int t = 0;

    while (p && p->data != n) {
        p = p->next;
    }

    if (!p) {
        cout << "������û�и�ֵ��ɾ��ʧ�ܡ�" << endl;
        return;
    }

    if (p == head) {
        head = head->next; 
        if (head) head->prior = NULL;
    } else {
        p->prior->next = p->next;
        if (p->next) {
            p->next->prior = p->prior;
        }
    }

    delete p; 
}

void DoubleLink::print()  {
    if (!head) return;
    Node* temp = head;
    do {
        cout << temp->data << " ";
        temp = temp->next;
    } while (temp != NULL);
    cout << endl;
}


void DoubleLink::destroy()
{
	Node* p = head;
	while(p != NULL)
	{
		Node* p1 = p;
		p = p -> next;
		delete p1;
	}
	head = NULL;
} 


void DoubleLink::get()
{
	int n;
	cout << "��������Ҫ���ҵ�λ�ã��±��1��ʼ����";
	cin >> n;
	int t = 0;
	Node* p = head;
	if(!head) 
	{
		cout << "����Ϊ�գ�����ʧ�ܡ�" << endl;
		return; 
	}
	while(p -> next != head)
	{
		t += 1;
		if(t == n)
		{
			cout << "��λ�õ�ֵΪ��" << p -> data << endl;
			return; 
		}
		p = p -> next;
	}
	if(t == n - 1)
	{
		cout << "��λ�õ�ֵΪ��" << p -> data << endl; 
		return;
	}
	if(t < n)
	{
		cout << "λ�ó�����Χ������ʧ�ܡ�" << endl;
		return; 
	}
} 

void DoubleLink::search()
{
	int x;
	int t = 0;
	cout << "��������Ҫ���ҵ�ֵ��";
	cin >> x;
	Node* p = head;
	if(!head) 
	{
		cout << "����Ϊ�գ�����ʧ�ܡ�" << endl;
		return; 
	}
	if(head -> data == x)
	{
		cout << "��ֵ����λ��Ϊ���±��1��ʼ����" << "1" << endl;
		return;
	}
	while(p -> next != head)
	{
		t += 1;
		if(p -> data == x)
		{
			cout << "��ֵ����λ��Ϊ���±��1��ʼ����" << t << endl;
			return; 
		}
		p = p -> next;
	}
	cout << "������û�и�ֵ������ʧ�ܡ�" << endl;
	 
}
