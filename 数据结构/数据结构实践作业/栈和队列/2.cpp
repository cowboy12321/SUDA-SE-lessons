#include <iostream>
#include <string>
using namespace std;

class Stack {
public:
    Stack(int);
    ~Stack();
    bool isEmpty();
    bool isFull();
    void push(char);
    char pop();
    char peek();
private:
    int top;
    int capacity;
    char *stack; 
};

Stack::Stack(int size) {
    capacity = size;
    stack = new char[capacity]; 
    top = -1;
}

Stack::~Stack() {
    delete[] stack;
}

void Stack::push(char n) {
    if (!isFull()) {
        stack[++top] = n;
    }
}

char Stack::pop() {
    return stack[top--];
}

bool Stack::isEmpty() {
    return top == -1;
}

bool Stack::isFull() {
    return top == capacity - 1;
}

char Stack::peek() {
    return stack[top];
}

void stringdeal(string s) {
    Stack stack(s.size()); 
    string ans = "";
    for (int i = 0; i < s.size(); i++) {
        if (stack.isEmpty() || s[i] != stack.peek()) {
            stack.push(s[i]);
        } else {
            stack.pop(); 
        }
    }

    while (!stack.isEmpty()) {
        ans = stack.pop() + ans; 
    }
    cout << ans << endl; 
}

int main() {
    string s;
    cin >> s;
    stringdeal(s); 
    return 0;
}

