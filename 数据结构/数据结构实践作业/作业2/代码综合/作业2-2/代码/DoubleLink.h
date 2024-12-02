#ifndef DOUBLELINK_H
#define DOUBLELINK_H

class DoubleLink //双向链表(非循环) 
{
	public:
		DoubleLink();
		void create(int);
		void add();
		void remove();
		void get(); //查找第i个数据 
    	void search();//查找数据x所处位置 
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
