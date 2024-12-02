#include "stack1.h"
#include <iostream>
using namespace std;

stack1::stack1(int size)
{
	capacity = size;
	top = -1;
}

bool stack1::isEmpty()
{
	return top == -1;
}

void stack1::push(int n)
{
    if(top + 1 < capacity)  
    {
        stack.push_back(n); 
        ++top;  
    }
}
int stack1::pop()
{
	if(isEmpty())
	{
		return -1; 
	}
	int value = stack[top];
	stack[top--];
	return value;
}
int stack1::peek()
{
	return pop();
}
int stack1::get1()
{
	return top;
}
vector<int> stack1::get2()
{
	return stack;
}
