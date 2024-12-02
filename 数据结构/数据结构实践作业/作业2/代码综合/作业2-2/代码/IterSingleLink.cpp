#include "IterSingleLink.h"
#include <iostream>
using namespace std;

IterSingleLink::IterSingleLink()
{
	head = NULL;
}
void IterSingleLink::insert(int n) {
    Node* newNode = new Node;
    newNode -> data = n;
    if (!head) {
        head = newNode;
        head->next = head; 
    } else {
        Node* temp = head;
        while (temp->next != head) {
            temp = temp->next;
        }
        temp->next = newNode;
        newNode->next = head; 
    }
}

void IterSingleLink::add() {
    int x, n;
    cout << "请输入想要插入的位置和值：";
    cin >> n >> x;

    Node* newNode = new Node;
    newNode->data = x;

    if (!head) {
        cout << "链表为空，无法插入" << endl;
        return;
    }

    Node* temp = head;
    int pos = 1;  
    while (pos < n && temp->next != head) {
        temp = temp->next;
        pos++;
    }

    if (pos < n) {
        cout << "超出范围" << endl;
        delete newNode;
        return;
    }

    newNode->next = temp->next;
    temp->next = newNode;
}
void IterSingleLink::del()
{
	int n;
	cout << "请输入需要删除的值：";
	cin >> n;
	if(!head) return;
	Node* t = head;
	Node* p = head;
	do{
		if(t -> data == n){
			if(t == head){
				if(head -> next == head)
				{
					delete head;
					head = NULL;
					return;
				}
				while(p -> next != head)
				{
					p = p -> next;
				}
				p -> next = head -> next;
				head = head -> next;
				delete t;
				return;
			}
			else{
				while(p -> next)
				{
					if(p -> next -> data == n){
						Node* p1 = p -> next;
						p -> next = p1 -> next;
						delete p1;
						return;
					}
					p = p -> next;
				}
			}
		}
		t = t -> next;
	} while(t -> next != head);
	
}

void IterSingleLink::get()
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

void IterSingleLink::search()
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

void IterSingleLink::destroy() {
    if (!head) return;
    
    Node* p = head;
    Node* temp;
    do {
        temp = p;
        p = p->next;
        delete temp;
    } while (p != head);

    head = NULL; 
}

void IterSingleLink::print()  {
    if (!head) return;
    Node* temp = head;
    do {
        cout << temp->data << " ";
        temp = temp->next;
    } while (temp != head);
    cout << endl;
}

