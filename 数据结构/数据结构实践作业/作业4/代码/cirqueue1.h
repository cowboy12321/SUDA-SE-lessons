#ifndef CIRQUEUE1_H
#define CIRQUEUE1_H

class cirqueue1
{
	public:
		cirqueue1(int);
		void en(int);
		int de();
		void print();
		bool isEmpty();
	protected:
		int front,rear,qsize;
		int* data;
};

#endif
