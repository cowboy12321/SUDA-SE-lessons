#include "life.h"
#include <iostream>
using namespace std;

bool user_says_yes()
{
	int c;
	bool initial_response = true;
	do {
		if (initial_response)
			cout << "(y,n)?" << flush;
		else
		cout << "Respond with either y or n:" << flush;
		do {
			c = cin.get();
			}while(c == '\n' || c == ' ' || c == '\t');
		initial_response = false;
	}while (c != 'y' && c != 'Y' && c != 'n' && c != 'N');
	return (c == 'y' || c == 'Y');
}

void instructions( ) 
{ 
cout << "Welcome to Conway's game of Life." << endl; 
cout << "This game uses a grid of size " 
<< maxrow << " by " << maxcol << " in which each" << endl; 
cout << "cell can either be occupied by an organism or not." << endl; 
cout << "The occupied cells change from generation to generation"<< endl; 
cout << "according to how many neighboring cells are alive."<< endl; 
}

int main()
{
	life configuration;
	instructions();
	configuration.initialize();
	configuration.print();
	cout << "Continue viewing new generations?" << endl;
	while (user_says_yes())
	{
		configuration.update();
		configuration.print();
		cout << "Continue viewing new generations?" << endl;
	}
}
