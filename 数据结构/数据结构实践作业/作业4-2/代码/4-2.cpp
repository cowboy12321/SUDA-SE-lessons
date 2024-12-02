#include <iostream>
#include <vector>
#include <string>
using namespace std;

class cirqueue
{
	public:
		cirqueue(int n)
		{
			front = rear = -1;
			qsize =  n;
			data = new int[qsize];
		}
		void en(int n)
		{
			if((rear + 1) % qsize == front)
			{
				return;
			}
			rear = (rear + 1) % qsize;
			data[rear] = n;
		}
		int de()
		{
			if(isEmpty())
			{
				return -1;
			}
			front = (front + 1) % qsize;
			int value = data[front];
			return value;
		}
		void print()
		{
			if(isEmpty())
			{
				return;
			}
			int i = (front + 1) % qsize;
			while (true)
			{
				cout << data[i];
				if(i == rear)
				{
					break;
				}
				i = (i + 1) % qsize;
			}
		}
		bool isEmpty()
		{
			return front == rear;
		}
	protected:
		int front,rear,qsize;
		int* data;
};

void func1(int n, int ans, string res, vector<string> &t)
{
    res = to_string(ans) + res;
    t.push_back(res);
    for (int i = 1; i <= ans / 2; i++) 
	{
        func1(n, i, res, t);
    }
}

void func2(vector<string> &t,vector<int> &ans)
{
	for(int i = 0;i < t.size();i++)
	{
		ans.push_back(stoi(t[i]));
	}
	for(int i = 0;i < ans.size();i++)
	{
		for(int j = i;j < ans.size();j++)
		{
			int res = 0;
			if(ans[j] > ans[j + 1])
			{
				res = ans[j];
				ans[j] = ans[j + 1];
				ans[j + 1] = res;
			}
		}
	}
}



int main() {
    int n;
    cout << "ÇëÊäÈëÊý×Ö£º"; 
    cin >> n;
    string res = "";
    vector<string> t;
    vector<int> ans;
    func1(n, n, res, t);
    func2(t,ans);
    cirqueue q(n * 2);
    for(int i = 0;i < ans.size();i++)
    {
    	q.en(ans[i]);
	}
	while(!q.isEmpty())
	{
		cout << q.de() << endl;
	}
	return 0;
}
