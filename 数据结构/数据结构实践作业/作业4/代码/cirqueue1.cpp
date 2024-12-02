#include "cirqueue1.h"
#include <iostream>
using namespace std;

cirqueue1::cirqueue1(int n)
{
	front = rear = -1;
	qsize = n;
	data = new int[n];
}
void cirqueue1::en(int n)
{
	if((rear + 1) % qsize == front)
	{
		return;
	}
	rear = (rear + 1) % qsize;
	data[rear] = n;
}
int cirqueue1::de()
{
	if(isEmpty())
	{
		return -1;
	}
	front = (front + 1) % qsize;
	int value = data[front];
	return value;
}
void cirqueue1::print()
{
	if(isEmpty())
	{
		return;
	}
	int i = (front + 1) % qsize;
	while(true)
	{
		cout <<data[i] << " ";
		if(i == rear)
		{
			break;
		}	
		i = (i + 1) % qsize;
	}
}
bool cirqueue1::isEmpty()
{
	return front == rear;
}

