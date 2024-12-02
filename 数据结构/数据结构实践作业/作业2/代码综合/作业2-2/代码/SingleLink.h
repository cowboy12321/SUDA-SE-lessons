#ifndef SINGLELINK_H
#define SINGLELINK_H

class SingleLink //单链表类 
{
	public:
		SingleLink();
    	void insert(int);//直接插入末尾 
    	void add();//中间插入 
    	void remove();//删除特定值 
    	void get(); //查找第i个数据 
    	void search();//查找数据x所处位置 
		void print(); //输出 
		void destroy();//销毁 
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
