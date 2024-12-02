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
	cout << "请输入想要插入的位置和值："; 
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
		cout << "超出范围" << endl;
		return; 
	}
	q -> next = p -> next;
	p -> next = q;
} 
void SingleLink::remove()
{
	int n;
	cout << "请输入需要删除的值：";
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
    cout << "请输入需要查找的位置（下标从1开始）：";
    cin >> n;
    int t = 1;
    Node* p = head;
    if (!head) {
        cout << "链表为空，查找失败。" << endl;
        return;
    }
    while (p && t < n) {  
        p = p -> next;
        t++;
    }
    if (p) {
        cout << "该位置的值为：" << p -> data << endl;
    } else {
        cout << "位置超出范围，查找失败。" << endl;
    }
}


void SingleLink::search()
{
	int x;
	int t = 0;
	cout << "请输入需要查找的值：";
	cin >> x;
	Node* p = head;
	if(!head) 
	{
		cout << "链表为空，查找失败。" << endl;
		return; 
	}
	while(p)
	{
		t += 1;
		p = p -> next;
		if(p -> data == x)
		{
			cout << "该值所处位置为（下标从1开始）：" << t << endl;
			return; 
		}
	}
	cout << "链表中没有该值，查找失败。" << endl;
	 
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
