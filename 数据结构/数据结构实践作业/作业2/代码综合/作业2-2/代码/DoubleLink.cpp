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
    if (!head) { // 如果链表为空
        newNode->prior = NULL;
        head = newNode;
    } else {
        Node* p = head;
        while (p->next) { // 找到链表的尾部
            p = p->next;
        }
        p->next = newNode;
        newNode->prior = p;
    }
}

void DoubleLink::add()
{
    int n, x;
    cout << "请输入想要插入的位置和值：";
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
        cout << "超出范围" << endl;
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
    cout << "请输入需要删除的值：";
    cin >> n;
    if (!head) {
        cout << "链表为空，无法删除" << endl;
        return;
    }
    Node* p = head;
    int t = 0;

    while (p && p->data != n) {
        p = p->next;
    }

    if (!p) {
        cout << "链表中没有该值，删除失败。" << endl;
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
	cout << "请输入需要查找的位置（下标从1开始）：";
	cin >> n;
	int t = 0;
	Node* p = head;
	if(!head) 
	{
		cout << "链表为空，查找失败。" << endl;
		return; 
	}
	while(p -> next != head)
	{
		t += 1;
		if(t == n)
		{
			cout << "该位置的值为：" << p -> data << endl;
			return; 
		}
		p = p -> next;
	}
	if(t == n - 1)
	{
		cout << "该位置的值为：" << p -> data << endl; 
		return;
	}
	if(t < n)
	{
		cout << "位置超出范围，查找失败。" << endl;
		return; 
	}
} 

void DoubleLink::search()
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
	if(head -> data == x)
	{
		cout << "该值所处位置为（下标从1开始）：" << "1" << endl;
		return;
	}
	while(p -> next != head)
	{
		t += 1;
		if(p -> data == x)
		{
			cout << "该值所处位置为（下标从1开始）：" << t << endl;
			return; 
		}
		p = p -> next;
	}
	cout << "链表中没有该值，查找失败。" << endl;
	 
}
