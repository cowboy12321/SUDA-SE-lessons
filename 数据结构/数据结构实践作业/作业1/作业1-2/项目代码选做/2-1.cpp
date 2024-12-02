#include <iostream>
using namespace std;
void RecursivePrint(int n){
	if(n > 0)
	{
		RecursivePrint(n - 1);
		cout << n << " ";
	}
}
int main()
{
	int n;
	cin >> n;
	RecursivePrint(n);
}
