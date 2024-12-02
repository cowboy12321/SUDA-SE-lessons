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
    cout << "��������Ҫ�����λ�ú�ֵ��";
    cin >> n >> x;

    Node* newNode = new Node;
    newNode->data = x;

    if (!head) {
        cout << "����Ϊ�գ��޷�����" << endl;
        return;
    }

    Node* temp = head;
    int pos = 1;  
    while (pos < n && temp->next != head) {
        temp = temp->next;
        pos++;
    }

    if (pos < n) {
        cout << "������Χ" << endl;
        delete newNode;
        return;
    }

    newNode->next = temp->next;
    temp->next = newNode;
}
void IterSingleLink::del()
{
	int n;
	cout << "��������Ҫɾ����ֵ��";
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

void IterSingleLink::search()
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

