#include <iostream>
#include <vector>
using namespace std;

void dfs(int n,vector<vector<int>>& num,vector<bool>& visited)
{
	visited[n] = true;
	for(size_t i = 0;i < num.size();i++)
	{
		if(num[n][i] == 1 && visited[i] == 0)
		{
			dfs(i,num,visited);
		}
	}
}

int count(vector<vector<int>>& num)
{
	size_t n = num.size();
	vector<bool> visited(n,false);
	int t = 0;
	for(size_t i = 0;i < n;i++)
	{
		if(!visited[i])
		{
			dfs(i,num,visited);
			t++;
		}
	}
	return t;
}

int main()
{
	int n;
	cin >> n;
	vector<vector<int>> num(n,vector<int>(n));
	for(int i = 0;i < n;i++)
	{
		for(int j = 0;j < n;j++)
		{
			cin >> num[i][j];
		}
	}
	int ans = count(num);
	cout << ans;
	return 0;
}
