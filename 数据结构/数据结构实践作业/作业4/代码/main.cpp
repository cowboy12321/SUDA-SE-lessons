#include <iostream>
#include "stack1.h"
#include "cirqueue1.h"
using namespace std; 

int main() 
{    
    int n;
    cout << "请输入每个人的牌数：";
    cin >> n;
    stack1 Y(2 * n); 
	cirqueue1 X1(2 * n), X2(2 * n);
    cout << "请输入A的牌：";
    for(int i = 0; i < n; i++) {
    	int num1;
        cin >> num1;
        X1.en(num1);
    }

    cout << "请输入B的牌：";
    for(int i = 0; i < n; i++) {
    	int num2;
        cin >> num2;
        X2.en(num2);
    }

    while (!X1.isEmpty() && !X2.isEmpty()) 
	{
        int a = X1.de();  
        int b = X2.de();  
    	
        cout << "A出牌: " << a << ", B出牌: " << b << endl;
        bool t = false;
		if (!Y.isEmpty()) 
		{
    		for (int i = 0; i <= Y.get1(); i++)  
    		{
        		if (Y.get2()[i] == a)  
        		{
            		t = true;
            		X1.en(a); 
            		while (Y.get1() >= i)  
            		{
                		X1.en(Y.pop());
            		}
            		break;
        		}
    		}
		}

		if (!t)
		{
    		Y.push(a); 
		}

        t =  false;
		if (!Y.isEmpty()) 
		{
    		for (int i = 0; i <= Y.get1(); i++)  
    		{
        		if (Y.get2()[i] == b)  
        		{
            		t = true;
            		X2.en(b); 
            		while (Y.get1() >= i)  
            		{
                		X2.en(Y.pop());
            		}
            		break;
        		}
    		}
		}
		if (!t)
		{
    		Y.push(b); 
		}
		if (X1.isEmpty()) 
		{
        	cout << "A赢了！" << endl;
        	break;
    	} 
		else if (X2.isEmpty()) 
		{
        	cout << "B赢了！" << endl;
        	break;
    	}
	}

    return 0;
}

