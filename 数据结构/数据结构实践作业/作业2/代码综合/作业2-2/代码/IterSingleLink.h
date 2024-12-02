#ifndef ITERSINGLELINK_H
#define ITERSINGLELINK_H

class IterSingleLink //����ѭ������ 
{
	public:
		IterSingleLink();
		void insert(int);
		void add();
		void del();
		void get(); //���ҵ�i������ 
    	void search();//��������x����λ�� 
		void print();
		void destroy();
	private:
		class Node 
		{
			public:
    			int data;   
    			Node* next;
		};
		Node* head; 
};

#endif
