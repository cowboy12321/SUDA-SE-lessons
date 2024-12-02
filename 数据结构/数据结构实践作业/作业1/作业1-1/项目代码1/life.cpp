#include "life.h"
#include <iostream>   
#include <fstream>    
#include <vector>     

using namespace std;

int life::neighbor_count(int row,int col)
{
	int i,j,count = 0;
	for(i = row - 1;i <= row + 1;i++)
	{
		for(j = col - 1;j <= col + 1;j++)
			count += grid[i][j];
	}
	count -= grid[row][col];
	return count;
}

void life::initialize()
{
	int row,col;
	int n;
	for(row = 0;row <= maxrow + 1;row++)
	{
		for(col = 0;col <= maxcol + 1;col++)
			grid[row][col] = 0;
	}
	cout << "List the coordinates for living cells." << endl;
	cout << "Please choose your input method:" << endl;
	cout << "  1.Keyboard Entry  ";
	cout << "2.File Import" << endl;
	cin >> n;
	if(n == 1)
	{
		cin >> row >> col;
	while(row != -1 || col != -1)
	{
		if(row == 0 || row > 20)
	{
		cout << "Row " << row << " is out of range" << endl;
		cout << "Please enter the coordinates again." << endl;
		cin >> row >> col;
	}
		if(row >= 1 && row <= maxrow)
		{
			if(col >= 1 && col <= maxcol)
				grid[row][col] = 1;
			else
			{
				cout << "Column " << col << " is out of range" << endl;
				cout << "Please enter the coordinates again." << endl;}
			cin >> row >> col;
		}
	}
	}
	else
	{
		std::ifstream file("ans.txt"); 
    	if (!file) 
		{
        	std::cerr << "Unable to open file\n";
        	return;
    	}
    	int row, col;
    	while (file >> row >> col) 
		{ 
       		if (row == -1 && col == -1) 
			{ 
            	break;
        	}
        	if (row < 1 || row > maxrow) 
			{
            	std::cout << "Row " << row << " is out of range" << std::endl;
            	continue;
        	}
        	if (col < 1 || col > maxcol) 
			{
            	std::cout << "Column " << col << " is out of range" << std::endl;
            	continue;
        	}
        	grid[row][col] = 1;
    }
    file.close(); 	
	}
}

void life::print()
{
	int row,col;
	cout << "\nThe current Life configuration is:" << endl;
	for(row = 1;row <= maxrow;row++)
	{
		for(col = 1;col <= maxcol;col++)
		{
			if(grid[row][col] == 1)
				cout << "*";
			else
				cout << "¡õ";
		}
		cout << endl;
	}
		cout << endl;
}

void life::update()
{
	int row,col,new_grid[maxrow + 2][maxcol + 2];
	for(row = 1;row <= maxrow;row++)
		for(col = 1;col <= maxcol;col++)
			switch(neighbor_count(row,col))
			{
				case 2:
					new_grid[row][col] = grid[row][col];
					break;
				case 3:
					new_grid[row][col] = 1;
					break;
				default:
					new_grid[row][col] = 0;
			}
	for(row = 1;row <= maxrow;row++)
		for(col = 1; col <= maxcol;col++)
			grid[row][col] = new_grid[row][col];
}
