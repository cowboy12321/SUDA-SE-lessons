#include <iostream>
using namespace std;

void func3(int n,int *arr)
{
	int sum = 0;
	int res = 0;
	int l = 0,r = 0;
	int t = 0;
	for(int i = 0;i < n;i++)
	{
		res += arr[i];
		if(res < 0)
		{
			res = 0;
			t = i + 1;
		}
		if(res >= sum)
		{
			sum = res;
			l = t;
			r = i;
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
	func3(n,arr);
	delete[] arr;
	return 0;
}
