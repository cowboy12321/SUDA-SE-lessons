#include <iostream>
using namespace std;

class SeqStack {
private:
    int top;        // ջ��ָ��
    int capacity;   // ջ������
    int* stack;       // ջ�Ĵ洢����

public:
    SeqStack(int);
    ~SeqStack();
    bool isEmpty();
    bool isFull();
    void push(int,int*);
    pop();
    peek();
};
