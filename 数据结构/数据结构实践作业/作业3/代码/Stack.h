#include <iostream>
using namespace std;

class SeqStack {
private:
    int top;        // 栈顶指针
    int capacity;   // 栈的容量
    int* stack;       // 栈的存储数组

public:
    SeqStack(int);
    ~SeqStack();
    bool isEmpty();
    bool isFull();
    void push(int,int*);
    pop();
    peek();
};
