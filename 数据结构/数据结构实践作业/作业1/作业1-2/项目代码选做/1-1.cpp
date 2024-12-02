#include <iostream>
using namespace std;

void func1(int n,int* arr)
{
	int sum = 0;
	int l = 0,r = 0;
	for(int i = 0;i < n;i++)
	{
		for(int j = n - 1;j >= i;j--)
		{
			int res = 0;
			for(int k = i;k <= j;k++)
				res += arr[k];
			if(res > sum)
			{
				sum = res;
				l = i;
				r = j;
			}
		}
	}
	cout << sum << " " << l << "-" << r << endl;
}

int main(){
	int n;
	cin >> n;
	int* arr = new int[n];
	for(int i = 0;i < n;i++)
		cin >> arr[i];
	func1(n,arr);
	delete[] arr;
	return 0;
}
