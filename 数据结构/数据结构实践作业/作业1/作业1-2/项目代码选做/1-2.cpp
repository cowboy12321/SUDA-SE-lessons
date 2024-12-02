#include <iostream>
using namespace std;

void func2(int n,int* arr)
{
	int sum = 0;
	int l = 0,r = 0;
	for(int i = 0;i < n;i++)
	{
		int res = 0;
		for(int j = i;j < n;j++)
		{
			res += arr[j];
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
	func2(n,arr);
	delete[] arr;
	return 0;
}
