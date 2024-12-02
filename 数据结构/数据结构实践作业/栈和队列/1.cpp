#include <iostream>
#include <cmath>
#include <stack>
using namespace std;

class Stack {
public:
    Stack(int);
    ~Stack();
    bool isEmpty();
    bool isFull();
    void push(int);
    int pop();
private:
    int top;
    int capacity;
    int *stack;
};

Stack::Stack(int size) {
    capacity = size;
    stack = new int[capacity];
    top = -1;
}

Stack::~Stack() {
    delete[] stack;
}

void Stack::push(int n) {
    if (isFull()) {
        return;
    }
    stack[++top] = n;
}

int Stack::pop() {
    if (isEmpty()) {
        return -1;
    }
    return stack[top--];
}

bool Stack::isEmpty() {
    return top == -1;
}

bool Stack::isFull() {
    return top == capacity - 1;
}


void detach(int n) {
	int a = n;
    stack<int> stack; 
    string s = "";
    while (n % 2 == 0) {
        stack.push(2);
        n /= 2;
    }

    for (int i = 3; i <= sqrt(n); i += 2) {
        while (n % i == 0) {
            stack.push(i);
            n /= i;
        }
    }

    if (n > 2) {
        stack.push(n);
    }

    if (!stack.empty()) {
        s = to_string(a) + "=";
        s += to_string(stack.top());
        stack.pop();

        while (!stack.empty()) {
            s += "*";
            s += to_string(stack.top());
            stack.pop();
        }
    }

    cout << s << endl;
}

int main() {
    int n;
    cin >> n;
    detach(n);
    return 0;
}

