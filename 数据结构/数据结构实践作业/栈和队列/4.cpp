#include <iostream>
#include <stack>
#include <string>

using namespace std;

class QueueUsingStacks {
private:
    stack<int> stack1; 
    stack<int> stack2; 

public:
    void enqueue(int value) {
        stack1.push(value); 
    }

    int dequeue() {
        if (stack2.empty()) { 
            while (!stack1.empty()) {
                stack2.push(stack1.top());
                stack1.pop();
            }
        }
        
        
        int frontValue = stack2.top(); 
        stack2.pop(); 
        return frontValue;
    }

    bool isEmpty() {
        return stack1.empty() && stack2.empty(); // 判断队列是否为空
    }
};

int main() {
    int n;
    cin >> n; 

    QueueUsingStacks queue; 

    for (int i = 0; i < n; i++) {
        int item;
        cin >> item;
        queue.enqueue(item);
    }

    int m;
    cin >> m; 

    for (int i = 0; i < m; i++) {
        string operation;
        cin >> operation; 
        
        if (operation == "D") {
            if (!queue.isEmpty()) {
                cout << queue.dequeue() << endl; 
            } else {
                cout << "Queue is empty" << endl; 
            }
        } else {
            int value = stoi(operation); 
            queue.enqueue(value); 
        }
    }

    return 0;
}

