#include <iostream>
using namespace std;

char nums[2001],c;
int t = 0;

int count(int n){
    if (n > t || nums[n] == '#') return 0;
    int left = count(n*2);
    int right = count(n*2+1);
    return left + right + (nums[n*2] != '#' && nums[n*2+1] != '#');
}

void read(int n){

    t = t > n ? t : n;
    while ((c = getchar()) == ' ') continue;
    nums[n] = c;
    if (c == '#') return;

    read(n*2);
    read(n*2+1);
}

int main() {
    read(1);
    cout<<count(1);
    return 0;
}
