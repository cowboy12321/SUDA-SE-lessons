#ifndef ITERSINGLELINK_H
#define ITERSINGLELINK_H

class IterSingleLink //单向循环链表 
{
	public:
		IterSingleLink();
		void insert(int);
		void add();
		void del();
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
		};
		Node* head; 
};

#endif
