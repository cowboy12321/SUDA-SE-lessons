#include "SingleLink.h"
#include <iostream>
using namespace std;

SingleLink::SingleLink() {
    head = NULL;
}

void SingleLink::insert(int n) {
    Node* p1 = new Node;
    p1 -> data = n;
    p1 -> next = NULL; 
    if (!head) {
        head = p1; 
    } else {
        Node* t = head;
        while (t->next != NULL) {
            t = t->next; 
        }
        t->next = p1; 
    }
}


void SingleLink::add()
{
	int n;
	int x;
	cout << "��������Ҫ�����λ�ú�ֵ��"; 
	cin >> n >> x; 
	Node* p = head;
	Node* q = new Node;
	q -> data = x;
	int t = 0;
	if(n == 1)
	{
		q -> next = head -> next;
		head -> next = q;
		return;
	}
	for(int i = 0;i < n;i++)
	{
		if(p -> next)
		{
			t += 1;
			p = p -> next;
		}	
	}
	if(t < n)
	{
		cout << "������Χ" << endl;
		return; 
	}
	q -> next = p -> next;
	p -> next = q;
} 
void SingleLink::remove()
{
	int n;
	cout << "��������Ҫɾ����ֵ��";
	cin >> n;
	if(!head)
	{
		return;
	}
	else
	{
		if(head -> data == n)
		{
			Node* t = head;
			head = head -> next;
			delete t;
			return;
		}
		else
		{
			Node* p1 = head;
			while(p1 -> next)
			{
				if(p1 -> next -> data == n)
				{
					Node* p2 = p1 -> next;
					p1 -> next = p1 -> next -> next;
					delete p2;
					return;
				}
				p1 = p1 -> next;
			}
		}
	}
}
void SingleLink::print()
{
	Node* t = head;
	while(t != NULL)
	{
		cout << t -> data << " ";
		t = t -> next;
	}
	cout << endl;
}

void SingleLink::get() {
    int n;
    cout << "��������Ҫ���ҵ�λ�ã��±��1��ʼ����";
    cin >> n;
    int t = 1;
    Node* p = head;
    if (!head) {
        cout << "����Ϊ�գ�����ʧ�ܡ�" << endl;
        return;
    }
    while (p && t < n) {  
        p = p -> next;
        t++;
    }
    if (p) {
        cout << "��λ�õ�ֵΪ��" << p -> data << endl;
    } else {
        cout << "λ�ó�����Χ������ʧ�ܡ�" << endl;
    }
}


void SingleLink::search()
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
	while(p)
	{
		t += 1;
		p = p -> next;
		if(p -> data == x)
		{
			cout << "��ֵ����λ��Ϊ���±��1��ʼ����" << t << endl;
			return; 
		}
	}
	cout << "������û�и�ֵ������ʧ�ܡ�" << endl;
	 
}

void SingleLink::destroy()
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
