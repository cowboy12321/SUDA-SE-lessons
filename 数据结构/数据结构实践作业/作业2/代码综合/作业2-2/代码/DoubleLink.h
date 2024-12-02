#ifndef DOUBLELINK_H
#define DOUBLELINK_H

class DoubleLink //˫������(��ѭ��) 
{
	public:
		DoubleLink();
		void create(int);
		void add();
		void remove();
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
    			Node* prior;
		};
		Node* head;
};

#endif
