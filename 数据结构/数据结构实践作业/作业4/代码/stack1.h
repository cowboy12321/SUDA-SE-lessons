#ifndef STACK1_H
#define STACK1_H
#include <vector>
using namespace std;
class stack1
{
	public:
		stack1(int);
		void push(int);
		int pop();
		bool isEmpty();
		int peek();
		int get1();
		vector<int> get2();
	protected:
		int top;
		int capacity;
		vector<int> stack;
};

#endif
