#ifndef SINGLELINK_H
#define SINGLELINK_H

class SingleLink //�������� 
{
	public:
		SingleLink();
    	void insert(int);//ֱ�Ӳ���ĩβ 
    	void add();//�м���� 
    	void remove();//ɾ���ض�ֵ 
    	void get(); //���ҵ�i������ 
    	void search();//��������x����λ�� 
		void print(); //��� 
		void destroy();//���� 
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
