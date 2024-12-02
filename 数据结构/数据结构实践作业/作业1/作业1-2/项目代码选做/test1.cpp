#include <iostream>
#include <time.h> 
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

int main() {
    int n;
    cin >> n;
    int* arr = new int[n];
    for (int i = 0; i < n; i++)
        cin >> arr[i];
	clock_t start_time,end_time;
	
	start_time = clock();  
    func1(n, arr);  
    end_time = clock();  
    double duration1 = static_cast<double>(end_time - start_time) * 1000.0 / CLOCKS_PER_SEC;  
    cout << duration1 << "ºÁÃë" << endl;  
  
    start_time = clock();  
    func2(n, arr);  
    end_time = clock();  
    double duration2 = static_cast<double>(end_time - start_time) * 1000.0 / CLOCKS_PER_SEC;  
    cout << duration2 << "ºÁÃë" << endl;  
  
    start_time = clock();  
    func3(n, arr);  
    end_time = clock();  
    double duration3 = static_cast<double>(end_time - start_time) * 1000.0 / CLOCKS_PER_SEC;  
    cout << duration3 << "ºÁÃë" << endl;  
    
    delete[] arr;
    return 0;
}
