#include "Stack.h"
#include <iostream>

SeqStack::SeqStack(int size) {
    capacity = size;
    stack = new int[capacity];
    top = -1;
}

SeqStack::~SeqStack() {
    delete[] stack;
}

void SeqStack::push(int value,int* Stack) {
    if (isFull()) {
        return;
    }
	Stack[++top] = value;
}

SeqStack::pop() {
    if (isEmpty()) {
        return -1;
    }
    return stack[top--];
}

SeqStack::peek() {
    if (isEmpty()) {
        cout << "Õ»ÊÇ¿ÕµÄ." << endl;
        return -1;
    }
    return stack[top];
}
bool SeqStack::isEmpty() 
{
    return top == -1;
}

bool SeqStack::isFull() 
{
    return top == capacity - 1;
}
