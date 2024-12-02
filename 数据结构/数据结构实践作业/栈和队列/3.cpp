#include <iostream>
#include <vector>
using namespace std;

class CirQueue {
public:
    CirQueue(int);
    ~CirQueue();
    void EnQueue(int);
    int DeQueue();
    int Empty();
    void PrintOut();
private:
    int front, rear, qsize;
    int* data;
};

CirQueue::CirQueue(int n) : front(-1), rear(-1), qsize(n) {
    data = new int[n];
}

CirQueue::~CirQueue() {
    delete[] data;
}

int CirQueue::Empty() {
    return rear == front; 
}

void CirQueue::EnQueue(int n) {
    if ((rear + 1) % qsize == front) { 
        return;
    }
    rear = (rear + 1) % qsize;
    data[rear] = n;
}

int CirQueue::DeQueue() {
    if (Empty()) {
        return -1; 
    }
    front = (front + 1) % qsize;
    int value = data[front];
    return value;
}

void CirQueue::PrintOut() {
    if (Empty()) {
        cout << "Queue is empty!" << endl;
        return;
    }
    int i = (front + 1) % qsize;
    while (true) {
        cout << data[i] << " ";
        if (i == rear) break; 
        i = (i + 1) % qsize; 
    }
    cout << endl;
}

void reverseOdd(CirQueue& sq, int n) {
    vector<int> arr; 
    for (int i = 0; i < n; i++) {
        int value = sq.DeQueue();
        if (value != -1) {
            if (value % 2 != 0) {
                arr.push_back(value);
            } 
            sq.EnQueue(value); 
        }
    }

    int k = arr.size() - 1;
    for (int i = 0; i < n; i++) {
        int value = sq.DeQueue(); 
        if (value % 2 != 0) {
            sq.EnQueue(arr[k]);
            k--;
        } else {
            sq.EnQueue(value); 
        }
    }
}

int main() {
    int n, item;
    cin >> n;
    CirQueue sq(n + 1);
    for (int i = 0; i < n; i++) {
        cin >> item;
        sq.EnQueue(item);
    }
    reverseOdd(sq, n);
    sq.PrintOut();
    return 0;
}

